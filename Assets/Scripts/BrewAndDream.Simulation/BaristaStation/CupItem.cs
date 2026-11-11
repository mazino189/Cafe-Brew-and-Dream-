using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Đại diện cho một ly nước đang được pha chế trên quầy bar.
    /// Ghi nhận từng thao tác người chơi thực hiện (CraftedStepLog) để phục vụ chấm điểm.
    /// </summary>
    public class CupItem : MonoBehaviour, IInteractable
    {
        [Header("Thông tin cốc")]
        public string cupName = "Ly thủy tinh";
        public DrinkRecipeModel targetRecipe;

        [Header("Nhật ký thao tác pha chế")]
        public List<CraftedStepLog> executedLogs = new List<CraftedStepLog>();
        public FlavorProfile currentFlavorProfile;

        private int _stepSequenceCounter = 0;
        public bool IsFinished { get; private set; }

        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            if (IsFinished)
                return $"Cầm [{cupName}] đã pha xong mang giao khách";
            return $"Tương tác với [{cupName}] (Đã thực hiện {_stepSequenceCounter} bước)";
        }

        public void OnInteract(PlayerInteractionController player)
        {
            if (player.HeldObject == null)
            {
                // Cầm ly lên tay
                player.HoldObject(this.gameObject);
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player)
        {
            if (player.HeldObject == this.gameObject)
            {
                // Đặt ly xuống quầy
                player.ReleaseHeldObject();
            }
        }

        /// <summary>
        /// Ghi nhận một bước thao tác (rót sữa, chiết cafe, thêm đá...)
        /// </summary>
        public void ApplyStep(int stepIndex, float actualAmount, bool correctOrder)
        {
            executedLogs.Add(new CraftedStepLog
            {
                stepIndex = stepIndex,
                actualAmount = actualAmount,
                wasCorrectOrder = correctOrder
            });
            _stepSequenceCounter++;
        }

        public void MarkAsServed()
        {
            IsFinished = true;
        }
    }
}

