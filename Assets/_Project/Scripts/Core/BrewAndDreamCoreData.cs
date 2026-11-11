// BrewAndDreamCoreData.cs
// Synchronized bridge for BrewAndDream Core Architecture.
// Canonical types reside in BrewAndDream.Core.Enums, BrewAndDream.Core.Models, and BrewAndDream.UnityBridge.

using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.UnityBridge;

namespace BrewAndDream.Core
{
    // Type aliases to preserve backwards compatibility without creating conflicting duplicate definitions.
    // Models: NPCPersonalityModel, FlavorProfile, EvaluationWeights, CraftedStepLog, ServiceEvaluationContext, EvaluationOutput
    // Enums: ShopTheme, NPCArchetype, SocialBehaviorType, CraftingActionType
    // ScriptableObjects: DrinkRecipeSO, NPCPersonalitySO
}
