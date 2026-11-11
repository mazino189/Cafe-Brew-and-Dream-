using System;
using System.Collections.Generic;
using BrewAndDream.Core.Enums;

namespace BrewAndDream.Core.Models
{
    [Serializable]
    public class IngredientModel
    {
        public string ingredientId;
        public string displayName;
        public float unitCostVND;
        public int shelfLifeDays;
        public float remainingUnits; // Đơn vị: ml hoặc gram trong kho

        public IngredientModel(string id, string name, float cost, int shelfLife, float stock = 0)
        {
            ingredientId = id;
            displayName = name;
            unitCostVND = cost;
            shelfLifeDays = shelfLife;
            remainingUnits = stock;
        }
    }

    [Serializable]
    public struct RecipeStep
    {
        public string stepDescription;
        public CraftingActionType actionType;
        public string requiredIngredientId;
        public float targetAmount;       // ml, gram, hoặc giây
        public float allowedTolerance;   // Sai số cho phép không bị trừ điểm
        public float maxErrorThreshold;  // Sai số tối đa trước khi hỏng hoàn toàn (0 điểm)
        public float stepImportance;     // Trọng số [0.1, 5]
        public bool requireStrictOrder;  // Phải thực hiện đúng thứ tự
    }

    [Serializable]
    public class DrinkRecipeModel
    {
        public string recipeId;
        public string drinkName;
        public string description;
        public int referenceMarketPriceVND;
        public bool unlockedFromStart;
        public FlavorProfile idealFlavorProfile;
        public List<RecipeStep> craftingSteps = new List<RecipeStep>();

        public float CalculateBaseIngredientCost(Dictionary<string, IngredientModel> ingredientDatabase)
        {
            float totalCost = 0f;
            foreach (var step in craftingSteps)
            {
                if (!string.IsNullOrEmpty(step.requiredIngredientId) &&
                    ingredientDatabase.TryGetValue(step.requiredIngredientId, out var ing))
                {
                    totalCost += ing.unitCostVND * step.targetAmount;
                }
            }
            return totalCost;
        }
    }
}

