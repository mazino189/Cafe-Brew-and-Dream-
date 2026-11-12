#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using BrewAndDream.Core;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.UnityBridge;
using BrewAndDream.EditorTools;

namespace BrewAndDream.ProjectRecovery
{
    public static class BrewAndDreamProjectRecoveryTool
    {
        private const string DataRoot = "Assets/_Project/Data";
        private const string RecipesPath = DataRoot + "/Recipes";
        private const string IngredientsPath = DataRoot + "/Ingredients";
        private const string NPCsPath = DataRoot + "/NPCPersonalities";
        private const string TemplateLibPath = DataRoot + "/ReviewTemplateLibrarySO.asset";

        [MenuItem("BrewAndDream/Tools/1. Chẩn Đoán Sức Khỏe Project", priority = 1)]
        public static void DiagnoseProjectHealth()
        {
            Debug.Log("[BrewAndDream Recovery] Bắt đầu chẩn đoán sức khỏe dự án...");

            bool hasDataRoot = Directory.Exists(DataRoot);
            Debug.Log($"[BrewAndDream Recovery] Thư mục Data: {(hasDataRoot ? "HỢP LỆ" : "CHƯA CÓ")}");

            string recipeFile = RecipesPath + "/Recipe_BacXiu.asset";
            bool hasRecipe = File.Exists(recipeFile);
            Debug.Log($"[BrewAndDream Recovery] Công thức Bạc Xỉu: {(hasRecipe ? "HỢP LỆ" : "THIẾU")}");

            bool hasTemplateLib = File.Exists(TemplateLibPath);
            Debug.Log($"[BrewAndDream Recovery] Thư viện mẫu Review: {(hasTemplateLib ? "HỢP LỆ" : "THIẾU")}");

            Debug.Log("[BrewAndDream Recovery] Hoàn tất chẩn đoán.");
        }

        [MenuItem("BrewAndDream/Tools/2. Khôi Phục Toàn Bộ Dữ Liệu (.asset) & Cấu Hình Build", priority = 2)]
        public static void RecoverAllDataAndBuildSettings()
        {
            Debug.Log("[BrewAndDream Recovery] Bắt đầu khởi tạo và khôi phục toàn bộ dữ liệu .asset...");

            EnsureFolder(DataRoot);
            EnsureFolder(RecipesPath);
            EnsureFolder(IngredientsPath);
            EnsureFolder(NPCsPath);

            // 1. Sinh 8 Presets NPC chuẩn Blueprint v2
            NPCPresetGenerator.GenerateAllNPCPresets();

            // 2. Tạo công thức 6 bước Bạc Xỉu chuẩn
            CreateBacXiuRecipe();

            // 3. Tạo Review Template Library SO
            CreateReviewTemplateLibrary();

            // 4. Tự động dựng Scene cơ sở nếu chưa có
            string scenePath = "Assets/_Project/Scenes/MainCafeScene.unity";
            if (!File.Exists(scenePath))
            {
                AutoSceneBuilder.BuildDefaultScene();
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[BrewAndDream Recovery] >> ĐÃ KHÔI PHỤC TOÀN BỘ DỮ LIỆU & SCENE THÀNH CÔNG!");
        }

        private static void CreateBacXiuRecipe()
        {
            string assetPath = RecipesPath + "/Recipe_BacXiu.asset";
            var recipe = AssetDatabase.LoadAssetAtPath<DrinkRecipeSO>(assetPath);
            if (recipe == null)
            {
                recipe = ScriptableObject.CreateInstance<DrinkRecipeSO>();
                AssetDatabase.CreateAsset(recipe, assetPath);
            }

            recipe.recipeId = "Recipe_BacXiu";
            recipe.drinkName = "Bạc Xỉu 3 Tầng";
            recipe.description = "Bạc xỉu Sài Gòn truyền thống: sữa đặc, đá viên, sữa tươi đánh bọt nhẹ, và một lớp cà phê phin đậm đà trên cùng.";
            recipe.referenceMarketPriceVND = 35000;
            recipe.idealFlavorProfile = new FlavorProfile(0.75f, 0.45f, 0.70f, 0.60f, 0.20f);

            recipe.craftingSteps = new List<RecipeStep>
            {
                new RecipeStep
                {
                    stepDescription = "1. Lấy ly sạch đặt vào quầy",
                    actionType = CraftingActionType.PlaceCup,
                    requiredIngredientId = "Cup_Glass_Medium",
                    targetAmount = 1f,
                    allowedTolerance = 0f,
                    maxErrorThreshold = 1f,
                    stepImportance = 1.0f,
                    requireStrictOrder = true
                },
                new RecipeStep
                {
                    stepDescription = "2. Rót sữa đặc vào đáy ly",
                    actionType = CraftingActionType.DispenseLiquid,
                    requiredIngredientId = "Ing_CondensedMilk",
                    targetAmount = 30f, // 30ml
                    allowedTolerance = 5f,
                    maxErrorThreshold = 15f,
                    stepImportance = 2.0f,
                    requireStrictOrder = true
                },
                new RecipeStep
                {
                    stepDescription = "3. Thêm đá viên lưng ly",
                    actionType = CraftingActionType.AddSolidIce,
                    requiredIngredientId = "Ing_IceCubes",
                    targetAmount = 120f, // 120g
                    allowedTolerance = 20f,
                    maxErrorThreshold = 50f,
                    stepImportance = 1.2f,
                    requireStrictOrder = true
                },
                new RecipeStep
                {
                    stepDescription = "4. Rót sữa tươi thanh trùng",
                    actionType = CraftingActionType.DispenseLiquid,
                    requiredIngredientId = "Ing_FreshMilk",
                    targetAmount = 80f, // 80ml
                    allowedTolerance = 10f,
                    maxErrorThreshold = 25f,
                    stepImportance = 2.0f,
                    requireStrictOrder = true
                },
                new RecipeStep
                {
                    stepDescription = "5. Rót cà phê phin đậm đặc lên tầng mặt",
                    actionType = CraftingActionType.DispenseLiquid,
                    requiredIngredientId = "Ing_PhinCoffee",
                    targetAmount = 25f, // 25ml
                    allowedTolerance = 5f,
                    maxErrorThreshold = 15f,
                    stepImportance = 3.0f,
                    requireStrictOrder = true
                },
                new RecipeStep
                {
                    stepDescription = "6. Khuấy nhẹ hoặc đậy nắp hoàn thiện",
                    actionType = CraftingActionType.Stir,
                    requiredIngredientId = "Tool_Stirrer",
                    targetAmount = 2f, // 2 giây
                    allowedTolerance = 1f,
                    maxErrorThreshold = 4f,
                    stepImportance = 1.0f,
                    requireStrictOrder = false
                }
            };

            EditorUtility.SetDirty(recipe);
        }

        private static void CreateReviewTemplateLibrary()
        {
            var lib = AssetDatabase.LoadAssetAtPath<ReviewTemplateLibrarySO>(TemplateLibPath);
            if (lib == null)
            {
                lib = ScriptableObject.CreateInstance<ReviewTemplateLibrarySO>();
                AssetDatabase.CreateAsset(lib, TemplateLibPath);
            }
            EditorUtility.SetDirty(lib);
        }

        private static void EnsureFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        [InitializeOnLoadMethod]
        private static void AutoRecoverOnLoad()
        {
            EditorApplication.delayCall += () =>
            {
                string recipePath = RecipesPath + "/Recipe_BacXiu.asset";
                if (!File.Exists(recipePath))
                {
                    RecoverAllDataAndBuildSettings();
                }
            };
        }
    }
}
#endif
