using System;
using UnityEngine;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Bồn rửa ly và Thùng rác quầy bar.
    /// Giúp người chơi đổ bỏ ly pha lỗi (reset ly) hoặc rửa ly bẩn để tái sử dụng.
    /// </summary>
    public class TrashAndWashSink : MonoBehaviour, IInteractable
    {
        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            return "Đổ bỏ ly pha hỏng / Rửa ly bẩn";
        }

        public void OnInteract(PlayerInteractionController player)
        {
            if (player.HeldObject != null)
            {
                var cup = player.HeldObject.GetComponent<CupItem>();
                if (cup != null)
                {
                    // Hủy đối tượng ly bị lỗi trên tay
                    player.ReleaseHeldObject();
                    Destroy(cup.gameObject);
                }
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player) { }
    }
}

