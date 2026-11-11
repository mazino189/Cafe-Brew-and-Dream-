using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;

namespace BrewAndDream.UnityBridge
{
    [CreateAssetMenu(fileName = "NewDrinkRecipeSO", menuName = "BrewAndDream/ScriptableObjects/Drink Recipe")]
    public class DrinkRecipeSO : ScriptableObject
    {
        public string recipeId;
        public string drinkName;
        [TextArea(2, 4)] public string description;
        public Sprite icon;
        public int referenceMarketPriceVND = 30000;
        public FlavorProfile idealFlavorProfile;
        public List<RecipeStep> craftingSteps = new List<RecipeStep>();

        public DrinkRecipeModel ToModel()
        {
            return new DrinkRecipeModel
            {
                recipeId = recipeId,
                drinkName = drinkName,
                description = description,
                referenceMarketPriceVND = referenceMarketPriceVND,
                idealFlavorProfile = idealFlavorProfile,
                craftingSteps = craftingSteps
            };
        }
    }
}

