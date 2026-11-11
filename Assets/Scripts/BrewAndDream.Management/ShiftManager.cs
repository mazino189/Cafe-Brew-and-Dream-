using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Engines;
using BrewAndDream.Core.Models;
using BrewAndDream.UI.FinancialReport;
using BrewAndDream.UI.Dialogues;

namespace BrewAndDream.Management
{
    /// <summary>
    /// Bộ quản lý nhịp độ 3 ca làm việc trong ngày (Sáng, Trưa cao điểm, Chiều-Tối).
    /// Điều phối doanh thu, chuyển ca, hiển thị báo cáo tài chính và chuyển giao ngày mới.
    /// </summary>
    public class ShiftManager : MonoBehaviour
    {
        [Header("Thông tin ngày & ca")]
        public int currentDay = 1;
        public ShiftType currentShift = ShiftType.Morning;
        public float shiftDurationSeconds = 180f; // Mỗi ca kéo dài 3 phút (mô phỏng)
        public float currentShiftTimer = 0f;
        public bool isShiftRunning = false;

        [Header("Sổ sách tài chính ca hiện tại")]
        public ShiftLedger currentLedger;
        public double storeTotalFunds = 50000000; // Vốn ban đầu (50 triệu VNĐ)

        [Header("UI Liên kết")]
        [SerializeField] private ShiftLedgerUI ledgerUI;
        [SerializeField] private EventDramaManager dramaManager;

        public event Action<ShiftType, int> OnShiftStarted;
        public event Action<ShiftLedger> OnShiftEnded;

        private void Start()
        {
            StartNewShift(ShiftType.Morning);
        }

        private void Update()
        {
            if (!isShiftRunning) return;

            currentShiftTimer += Time.deltaTime;
            if (currentShiftTimer >= shiftDurationSeconds)
            {
                EndCurrentShift();
            }
        }

        public void StartNewShift(ShiftType shift)
        {
            currentShift = shift;
            currentShiftTimer = 0f;
            isShiftRunning = true;

            currentLedger = new ShiftLedger
            {
                shiftName = $"Ngày {currentDay} - {GetShiftDisplayName(shift)}",
                openingCash = storeTotalFunds,
                cashRevenue = 0,
                rawMaterialExpenses = 0,
                overheadExpenses = 0,
                ordersCompleted = 0,
                ordersFailed = 0,
                totalSatisfactionSum = 0f
            };

            OnShiftStarted?.Invoke(currentShift, currentDay);
        }

        public void RecordOrderSale(float revenue, float cogsCost, float satisfactionScore)
        {
            if (currentLedger == null) return;

            currentLedger.cashRevenue += revenue;
            currentLedger.rawMaterialExpenses += cogsCost;
            currentLedger.ordersCompleted++;
            currentLedger.totalSatisfactionSum += satisfactionScore;
        }

        public void EndCurrentShift()
        {
            isShiftRunning = false;
            storeTotalFunds = currentLedger.closingCash;

            // Hiển thị giao diện báo cáo tài chính ca
            if (ledgerUI != null)
            {
                ledgerUI.DisplayLedger(currentLedger);
                ledgerUI.OnAcknowledgeReport += HandleNextShiftTransition;
            }

            OnShiftEnded?.Invoke(currentLedger);
        }

        private void HandleNextShiftTransition()
        {
            if (ledgerUI != null)
            {
                ledgerUI.OnAcknowledgeReport -= HandleNextShiftTransition;
            }

            // Chuyển ca kế tiếp
            switch (currentShift)
            {
                case ShiftType.Morning:
                    StartNewShift(ShiftType.NoonRush);
                    break;
                case ShiftType.NoonRush:
                    StartNewShift(ShiftType.AfternoonEvening);
                    break;
                case ShiftType.AfternoonEvening:
                    currentDay++;
                    StartNewShift(ShiftType.Morning);
                    break;
            }
        }

        private string GetShiftDisplayName(ShiftType shift)
        {
            switch (shift)
            {
                case ShiftType.Morning: return "Ca Sáng (06:30 - 11:30)";
                case ShiftType.NoonRush: return "Ca Trưa Cao Điểm (11:30 - 14:00)";
                case ShiftType.AfternoonEvening: return "Ca Chiều & Tối (14:00 - 22:00)";
                default: return "Ca làm việc";
            }
        }
    }
}

