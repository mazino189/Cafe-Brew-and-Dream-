using System;
using UnityEngine;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Thùng chứa đá viên hoặc trạm bột/đường/topping khô.
    /// Cho phép dùng muỗng/xẻng múc từng định lượng gram hoặc xúc đầy muỗng vào ly.
    /// </summary>
    public class IceAndSolidDispenser : MonoBehaviour, IInteractable
    {
        [Header("Thông tin nguyên liệu")]
        public string ingredientId = "ing_ice";
        public string ingredientName = "Đá viên sạch";
        public float amountPerScoop = 40f; // Mỗi lần múc = 40g đá

        [Header("Trạng thái")]
        [SerializeField] private Transform cupSnapPoint;
        private CupItem _activeCup;

        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            if (_activeCup == null)
                return $"Đặt ly vào trạm xúc [{ingredientName}]";
            return $"Nhấn để xúc 1 muỗng [{ingredientName}] (+{amountPerScoop}g)";
        }

        public void OnInteract(PlayerInteractionController player)
        {
            // Đặt ly vào trạm nếu đang cầm
            if (_activeCup == null && player.HeldObject != null)
            {
                var cup = player.HeldObject.GetComponent<CupItem>();
                if (cup != null)
                {
                    player.ReleaseHeldObject();
                    _activeCup = cup;
                    cup.transform.position = cupSnapPoint ? cupSnapPoint.position : transform.position;
                    cup.transform.rotation = cupSnapPoint ? cupSnapPoint.rotation : Quaternion.identity;
                    return;
                }
            }

            // Xúc thêm một lượng vào ly
            if (_activeCup != null)
            {
                // Áp dụng bước xúc đá (stepIndex = 2 theo mẫu công thức)
                _activeCup.ApplyStep(2, amountPerScoop, false);
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player)
        {
            // Nhặt ly ra khỏi trạm đá
            if (_activeCup != null && player.HeldObject == null)
            {
                player.HoldObject(_activeCup.gameObject);
                _activeCup = null;
            }
        }
    }
}

