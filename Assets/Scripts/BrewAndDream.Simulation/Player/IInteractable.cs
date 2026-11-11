using System;
using UnityEngine;

namespace BrewAndDream.Simulation.Player
{
    public interface IInteractable
    {
        string GetInteractionPrompt();
        void OnInteract(PlayerInteractionController player);
        void OnAlternateInteract(PlayerInteractionController player); // Click phải hoặc giữ chuột
        bool CanInteract { get; }
    }
}

