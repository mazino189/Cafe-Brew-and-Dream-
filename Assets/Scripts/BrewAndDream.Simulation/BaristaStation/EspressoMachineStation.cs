using System;
using UnityEngine;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Máy pha cà phê Espresso / Máy phin chiết xuất.
    /// Có chu kỳ hỏng hóc, nút bấm khởi động và thanh tiến độ chiết xuất.
    /// </summary>
    public class EspressoMachineStation : MonoBehaviour, IInteractable
    {
        [Header("Thông số máy")]
        public string machineName = "Máy pha Espresso 2 Group";
        public float extractionTimeSeconds = 25f; // Chuẩn chiết xuất 25s
        public float currentMaintenanceCondition = 0.95f; // [0, 1]
        public int usageCountToday = 0;

        [Header("Trạng thái")]
        [SerializeField] private Transform cupSnapPoint;
        private CupItem _activeCup;
        private bool _isExtracting = false;
        private float _extractionTimer = 0f;

        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            if (_activeCup == null)
                return $"Đặt ly vào họng chiết xuất [{machineName}]";
            if (_isExtracting)
                return $"Đang chiết xuất cafe... ({_extractionTimer:F1}/{extractionTimeSeconds:F1}s)";
            return $"Nhấn để Bắt đầu chiết xuất cafe vào ly";
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
                    return;
                }
            }

            if (_activeCup != null && !_isExtracting)
            {
                _isExtracting = true;
                _extractionTimer = 0f;
                usageCountToday++;
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player)
        {
            if (_activeCup != null && !_isExtracting && player.HeldObject == null)
            {
                player.HoldObject(_activeCup.gameObject);
                _activeCup = null;
            }
        }

        private void Update()
        {
            if (_isExtracting && _activeCup != null)
            {
                _extractionTimer += Time.deltaTime;
                if (_extractionTimer >= extractionTimeSeconds)
                {
                    _isExtracting = false;
                    // Ghi nhận bước chiết xuất: 50ml cafe
                    _activeCup.ApplyStep(1, 50f, true);
                }
            }
        }
    }
}

