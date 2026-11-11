#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.UnityBridge;

namespace BrewAndDream.EditorTools
{
    /// <summary>
    /// Script Editor tự động sinh đầy đủ 8 file ScriptableObject NPCPersonalitySO
    /// bám sát 100% bảng thông số cân bằng toán học Mục 5 của Dev Blueprint v2.
    /// Menu: Tools -> Brew & Dream -> Generate 8 NPC Personalities (.asset)
    /// </summary>
    public static class NPCPresetGenerator
    {
        [MenuItem("Tools/Brew & Dream/Generate 8 NPC Personalities (.asset)")]
        public static void GenerateAllNPCPresets()
        {
            string folderPath = "Assets/_Project/ScriptableObjects/NPCs";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 1. Sinh viên cần Wifi (StudentNeedWifi)
            CreatePreset(folderPath, "NPC_StudentNeedWifi", new NPCPersonalitySO
            {
                archetypeId = "StudentNeedWifi",
                archetypeName = "Sinh viên cần Wifi & Ổ điện",
                archetypeType = NPCArchetype.StudentNeedWifi,
                personalityDescription = "Học sinh/sinh viên ngồi học bài, ngân sách eo hẹp, nhạy cảm giá cao, kiên nhẫn vừa phải.",
                idealWaitTimeSeconds = 35f,
                maxWaitTimeSeconds = 120f,
                waitDecayExponent = 1.3f,
                strictnessAlpha = 0.8f,
                satisfactionThreshold = 55f,
                expectedBudgetVND = 25000f,
                priceSensitivityBeta = 1.8f,
                preferredFlavor = new FlavorProfile(0.7f, 0.3f, 0.6f, 0.4f, 0.2f),
                tasteTolerance = 0.45f,
                weights = new EvaluationWeights { speedWeight = 0.15f, qualityWeight = 0.20f, attitudeWeight = 0.15f, hygieneWeight = 0.15f, ambianceWeight = 0.20f, priceWeight = 0.15f },
                socialType = SocialBehaviorType.OnlineReviewer,
                baseReviewProbability = 0.40f,
                reputationImpactMultiplier = 1.0f
            });

            // 2. Dân văn phòng vội (BusyOfficeWorker)
            CreatePreset(folderPath, "NPC_BusyOfficeWorker", new NPCPersonalitySO
            {
                archetypeId = "BusyOfficeWorker",
                archetypeName = "Dân văn phòng vội vã",
                archetypeType = NPCArchetype.BusyOfficeWorker,
                personalityDescription = "Mua cà phê sáng mang đi, ưu tiên tốc độ số 1, sẵn sàng trả giá cao nếu phục vụ nhanh.",
                idealWaitTimeSeconds = 20f,
                maxWaitTimeSeconds = 60f,
                waitDecayExponent = 2.0f,
                strictnessAlpha = 1.3f,
                satisfactionThreshold = 65f,
                expectedBudgetVND = 40000f,
                priceSensitivityBeta = 0.6f,
                preferredFlavor = new FlavorProfile(0.4f, 0.8f, 0.3f, 0.8f, 0.1f),
                tasteTolerance = 0.30f,
                weights = new EvaluationWeights { speedWeight = 0.40f, qualityWeight = 0.25f, attitudeWeight = 0.15f, hygieneWeight = 0.10f, ambianceWeight = 0.05f, priceWeight = 0.05f },
                socialType = SocialBehaviorType.WordOfMouthSpreader,
                baseReviewProbability = 0.25f,
                reputationImpactMultiplier = 1.2f
            });

            // 3. Bà cô khó tính (StrictAuntie)
            CreatePreset(folderPath, "NPC_StrictAuntie", new NPCPersonalitySO
            {
                archetypeId = "StrictAuntie",
                archetypeName = "Bà cô hàng xóm khó tính",
                archetypeType = NPCArchetype.StrictAuntie,
                personalityDescription = "Rất soi vệ sinh và thái độ chào hỏi, ghét ngọt gắt, dễ bực nếu quán bừa bộn.",
                idealWaitTimeSeconds = 40f,
                maxWaitTimeSeconds = 100f,
                waitDecayExponent = 1.6f,
                strictnessAlpha = 1.8f,
                satisfactionThreshold = 70f,
                expectedBudgetVND = 25000f,
                priceSensitivityBeta = 1.5f,
                preferredFlavor = new FlavorProfile(0.3f, 0.8f, 0.4f, 0.9f, 0.0f),
                tasteTolerance = 0.20f,
                weights = new EvaluationWeights { speedWeight = 0.10f, qualityWeight = 0.25f, attitudeWeight = 0.25f, hygieneWeight = 0.25f, ambianceWeight = 0.05f, priceWeight = 0.10f },
                socialType = SocialBehaviorType.WordOfMouthSpreader,
                baseReviewProbability = 0.60f,
                reputationImpactMultiplier = 2.0f
            });

            // 4. Nhà phê bình cà phê (CoffeeCritic)
            CreatePreset(folderPath, "NPC_CoffeeCritic", new NPCPersonalitySO
            {
                archetypeId = "CoffeeCritic",
                archetypeName = "Chuyên gia / Critic Cà Phê",
                archetypeType = NPCArchetype.CoffeeCritic,
                personalityDescription = "Cực kỳ am hiểu kỹ thuật chiết xuất, chỉ quan tâm chất lượng ly nước và hương vị chuẩn mực.",
                idealWaitTimeSeconds = 45f,
                maxWaitTimeSeconds = 150f,
                waitDecayExponent = 1.0f,
                strictnessAlpha = 2.2f,
                satisfactionThreshold = 75f,
                expectedBudgetVND = 65000f,
                priceSensitivityBeta = 0.4f,
                preferredFlavor = new FlavorProfile(0.2f, 0.9f, 0.2f, 1.0f, 0.7f),
                tasteTolerance = 0.15f,
                weights = new EvaluationWeights { speedWeight = 0.05f, qualityWeight = 0.55f, attitudeWeight = 0.15f, hygieneWeight = 0.15f, ambianceWeight = 0.05f, priceWeight = 0.05f },
                socialType = SocialBehaviorType.OnlineReviewer,
                baseReviewProbability = 0.90f,
                reputationImpactMultiplier = 4.0f
            });

            // 5. Cặp đôi hẹn hò (DatingCouple)
            CreatePreset(folderPath, "NPC_DatingCouple", new NPCPersonalitySO
            {
                archetypeId = "DatingCouple",
                archetypeName = "Cặp đôi hẹn hò",
                archetypeType = NPCArchetype.DatingCouple,
                personalityDescription = "Cần không gian lãng mạn, nhạc vừa phải, không gian sạch sẽ riêng tư, không nhạy cảm giá.",
                idealWaitTimeSeconds = 40f,
                maxWaitTimeSeconds = 140f,
                waitDecayExponent = 1.2f,
                strictnessAlpha = 0.9f,
                satisfactionThreshold = 60f,
                expectedBudgetVND = 50000f,
                priceSensitivityBeta = 0.5f,
                preferredFlavor = new FlavorProfile(0.6f, 0.4f, 0.7f, 0.7f, 0.4f),
                tasteTolerance = 0.40f,
                weights = new EvaluationWeights { speedWeight = 0.10f, qualityWeight = 0.20f, attitudeWeight = 0.15f, hygieneWeight = 0.20f, ambianceWeight = 0.30f, priceWeight = 0.05f },
                socialType = SocialBehaviorType.OnlineReviewer,
                baseReviewProbability = 0.35f,
                reputationImpactMultiplier = 1.2f
            });

            // 6. Blogger / TikToker Reviewer
            CreatePreset(folderPath, "NPC_BloggerTikToker", new NPCPersonalitySO
            {
                archetypeId = "BloggerTikToker",
                archetypeName = "Food Blogger / TikToker",
                archetypeType = NPCArchetype.BloggerTikToker,
                personalityDescription = "Thích món nước phân tầng đẹp mắt, không gian lên hình xinh, review có sức lan tỏa cực lớn.",
                idealWaitTimeSeconds = 35f,
                maxWaitTimeSeconds = 120f,
                waitDecayExponent = 1.4f,
                strictnessAlpha = 1.4f,
                satisfactionThreshold = 68f,
                expectedBudgetVND = 55000f,
                priceSensitivityBeta = 0.4f,
                preferredFlavor = new FlavorProfile(0.7f, 0.4f, 0.6f, 0.8f, 0.9f),
                tasteTolerance = 0.35f,
                weights = new EvaluationWeights { speedWeight = 0.10f, qualityWeight = 0.30f, attitudeWeight = 0.20f, hygieneWeight = 0.15f, ambianceWeight = 0.20f, priceWeight = 0.05f },
                socialType = SocialBehaviorType.OnlineReviewer,
                baseReviewProbability = 0.85f,
                reputationImpactMultiplier = 5.0f
            });

            // 7. Khách quen thân thiện (FriendlyRegular)
            CreatePreset(folderPath, "NPC_FriendlyRegular", new NPCPersonalitySO
            {
                archetypeId = "FriendlyRegular",
                archetypeName = "Khách quen dễ thương",
                archetypeType = NPCArchetype.FriendlyRegular,
                personalityDescription = "Ủng hộ quán từ đầu, dễ tính, thích trò chuyện chào hỏi với chủ quán.",
                idealWaitTimeSeconds = 40f,
                maxWaitTimeSeconds = 160f,
                waitDecayExponent = 1.0f,
                strictnessAlpha = 0.7f,
                satisfactionThreshold = 50f,
                expectedBudgetVND = 35000f,
                priceSensitivityBeta = 0.8f,
                preferredFlavor = new FlavorProfile(0.6f, 0.6f, 0.5f, 0.7f, 0.1f),
                tasteTolerance = 0.50f,
                weights = new EvaluationWeights { speedWeight = 0.15f, qualityWeight = 0.20f, attitudeWeight = 0.35f, hygieneWeight = 0.10f, ambianceWeight = 0.10f, priceWeight = 0.10f },
                socialType = SocialBehaviorType.WordOfMouthSpreader,
                baseReviewProbability = 0.45f,
                reputationImpactMultiplier = 1.5f
            });

            // 8. Khách kiếm chuyện (Troublemaker)
            CreatePreset(folderPath, "NPC_Troublemaker", new NPCPersonalitySO
            {
                archetypeId = "Troublemaker",
                archetypeName = "Khách hàng khó chịu / Kiếm chuyện",
                archetypeType = NPCArchetype.Troublemaker,
                personalityDescription = "Rất thiếu kiên nhẫn, dễ cáu gắt, đòi hỏi vô lý, dễ kích hoạt chuỗi drama bóc phốt.",
                idealWaitTimeSeconds = 15f,
                maxWaitTimeSeconds = 45f,
                waitDecayExponent = 2.5f,
                strictnessAlpha = 2.0f,
                satisfactionThreshold = 80f,
                expectedBudgetVND = 20000f,
                priceSensitivityBeta = 2.5f,
                preferredFlavor = new FlavorProfile(0.5f, 0.5f, 0.5f, 0.5f, 0.0f),
                tasteTolerance = 0.10f,
                weights = new EvaluationWeights { speedWeight = 0.35f, qualityWeight = 0.15f, attitudeWeight = 0.25f, hygieneWeight = 0.10f, ambianceWeight = 0.05f, priceWeight = 0.10f },
                socialType = SocialBehaviorType.OnlineReviewer,
                baseReviewProbability = 0.95f,
                reputationImpactMultiplier = 3.5f
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[Brew & Dream] >> ĐÃ TỰ ĐỘNG SINH THÀNH CÔNG ĐẦY ĐỦ 8 FILE NPC PERSONALITY SO!");
        }

        private static void CreatePreset(string folder, string fileName, NPCPersonalitySO data)
        {
            string assetPath = $"{folder}/{fileName}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<NPCPersonalitySO>(assetPath);
            if (existing != null)
            {
                EditorUtility.CopySerialized(data, existing);
            }
            else
            {
                AssetDatabase.CreateAsset(data, assetPath);
            }
        }
    }
}
#endif

