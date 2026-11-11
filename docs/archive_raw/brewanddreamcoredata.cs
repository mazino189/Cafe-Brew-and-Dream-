using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrewAndDream.Core
{
    // =========================================================================
    // 1. ENUMS & CẤU TRÚC PHỤ TRỢ (ENUMS & HELPER STRUCTS)
    // =========================================================================

    public enum ShopTheme
    {
        VietnameseTraditionalCoffee, // Cafe truyền thống Việt Nam (phin, bạc xỉu)
        SpecialtyCoffee,             // Specialty coffee
        MilkTea,                     // Trà sữa
        MatchaJapanese,              // Matcha / Nhật
        BookCafe,                    // Quán sách
        StudentStudyCafe,            // Quán cho sinh viên học bài
        AcousticCafe                 // Quán acoustic
    }

    public enum NPCArchetype
    {
        StudentNeedWifi,     // Sinh viên cần wifi và ổ điện
        BusyOfficeWorker,    // Dân văn phòng vội
        StrictAuntie,        // Bà cô khó tính
        CoffeeCritic,        // Critic cafe
        DatingCouple,        // Cặp đôi hẹn hò
        BloggerTikToker,     // Blogger / TikToker
        FriendlyRegular,     // Khách quen dễ thương
        Troublemaker         // Khách kiếm chuyện
    }

    public enum SocialBehaviorType
    {
        OnlineReviewer,      // Hay review trên app đánh giá trong game (ảnh hưởng danh tiếng khu vực)
        WordOfMouthSpreader, // Hay kể bạn bè (tăng lượng khách mới hôm sau)
        SilentLeaver         // Im lặng bỏ đi nếu không hài lòng, ít viết review
    }

    public enum CraftingActionType
    {
        PlaceCup,            // Đặt ly
        PourLiquid,          // Đong / đổ chất lỏng (ml)
        AddSolidOrPowder,    // Thêm bột / đá / topping (gram hoặc viên)
        BrewMachineOrPhin,   // Bấm máy chiết xuất hoặc ủ phin (giây)
        StirOrShake,         // Khuấy hoặc lắc đều (giây)
        Serve                // Đặt lên khay giao khách
    }

    /// <summary>
    /// Hồ sơ hương vị chuẩn hóa trên thang [0, 1] để so khớp giữa Đồ uống và Gu vị của NPC.
    /// </summary>
    [Serializable]
    public struct FlavorProfile
    {
        [Range(0f, 1f)] public float sweetness;  // Độ ngọt
        [Range(0f, 1f)] public float bitterness; // Độ đắng / đậm cafe hoặc trà
        [Range(0f, 1f)] public float creaminess; // Độ béo ngậy (sữa đặc, kem béo)
        [Range(0f, 1f)] public float aroma;      // Hương thơm đặc trưng
        [Range(0f, 1f)] public float novelty;    // Độ độc lạ (0: món truyền thống quen thuộc, 1: món sáng tạo lạ miệng)
    }

    /// <summary>
    /// Trọng số 6 tiêu chí đánh giá của mỗi NPC (Sẽ được chuẩn hóa để tổng bằng 1.0 khi tính toán).
    /// </summary>
    [Serializable]
    public struct EvaluationWeights
    {
        [Min(0f)] public float speedWeight;       // w_speed: Tốc độ phục vụ
        [Min(0f)] public float qualityWeight;     // w_quality: Hương vị & độ chuẩn công thức
        [Min(0f)] public float attitudeWeight;    // w_attitude: Thái độ phục vụ (lời chào, giao đúng món, xử lý sự cố)
        [Min(0f)] public float hygieneWeight;     // w_hygiene: Vệ sinh quầy và bàn ghế
        [Min(0f)] public float ambianceWeight;    // w_ambiance: Không gian, âm nhạc, wifi/ổ điện, độ ồn
        [Min(0f)] public float priceWeight;       // w_price: Độ hợp lý của giá bán so với ngân sách

        public float TotalWeight =>
            Mathf.Max(0.0001f, speedWeight + qualityWeight + attitudeWeight + hygieneWeight + ambianceWeight + priceWeight);
    }

    // =========================================================================
    // 2. SCRIPTABLE OBJECT: NGUYÊN LIỆU & CÔNG THỨC ĐỒ UỐNG (RECIPE SYSTEM)
    // =========================================================================

    [CreateAssetMenu(fileName = "NewIngredient", menuName = "BrewAndDream/Data/Ingredient Data")]
    public class IngredientDataSO : ScriptableObject
    {
        [Header("Thông tin cơ bản")]
        public string ingredientId;
        public string displayName;
        public Sprite icon;
        public Color liquidOrVisualColor = Color.white;

        [Header("Kinh tế & Kho")]
        [Tooltip("Chi phí nhập trên mỗi đơn vị chuẩn (VNĐ / ml hoặc VNĐ / gram)")]
        public float unitCostVND;
        [Tooltip("Số ngày bảo quản tối đa trước khi hỏng")]
        public int shelfLifeDays = 3;
    }

    [Serializable]
    public struct RecipeStep
    {
        public string stepDescription;
        public CraftingActionType actionType;
        public IngredientDataSO requiredIngredient;

        [Tooltip("Định lượng mục tiêu (ml, gram, hoặc số giây chiết xuất/khuấy)")]
        public float targetAmount;

        [Tooltip("Sai số cho phép không bị trừ điểm (ví dụ: +/- 5ml)")]
        public float allowedTolerance;

        [Tooltip("Sai số tối đa trước khi bước này tính là hỏng hoàn toàn (0 điểm)")]
        public float maxErrorThreshold;

        [Tooltip("Trọng số quan trọng của bước này trong tổng chất lượng ly nước")]
        [Range(0.1f, 5f)] public float stepImportance;

        [Tooltip("Nếu bật, người chơi phải thực hiện đúng thứ tự bước này")]
        public bool requireStrictOrder;
    }

    [CreateAssetMenu(fileName = "NewDrinkRecipe", menuName = "BrewAndDream/Data/Drink Recipe")]
    public class DrinkRecipeSO : ScriptableObject
    {
        [Header("Thông tin chung")]
        public string recipeId;
        public string drinkName;
        [TextArea(2, 4)] public string description;
        public Sprite menuIcon;
        public List<ShopTheme> suitableThemes = new List<ShopTheme>();

        [Header("Định giá & Mở khóa")]
        [Tooltip("Giá thị trường tham chiếu (VNĐ) cho món này")]
        public int referenceMarketPriceVND = 35000;
        public bool unlockedFromStart = true;

        [Header("Hồ sơ hương vị khi pha chuẩn 100%")]
        public FlavorProfile idealFlavorProfile;

        [Header("Các bước pha chế góc nhìn thứ nhất (FPS Steps)")]
        public List<RecipeStep> craftingSteps = new List<RecipeStep>();

        /// <summary>
        /// Tính tổng giá vốn nguyên liệu (COGS) cho 1 ly chuẩn.
        /// </summary>
        public float CalculateBaseIngredientCost()
        {
            float totalCost = 0f;
            foreach (var step in craftingSteps)
            {
                if (step.requiredIngredient != null)
                {
                    totalCost += step.requiredIngredient.unitCostVND * step.targetAmount;
                }
            }
            return totalCost;
        }
    }

    // =========================================================================
    // 3. SCRIPTABLE OBJECT: TÍNH CÁCH & NHU CẦU NPC (NPC PERSONALITY SYSTEM)
    // =========================================================================

    [CreateAssetMenu(fileName = "NewNPCPersonality", menuName = "BrewAndDream/Data/NPC Personality")]
    public class NPCPersonalitySO : ScriptableObject
    {
        [Header("Định danh mẫu NPC")]
        public string archetypeId;
        public string archetypeName;
        public NPCArchetype archetypeType;
        [TextArea(2, 4)] public string personalityDescription;

        [Header("1. Kiên nhẫn (Patience)")]
        [Tooltip("Thời gian chờ lý tưởng (giây) - trả nước trong khoảng này đạt 100 điểm tốc độ")]
        public float idealWaitTimeSeconds = 30f;
        [Tooltip("Thời gian chờ tối đa (giây) trước khi bực tức bỏ đi")]
        public float maxWaitTimeSeconds = 120f;
        [Tooltip("Hệ số phạt tốc độ (k > 1: tụt điểm rất nhanh khi gần hết kiên nhẫn)")]
        [Range(0.5f, 3f)] public float waitDecayExponent = 1.5f;

        [Header("2. Khó tính / Dễ tính (Strictness)")]
        [Tooltip("Hệ số mũ khắt khe alpha (alpha = 0.7: dễ tính; alpha = 1.0: bình thường; alpha >= 1.6: bà cô khó tính / critic)")]
        [Range(0.5f, 2.5f)] public float strictnessAlpha = 1.0f;
        [Tooltip("Ngưỡng điểm hài lòng tối thiểu (0-100) để khách tính là 'Hài lòng' và muốn quay lại")]
        [Range(30f, 85f)] public float satisfactionThreshold = 60f;

        [Header("3. Gu vị & Món yêu thích (Taste Preference)")]
        public FlavorProfile preferredFlavor;
        [Tooltip("Mức độ chấp nhận lệch gu vị (càng nhỏ càng kén vị)")]
        [Range(0.1f, 1f)] public float tasteTolerance = 0.35f;
        public List<DrinkRecipeSO> favoriteDrinks = new List<DrinkRecipeSO>();

        [Header("4. Ngân sách & Nhạy cảm giá (Budget & Price Sensitivity)")]
        [Tooltip("Mức giá kỳ vọng của tệp khách này cho 1 ly nước (VNĐ)")]
        public float expectedBudgetVND = 35000f;
        [Tooltip("Độ nhạy cảm giá beta (cao = sinh viên tiết kiệm; thấp = critic / cặp đôi hẹn hò)")]
        [Range(0.2f, 3f)] public float priceSensitivityBeta = 1.2f;

        [Header("5. Trọng số ưu tiên 6 tiêu chí (Priority Weights)")]
        public EvaluationWeights weights = new EvaluationWeights
        {
            speedWeight = 0.20f,
            qualityWeight = 0.30f,
            attitudeWeight = 0.15f,
            hygieneWeight = 0.15f,
            ambianceWeight = 0.10f,
            priceWeight = 0.10f
        };

        [Header("6. Tính cách xã hội & Cốt truyện (Social & Narrative)")]
        public SocialBehaviorType socialType = SocialBehaviorType.OnlineReviewer;
        [Tooltip("Xác suất cơ bản để lại review trên App cuối ngày (0 - 1)")]
        [Range(0f, 1f)] public float baseReviewProbability = 0.35f;
        [Tooltip("Hệ số ảnh hưởng danh tiếng khu vực (Blogger/TikToker hoặc Critic có thể lên tới 3.0 - 5.0)")]
        [Range(0.5f, 5f)] public float reputationImpactMultiplier = 1.0f;
        [Tooltip("Số lần phục vụ đạt trên ngưỡng hài lòng để mở khóa câu chuyện riêng (Mini-quest)")]
        public int visitsToUnlockMiniQuest = 5;
        [Tooltip("Tên Node hội thoại trong Yarn Spinner / Ink khi mở khóa câu chuyện")]
        public string yarnDialogueNodeName;
    }

    // =========================================================================
    // 4. DỮ LIỆU ĐẦU VÀO KHI PHỤC VỤ 1 LY NƯỚC (RUNTIME INPUTS)
    // =========================================================================

    [Serializable]
    public struct CraftedStepLog
    {
        public int stepIndex;
        public float actualAmount;       // Lượng người chơi đã đong/pha thực tế
        public bool wasCorrectOrder;     // Có làm đúng thứ tự quy định không
    }

    [Serializable]
    public struct ServiceEvaluationContext
    {
        public DrinkRecipeSO orderedRecipe;
        public List<CraftedStepLog> playerStepLogs;
        public FlavorProfile actualFlavorProfile;

        public float actualWaitTimeSeconds;      // Thời gian khách phải đợi từ lúc gọi món
        public float sellingPriceVND;            // Giá bán hiện tại trên menu của quán
        public float playerAttitudeScore01;      // [0, 1] Thái độ (chào hỏi, giao đúng món, xin lỗi khi trễ)
        public float shopHygieneScore01;         // [0, 1] Độ sạch sẽ hiện tại của quầy & bàn ghế
        public float shopAmbianceMatch01;        // [0, 1] Độ khớp không gian (nhạc, chủ đề, wifi/ổ điện, độ ồn)
    }

    [Serializable]
    public struct EvaluationOutput
    {
        public float finalSatisfactionScore; // S_total thuộc [0, 100]
        public int starRating;               // 1 đến 5 sao
        public bool willReturn;              // Khách có quay lại không
        public bool willWriteReview;         // Khách có đăng bài lên App đánh giá không
        public float reputationDelta;        // Điểm danh tiếng cộng/trừ cho quán
        public string dominantPraiseCriterion;
        public string dominantComplaintCriterion;
    }

    // =========================================================================
    // 5. BỘ MÁY TOÁN HỌC CHẤM ĐIỂM ĐỘ HÀI LÒNG NPC (SCORING ENGINE)
    // =========================================================================

    public static class NPCSatisfactionCalculator
    {
        /// <summary>
        /// Tính điểm thực hiện kỹ thuật pha chế E_craft thuộc [0, 1] dựa trên sai số từng bước.
        /// </summary>
        public static float CalculateCraftingExecution01(DrinkRecipeSO recipe, List<CraftedStepLog> logs)
        {
            if (recipe == null || recipe.craftingSteps.Count == 0) return 1f;
            if (logs == null || logs.Count == 0) return 0f;

            float weightedScoreSum = 0f;
            float totalImportance = 0f;

            for (int i = 0; i < recipe.craftingSteps.Count; i++)
            {
                RecipeStep step = recipe.craftingSteps[i];
                totalImportance += step.stepImportance;

                // Tìm log tương ứng với bước i
                int foundIndex = logs.FindIndex(l => l.stepIndex == i);
                if (foundIndex < 0) continue; // Bỏ quên bước pha chế -> 0 điểm bước này

                CraftedStepLog log = logs[foundIndex];
                float absError = Mathf.Abs(log.actualAmount - step.targetAmount);

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
                    float range = Mathf.Max(0.0001f, step.maxErrorThreshold - step.allowedTolerance);
                    stepAccuracy = 1f - ((absError - step.allowedTolerance) / range);
                }

                // Phạt 30% điểm bước này nếu yêu cầu đúng thứ tự nhưng làm sai thứ tự
                if (step.requireStrictOrder && !log.wasCorrectOrder)
                {
                    stepAccuracy *= 0.7f;
                }

                weightedScoreSum += stepAccuracy * step.stepImportance;
            }

            return Mathf.Clamp01(weightedScoreSum / Mathf.Max(0.0001f, totalImportance));
        }

        /// <summary>
        /// Tính độ khớp gu hương vị M_taste thuộc [0, 1] bằng khoảng cách Euclid chuẩn hóa.
        /// </summary>
        public static float CalculateTasteMatch01(FlavorProfile actual, FlavorProfile preferred, float tolerance)
        {
            float dSweet = actual.sweetness - preferred.sweetness;
            float dBitter = actual.bitterness - preferred.bitterness;
            float dCream = actual.creaminess - preferred.creaminess;
            float dAroma = actual.aroma - preferred.aroma;
            float dNovelty = actual.novelty - preferred.novelty;

            // Khoảng cách Euclid trung bình trên 5 chiều hương vị (thuộc [0, 1])
            float distance = Mathf.Sqrt((dSweet * dSweet + dBitter * dBitter + dCream * dCream + dAroma * dAroma + dNovelty * dNovelty) / 5f);

            if (distance <= tolerance * 0.5f) return 1f;
            float normalizedDiff = (distance - tolerance * 0.5f) / Mathf.Max(0.05f, 1f - tolerance * 0.5f);
            return Mathf.Clamp01(1f - normalizedDiff);
        }

        /// <summary>
        /// Đánh giá tổng hợp 6 tiêu chí và trả về kết quả chấm điểm của NPC.
        /// </summary>
        public static EvaluationOutput EvaluateService(NPCPersonalitySO npc, ServiceEvaluationContext ctx)
        {
            // 1. Điểm Tốc độ (S_speed thuộc [0, 1])
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
                              Mathf.Max(1f, npc.maxWaitTimeSeconds - npc.idealWaitTimeSeconds);
                sSpeed = Mathf.Clamp01(1f - Mathf.Pow(ratio, npc.waitDecayExponent));
            }

            // 2. Điểm Chất lượng đồ uống (S_quality thuộc [0, 1] = 70% Kỹ thuật pha + 30% Hợp gu vị)
            float eCraft = CalculateCraftingExecution01(ctx.orderedRecipe, ctx.playerStepLogs);
            float mTaste = CalculateTasteMatch01(ctx.actualFlavorProfile, npc.preferredFlavor, npc.tasteTolerance);
            float sQuality = Mathf.Clamp01(0.70f * eCraft + 0.30f * mTaste);

            // 3. Điểm Thái độ, Vệ sinh, Không gian (thuộc [0, 1])
            float sAttitude = Mathf.Clamp01(ctx.playerAttitudeScore01);
            float sHygiene = Mathf.Clamp01(ctx.shopHygieneScore01);
            float sAmbiance = Mathf.Clamp01(ctx.shopAmbianceMatch01);

            // 4. Điểm Giá bán (S_price thuộc [0, 1])
            float priceRatio = (ctx.sellingPriceVND - npc.expectedBudgetVND) / Mathf.Max(1000f, npc.expectedBudgetVND);
            float sPrice;
            if (priceRatio <= 0f)
            {
                // Giá rẻ hơn hoặc bằng kỳ vọng -> đạt tối đa (hoặc cộng nhẹ thiện cảm)
                sPrice = 1f;
            }
            else
            {
                // Giá đắt hơn kỳ vọng -> giảm theo hàm mũ độ nhạy cảm giá beta
                sPrice = Mathf.Clamp01(Mathf.Exp(-npc.priceSensitivityBeta * priceRatio));
            }

            // 5. Áp dụng Độ khó tính (Strictness alpha) lên từng tiêu chí và nhân trọng số
            EvaluationWeights w = npc.weights;
            float wSum = w.TotalWeight;

            float cSpeed = Mathf.Pow(sSpeed, npc.strictnessAlpha);
            float cQuality = Mathf.Pow(sQuality, npc.strictnessAlpha);
            float cAttitude = Mathf.Pow(sAttitude, npc.strictnessAlpha);
            float cHygiene = Mathf.Pow(sHygiene, npc.strictnessAlpha);
            float cAmbiance = Mathf.Pow(sAmbiance, npc.strictnessAlpha);
            float cPrice = Mathf.Pow(sPrice, npc.strictnessAlpha);

            float weightedRaw = (
                w.speedWeight * cSpeed +
                w.qualityWeight * cQuality +
                w.attitudeWeight * cAttitude +
                w.hygieneWeight * cHygiene +
                w.ambianceWeight * cAmbiance +
                w.priceWeight * cPrice
            ) / wSum;

            // Hệ số phạt nghiêm trọng (Critical Veto): Nếu chất lượng ly nước < 0.2 hoặc hết sạch kiên nhẫn
            float criticalMultiplier = (sQuality < 0.2f || sSpeed <= 0.01f) ? 0.45f : 1.0f;

            float finalScore100 = Mathf.Clamp(weightedRaw * criticalMultiplier * 100f, 0f, 100f);

            // 6. Quy đổi ra số sao (1 - 5 sao)
            int stars = Mathf.Clamp(Mathf.CeilToInt(finalScore100 / 20f), 1, 5);

            // 7. Quyết định quay lại, viết review và thay đổi danh tiếng
            bool willReturn = finalScore100 >= npc.satisfactionThreshold;

            // Khách cực kỳ hài lòng hoặc cực kỳ tức giận sẽ có xác suất viết review cao hơn bình thường
            float extremityBoost = Mathf.Abs(finalScore100 - 50f) / 50f; // [0, 1]
            float reviewProb = Mathf.Clamp01(npc.baseReviewProbability * (0.6f + 0.8f * extremityBoost));
            bool willReview = UnityEngine.Random.value <= reviewProb;

            float repDelta = ((finalScore100 - npc.satisfactionThreshold) / 10f) * npc.reputationImpactMultiplier;

            // Xác định tiêu chí khen/chê nổi bật nhất để ghép câu thoại hoặc gửi vào prompt LLM
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