using System;
using UnityEngine;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Thìa khuấy dài Barspoon hoặc Bình lắc Shaker.
    /// Cho phép khuấy đều các tầng chất lỏng để hòa quyện hương vị (Flavor Blending).
    /// </summary>
    public class StirringStation : MonoBehaviour, IInteractable
    {
        [Header("Cấu hình khuấy")]
        public float requiredStirDuration = 3.0f; // Khuấy tối thiểu 3 giây
        public float stirProgress = 0f;
        public bool isStirring = false;

        [Header("Vị trí đặt ly")]
        [SerializeField] private Transform cupSnapPoint;
        private CupItem _activeCup;

        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            if (_activeCup == null)
                return "Đặt ly vào đế khuấy";
            if (isStirring)
                return $"Đang khuấy đều ly nước... ({stirProgress:F1}/{requiredStirDuration:F1}s)";
            return "Nhấn để bắt đầu khuấy / dừng khuấy";
        }

        public void OnInteract(PlayerInteractionController player)
        {
            if (_activeCup == null && player.HeldObject != null)
            {
                var cup = player.HeldObject.GetComponent<CupItem>();
                if (cup != null)
                {
                    player.ReleaseHeldObject();
                    _activeCup = cup;
                    cup.transform.position = cupSnapPoint ? cupSnapPoint.position : transform.position;
                    cup.transform.rotation = cupSnapPoint ? cupSnapPoint.rotation : Quaternion.identity;
                    stirProgress = 0f;
                    return;
                }
            }

            if (_activeCup != null)
            {
                isStirring = !isStirring;
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player)
        {
            if (_activeCup != null && !isStirring && player.HeldObject == null)
            {
                player.HoldObject(_activeCup.gameObject);
                _activeCup = null;
            }
        }

        private void Update()
        {
            if (isStirring && _activeCup != null)
            {
                stirProgress += Time.deltaTime;
                if (stirProgress >= requiredStirDuration)
                {
                    isStirring = false;
                    // Ghi nhận hoàn thành công đoạn khuấy
                    _activeCup.ApplyStep(3, stirProgress, true);
                }
            }
        }
    }
}

