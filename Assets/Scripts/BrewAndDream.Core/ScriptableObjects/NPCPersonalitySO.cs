using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;

namespace BrewAndDream.UnityBridge
{
    [CreateAssetMenu(fileName = "NewNPCPersonalitySO", menuName = "BrewAndDream/ScriptableObjects/NPC Personality")]
    public class NPCPersonalitySO : ScriptableObject
    {
        public string archetypeId;
        public string archetypeName;
        public NPCArchetype archetypeType;
        [TextArea(2, 4)] public string personalityDescription;

        public float idealWaitTimeSeconds = 30f;
        public float maxWaitTimeSeconds = 120f;
        public float waitDecayExponent = 1.5f;

        public float strictnessAlpha = 1.0f;
        public float satisfactionThreshold = 60f;

        public FlavorProfile preferredFlavor;
        public float tasteTolerance = 0.35f;

        public float expectedBudgetVND = 35000f;
        public float priceSensitivityBeta = 1.2f;

        public EvaluationWeights weights = new EvaluationWeights
        {
            speedWeight = 0.20f,
            qualityWeight = 0.30f,
            attitudeWeight = 0.15f,
            hygieneWeight = 0.15f,
            ambianceWeight = 0.10f,
            priceWeight = 0.10f
        };

        public SocialBehaviorType socialType = SocialBehaviorType.OnlineReviewer;
        public float baseReviewProbability = 0.35f;
        public float reputationImpactMultiplier = 1.0f;

        public NPCPersonalityModel ToModel()
        {
            return new NPCPersonalityModel
            {
                archetypeId = archetypeId,
                archetypeName = archetypeName,
                archetypeType = archetypeType,
                personalityDescription = personalityDescription,
                idealWaitTimeSeconds = idealWaitTimeSeconds,
                maxWaitTimeSeconds = maxWaitTimeSeconds,
                waitDecayExponent = waitDecayExponent,
                strictnessAlpha = strictnessAlpha,
                satisfactionThreshold = satisfactionThreshold,
                preferredFlavor = preferredFlavor,
                tasteTolerance = tasteTolerance,
                expectedBudgetVND = expectedBudgetVND,
                priceSensitivityBeta = priceSensitivityBeta,
                weights = weights,
                socialType = socialType,
                baseReviewProbability = baseReviewProbability,
                reputationImpactMultiplier = reputationImpactMultiplier
            };
        }
    }
}

