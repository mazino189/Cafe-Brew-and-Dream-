using System;
using System.Collections.Generic;
using BrewAndDream.Core.Enums;

namespace BrewAndDream.Core.Models
{
    [Serializable]
    public class NPCPersonalityModel
    {
        public string archetypeId;
        public string archetypeName;
        public NPCArchetype archetypeType;
        public string personalityDescription;

        // 1. Kiên nhẫn
        public float idealWaitTimeSeconds = 30f;
        public float maxWaitTimeSeconds = 120f;
        public float waitDecayExponent = 1.5f;

        // 2. Khắt khe
        public float strictnessAlpha = 1.0f;
        public float satisfactionThreshold = 60f;

        // 3. Gu vị & Món yêu thích
        public FlavorProfile preferredFlavor;
        public float tasteTolerance = 0.35f;
        public List<string> favoriteRecipeIds = new List<string>();

        // 4. Ngân sách & Giá
        public float expectedBudgetVND = 35000f;
        public float priceSensitivityBeta = 1.2f;

        // 5. Trọng số 6 tiêu chí
        public EvaluationWeights weights = new EvaluationWeights
        {
            speedWeight = 0.20f,
            qualityWeight = 0.30f,
            attitudeWeight = 0.15f,
            hygieneWeight = 0.15f,
            ambianceWeight = 0.10f,
            priceWeight = 0.10f
        };

        // 6. Xã hội & Cốt truyện
        public SocialBehaviorType socialType = SocialBehaviorType.OnlineReviewer;
        public float baseReviewProbability = 0.35f;
        public float reputationImpactMultiplier = 1.0f;
        public int visitsToUnlockMiniQuest = 5;
        public string yarnDialogueNodeName;
    }

    [Serializable]
    public struct CraftedStepLog
    {
        public int stepIndex;
        public float actualAmount;
        public bool wasCorrectOrder;
    }

    [Serializable]
    public struct ServiceEvaluationContext
    {
        public DrinkRecipeModel orderedRecipe;
        public List<CraftedStepLog> playerStepLogs;
        public FlavorProfile actualFlavorProfile;

        public float actualWaitTimeSeconds;
        public float sellingPriceVND;
        public float playerAttitudeScore01;
        public float shopHygieneScore01;
        public float shopAmbianceMatch01;
    }

    [Serializable]
    public struct EvaluationOutput
    {
        public float finalSatisfactionScore; // S_total [0, 100]
        public int starRating;               // 1 đến 5 sao
        public bool willReturn;              // Khách có quay lại không
        public bool willWriteReview;         // Khách có đăng review không
        public float reputationDelta;        // Tăng giảm danh tiếng
        public string dominantPraiseCriterion;
        public string dominantComplaintCriterion;
    }
}

