using System;
using UnityEngine;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Chai nguyên liệu lỏng / Hộp nguyên liệu (Sữa đặc, siro, sữa tươi).
    /// Hỗ trợ thao tác giữ chuột để rót chính xác (Hold to Pour).
    /// </summary>
    public class LiquidDispenserStation : MonoBehaviour, IInteractable
    {
        [Header("Thông số chai / trạm rót")]
        public string ingredientId = "ing_condensed_milk";
        public string ingredientName = "Sữa đặc Ông Thọ";
        public float pourRatePerSecond = 20f; // Tốc độ rót: 20ml / giây

        [Header("Liên kết ly đang đặt dưới vòi rót")]
        [SerializeField] private Transform cupSnapPoint;
        private CupItem _activeCup;
        private bool _isPouring = false;
        private float _pouredAmountThisSession = 0f;

        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            if (_activeCup == null)
                return $"Đặt ly vào trạm rót [{ingredientName}]";
            return $"Nhấn giữ chuột để rót [{ingredientName}] (Đã rót: {_pouredAmountThisSession:F1} ml)";
        }

        public void OnInteract(PlayerInteractionController player)
        {
            // Nếu người chơi đang cầm ly, đặt ly vào trạm
            if (_activeCup == null && player.HeldObject != null)
            {
                var cup = player.HeldObject.GetComponent<CupItem>();
                if (cup != null)
                {
                    player.ReleaseHeldObject();
                    _activeCup = cup;
                    cup.transform.position = cupSnapPoint ? cupSnapPoint.position : transform.position;
                    cup.transform.rotation = cupSnapPoint ? cupSnapPoint.rotation : Quaternion.identity;
                    _pouredAmountThisSession = 0f;
                    return;
                }
            }

            // Nếu đã có ly, bật cờ rót
            if (_activeCup != null)
            {
                _isPouring = !_isPouring;
                if (!_isPouring)
                {
                    // Kết thúc rót: ghi log vào ly
                    _activeCup.ApplyStep(0, _pouredAmountThisSession, true);
                }
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player)
        {
            // Nhặt ly ra khỏi trạm rót
            if (_activeCup != null && player.HeldObject == null)
            {
                player.HoldObject(_activeCup.gameObject);
                _activeCup = null;
                _isPouring = false;
            }
        }

        private void Update()
        {
            if (_isPouring && _activeCup != null)
            {
                float delta = pourRatePerSecond * Time.deltaTime;
                _pouredAmountThisSession += delta;
            }
        }
    }
}

