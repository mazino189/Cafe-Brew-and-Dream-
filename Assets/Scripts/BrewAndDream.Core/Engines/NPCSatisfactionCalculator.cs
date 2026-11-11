using System;
using System.Collections.Generic;
using BrewAndDream.Core.Models;

namespace BrewAndDream.Core.Engines
{
    public static class NPCSatisfactionCalculator
    {
        public static float CalculateCraftingExecution01(DrinkRecipeModel recipe, List<CraftedStepLog> logs)
        {
            if (recipe == null || recipe.craftingSteps.Count == 0) return 1f;
            if (logs == null || logs.Count == 0) return 0f;

            float weightedScoreSum = 0f;
            float totalImportance = 0f;

            for (int i = 0; i < recipe.craftingSteps.Count; i++)
            {
                RecipeStep step = recipe.craftingSteps[i];
                totalImportance += step.stepImportance;

                int foundIndex = logs.FindIndex(l => l.stepIndex == i);
                if (foundIndex < 0) continue;

                CraftedStepLog log = logs[foundIndex];
                float absError = Math.Abs(log.actualAmount - step.targetAmount);

                float stepAccuracy;
                if (absError <= step.allowedTolerance)
                {
                    stepAccuracy = 1f;
                }
                else if (absError >= step.maxErrorThreshold)
                {
                    stepAccuracy = 0f;
                }
                else
                {
                    float range = Math.Max(0.0001f, step.maxErrorThreshold - step.allowedTolerance);
                    stepAccuracy = 1f - ((absError - step.allowedTolerance) / range);
                }

                if (step.requireStrictOrder && !log.wasCorrectOrder)
                {
                    stepAccuracy *= 0.7f;
                }

                weightedScoreSum += stepAccuracy * step.stepImportance;
            }

            return Math.Clamp(weightedScoreSum / Math.Max(0.0001f, totalImportance), 0f, 1f);
        }

        public static float CalculateTasteMatch01(FlavorProfile actual, FlavorProfile preferred, float tolerance)
        {
            float dSweet = actual.sweetness - preferred.sweetness;
            float dBitter = actual.bitterness - preferred.bitterness;
            float dCream = actual.creaminess - preferred.creaminess;
            float dAroma = actual.aroma - preferred.aroma;
            float dNovelty = actual.novelty - preferred.novelty;

            float distance = (float)Math.Sqrt((dSweet * dSweet + dBitter * dBitter + dCream * dCream + dAroma * dAroma + dNovelty * dNovelty) / 5.0);

            if (distance <= tolerance * 0.5f) return 1f;
            float normalizedDiff = (distance - tolerance * 0.5f) / Math.Max(0.05f, 1f - tolerance * 0.5f);
            return Math.Clamp(1f - normalizedDiff, 0f, 1f);
        }

        public static EvaluationOutput EvaluateService(NPCPersonalityModel npc, ServiceEvaluationContext ctx, Random random = null)
        {
            if (random == null) random = new Random();

            // 1. Tốc độ phục vụ
            float sSpeed;
            if (ctx.actualWaitTimeSeconds <= npc.idealWaitTimeSeconds)
            {
                sSpeed = 1f;
            }
            else if (ctx.actualWaitTimeSeconds >= npc.maxWaitTimeSeconds)
            {
                sSpeed = 0f;
            }
            else
            {
                float ratio = (ctx.actualWaitTimeSeconds - npc.idealWaitTimeSeconds) /
                              Math.Max(1f, npc.maxWaitTimeSeconds - npc.idealWaitTimeSeconds);
                sSpeed = Math.Clamp(1f - (float)Math.Pow(ratio, npc.waitDecayExponent), 0f, 1f);
            }

            // 2. Chất lượng đồ uống
            float eCraft = CalculateCraftingExecution01(ctx.orderedRecipe, ctx.playerStepLogs);
            float mTaste = CalculateTasteMatch01(ctx.actualFlavorProfile, npc.preferredFlavor, npc.tasteTolerance);
            float sQuality = Math.Clamp(0.70f * eCraft + 0.30f * mTaste, 0f, 1f);

            // 3. Thái độ, Vệ sinh, Không gian
            float sAttitude = Math.Clamp(ctx.playerAttitudeScore01, 0f, 1f);
            float sHygiene = Math.Clamp(ctx.shopHygieneScore01, 0f, 1f);
            float sAmbiance = Math.Clamp(ctx.shopAmbianceMatch01, 0f, 1f);

            // 4. Giá bán
            float priceRatio = (ctx.sellingPriceVND - npc.expectedBudgetVND) / Math.Max(1000f, npc.expectedBudgetVND);
            float sPrice;
            if (priceRatio <= 0f)
            {
                sPrice = 1f;
            }
            else
            {
                sPrice = Math.Clamp((float)Math.Exp(-npc.priceSensitivityBeta * priceRatio), 0f, 1f);
            }

            // 5. Trọng số & Strictness
            EvaluationWeights w = npc.weights;
            float wSum = w.TotalWeight;

            float cSpeed = (float)Math.Pow(sSpeed, npc.strictnessAlpha);
            float cQuality = (float)Math.Pow(sQuality, npc.strictnessAlpha);
            float cAttitude = (float)Math.Pow(sAttitude, npc.strictnessAlpha);
            float cHygiene = (float)Math.Pow(sHygiene, npc.strictnessAlpha);
            float cAmbiance = (float)Math.Pow(sAmbiance, npc.strictnessAlpha);
            float cPrice = (float)Math.Pow(sPrice, npc.strictnessAlpha);

            float weightedRaw = (
                w.speedWeight * cSpeed +
                w.qualityWeight * cQuality +
                w.attitudeWeight * cAttitude +
                w.hygieneWeight * cHygiene +
                w.ambianceWeight * cAmbiance +
                w.priceWeight * cPrice
            ) / wSum;

            // Critical Veto: đồ uống quá tệ (<0.2) hoặc chờ quá lâu (<=0.01)
            float criticalMultiplier = (sQuality < 0.2f || sSpeed <= 0.01f) ? 0.45f : 1.0f;
            float finalScore100 = Math.Clamp(weightedRaw * criticalMultiplier * 100f, 0f, 100f);

            int stars = Math.Clamp((int)Math.Ceiling(finalScore100 / 20.0), 1, 5);
            bool willReturn = finalScore100 >= npc.satisfactionThreshold;

            float extremityBoost = Math.Abs(finalScore100 - 50f) / 50f;
            float reviewProb = Math.Clamp(npc.baseReviewProbability * (0.6f + 0.8f * extremityBoost), 0f, 1f);
            bool willReview = (float)random.NextDouble() <= reviewProb;

            float repDelta = ((finalScore100 - npc.satisfactionThreshold) / 10f) * npc.reputationImpactMultiplier;

            (string praise, string complaint) = FindDominantCriteria(
                cSpeed, cQuality, cAttitude, cHygiene, cAmbiance, cPrice
            );

            return new EvaluationOutput
            {
                finalSatisfactionScore = finalScore100,
                starRating = stars,
                willReturn = willReturn,
                willWriteReview = willReview,
                reputationDelta = repDelta,
                dominantPraiseCriterion = praise,
                dominantComplaintCriterion = complaint
            };
        }

        private static (string praise, string complaint) FindDominantCriteria(
            float speed, float quality, float attitude, float hygiene, float ambiance, float price)
        {
            (string name, float val)[] items = new[]
            {
                ("Tốc độ phục vụ", speed),
                ("Chất lượng đồ uống", quality),
                ("Thái độ phục vụ", attitude),
                ("Vệ sinh quán", hygiene),
                ("Không gian & Âm nhạc", ambiance),
                ("Giá cả", price)
            };

            string best = items[0].name;
            float maxVal = items[0].val;
            string worst = items[0].name;
            float minVal = items[0].val;

            for (int i = 1; i < items.Length; i++)
            {
                if (items[i].val > maxVal) { maxVal = items[i].val; best = items[i].name; }
                if (items[i].val < minVal) { minVal = items[i].val; worst = items[i].name; }
            }

            return (best, worst);
        }
    }
}

