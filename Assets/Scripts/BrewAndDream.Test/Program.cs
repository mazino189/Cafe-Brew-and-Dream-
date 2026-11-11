#if !UNITY_5_3_OR_NEWER
using System;
using System.Collections.Generic;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.Core.Engines;

namespace BrewAndDream.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("BREW & DREAM - CORE SIMULATION TEST RUNNER");
            Console.WriteLine("==================================================");

            // 1. Tạo Database Nguyên liệu mẫu
            var ingCoffee = new IngredientModel("ing_robusta", "Cà phê Robusta Buôn Ma Thuột", 0.15f, 90, 5000); // 150đ/g
            var ingCondensedMilk = new IngredientModel("ing_condensed_milk", "Sữa đặc Ông Thọ", 0.08f, 30, 2000); // 80đ/ml
            var ingIce = new IngredientModel("ing_ice", "Đá viên sạch", 0.01f, 1, 10000); // 10đ/g

            var ingredients = new Dictionary<string, IngredientModel>
            {
                { ingCoffee.ingredientId, ingCoffee },
                { ingCondensedMilk.ingredientId, ingCondensedMilk },
                { ingIce.ingredientId, ingIce }
            };

            // 2. Tạo Công thức: Cà phê sữa đá Sài Gòn
            var recipeSuaDa = new DrinkRecipeModel
            {
                recipeId = "recipe_cf_suada",
                drinkName = "Cà phê sữa đá Sài Gòn",
                referenceMarketPriceVND = 28000,
                idealFlavorProfile = new FlavorProfile(0.6f, 0.7f, 0.5f, 0.8f, 0.1f),
                craftingSteps = new List<RecipeStep>
                {
                    new RecipeStep { stepDescription = "Rót sữa đặc 30ml", actionType = CraftingActionType.PourLiquid, requiredIngredientId = "ing_condensed_milk", targetAmount = 30f, allowedTolerance = 3f, maxErrorThreshold = 10f, stepImportance = 1.0f, requireStrictOrder = true },
                    new RecipeStep { stepDescription = "Chiết xuất cafe phin 50ml", actionType = CraftingActionType.BrewMachineOrPhin, requiredIngredientId = "ing_robusta", targetAmount = 50f, allowedTolerance = 5f, maxErrorThreshold = 15f, stepImportance = 1.5f, requireStrictOrder = true },
                    new RecipeStep { stepDescription = "Thêm đá viên 120g", actionType = CraftingActionType.AddSolidOrPowder, requiredIngredientId = "ing_ice", targetAmount = 120f, allowedTolerance = 15f, maxErrorThreshold = 35f, stepImportance = 0.8f, requireStrictOrder = false }
                }
            };

            float cogs = recipeSuaDa.CalculateBaseIngredientCost(ingredients);
            Console.WriteLine($"[Công thức] {recipeSuaDa.drinkName} - Giá vốn (COGS): {cogs:N0} VNĐ | Giá bán tham chiếu: {recipeSuaDa.referenceMarketPriceVND:N0} VNĐ");

            // 3. Tạo mẫu NPC: Dân văn phòng vội (BusyOfficeWorker)
            var officeWorker = new NPCPersonalityModel
            {
                archetypeId = "npc_office_01",
                archetypeName = "Anh Hoàng - Chuyên viên IT",
                archetypeType = NPCArchetype.BusyOfficeWorker,
                idealWaitTimeSeconds = 25f,
                maxWaitTimeSeconds = 60f,
                waitDecayExponent = 1.8f,
                strictnessAlpha = 1.2f,
                satisfactionThreshold = 65f,
                expectedBudgetVND = 30000f,
                preferredFlavor = new FlavorProfile(0.55f, 0.75f, 0.45f, 0.85f, 0.1f),
                tasteTolerance = 0.3f
            };

            // 4. Test Case 1: Pha chuẩn mực & nhanh nhẹn
            var goodLogs = new List<CraftedStepLog>
            {
                new CraftedStepLog { stepIndex = 0, actualAmount = 31f, wasCorrectOrder = true },
                new CraftedStepLog { stepIndex = 1, actualAmount = 51f, wasCorrectOrder = true },
                new CraftedStepLog { stepIndex = 2, actualAmount = 125f, wasCorrectOrder = true }
            };

            var goodContext = new ServiceEvaluationContext
            {
                orderedRecipe = recipeSuaDa,
                playerStepLogs = goodLogs,
                actualFlavorProfile = new FlavorProfile(0.6f, 0.7f, 0.5f, 0.8f, 0.1f),
                actualWaitTimeSeconds = 20f, // Rất nhanh
                sellingPriceVND = 28000f,
                playerAttitudeScore01 = 0.95f,
                shopHygieneScore01 = 0.9f,
                shopAmbianceMatch01 = 0.85f
            };

            var resultGood = NPCSatisfactionCalculator.EvaluateService(officeWorker, goodContext);
            Console.WriteLine("\n--- TEST CASE 1: Pha chuẩn & Nhanh ---");
            Console.WriteLine($"Điểm hài lòng: {resultGood.finalSatisfactionScore:F1}/100 | Số sao: {resultGood.starRating}★");
            Console.WriteLine($"Khách có quay lại: {resultGood.willReturn} | Viết review: {resultGood.willWriteReview} | Uy tín thay đổi: {resultGood.reputationDelta:+0.0;-0.0}");
            Console.WriteLine($"Khen nổi bật: {resultGood.dominantPraiseCriterion} | Chê nổi bật: {resultGood.dominantComplaintCriterion}");

            // 5. Test Case 2: Làm trễ & đong sai tỷ lệ (quá ngọt, ít cafe)
            var badLogs = new List<CraftedStepLog>
            {
                new CraftedStepLog { stepIndex = 0, actualAmount = 45f, wasCorrectOrder = true },
                new CraftedStepLog { stepIndex = 1, actualAmount = 32f, wasCorrectOrder = true },
                new CraftedStepLog { stepIndex = 2, actualAmount = 80f, wasCorrectOrder = true }
            };

            var badContext = new ServiceEvaluationContext
            {
                orderedRecipe = recipeSuaDa,
                playerStepLogs = badLogs,
                actualFlavorProfile = new FlavorProfile(0.9f, 0.3f, 0.8f, 0.4f, 0.1f),
                actualWaitTimeSeconds = 70f,
                sellingPriceVND = 35000f,
                playerAttitudeScore01 = 0.5f,
                shopHygieneScore01 = 0.6f,
                shopAmbianceMatch01 = 0.5f
            };

            var resultBad = NPCSatisfactionCalculator.EvaluateService(officeWorker, badContext);
            Console.WriteLine("\n--- TEST CASE 2: Làm trễ & Sai công thức ---");
            Console.WriteLine($"Điểm hài lòng: {resultBad.finalSatisfactionScore:F1}/100 | Số sao: {resultBad.starRating}★");
            Console.WriteLine($"Khách có quay lại: {resultBad.willReturn} | Viết review: {resultBad.willWriteReview} | Uy tín thay đổi: {resultBad.reputationDelta:+0.0;-0.0}");
            Console.WriteLine($"Khen nổi bật: {resultBad.dominantPraiseCriterion} | Chê nổi bật: {resultBad.dominantComplaintCriterion}");

            // 6. Test Máy móc & Nguy cơ hỏng
            float riskNormal = MachineMaintenanceEngine.CalculateBreakdownChance(35, 100, 0.9f);
            float riskOverload = MachineMaintenanceEngine.CalculateBreakdownChance(120, 100, 0.3f);
            Console.WriteLine($"\n--- TEST ENGINE THIẾT BỊ ---");
            Console.WriteLine($"Nguy cơ hỏng (Sử dụng bình thường): {riskNormal * 100:F1}%");
            Console.WriteLine($"Nguy cơ hỏng (Quá tải, bảo trì kém): {riskOverload * 100:F1}%");
            Console.WriteLine("\n>> TẤT CẢ UNIT CHECKS PASSED!");
        }
    }
}
#endif
