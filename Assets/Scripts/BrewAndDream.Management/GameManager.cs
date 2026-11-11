using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.UI.POS;
using BrewAndDream.UI.Reviews;
using BrewAndDream.Persistence;

namespace BrewAndDream.Management
{
    /// <summary>
    /// Bộ điều phối tổng thể (Central Game Master Controller).
    /// Kết nối ShiftManager, CustomerTrafficSpawner, EventDramaManager, POS, App Review và Lưu trữ Persistence.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Các bộ điều khiển phân hệ (Subsystems)")]
        [SerializeField] private ShiftManager shiftManager;
        [SerializeField] private CustomerTrafficSpawner trafficSpawner;
        [SerializeField] private EventDramaManager dramaManager;
        [SerializeField] private POSOverlayUI posUI;
        [SerializeField] private CustomerReviewAppUI reviewAppUI;
        [SerializeField] private DataPersistenceManager persistenceManager;

        [Header("Chỉ số vận hành toàn quán")]
        public float shopReputationScore = 75.0f; // Danh tiếng ban đầu [0 - 100]
        public int totalCustomersServedLifetime = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            BindSubsystems();
            RestoreSavedData();
        }

        private void RestoreSavedData()
        {
            if (persistenceManager != null && persistenceManager.CurrentState != null)
            {
                shopReputationScore = persistenceManager.CurrentState.ShopReputationScore;
                totalCustomersServedLifetime = persistenceManager.CurrentState.TotalCustomersServedLifetime;
                if (shiftManager != null)
                {
                    shiftManager.currentDay = persistenceManager.CurrentState.CurrentDay;
                    shiftManager.storeTotalFunds = persistenceManager.CurrentState.StoreTotalFunds;
                }
            }
        }

        private void BindSubsystems()
        {
            if (shiftManager != null)
            {
                shiftManager.OnShiftStarted += HandleShiftStarted;
                shiftManager.OnShiftEnded += HandleShiftEnded;
            }

            if (posUI != null)
            {
                posUI.OnOrderCompleted += (items, total) =>
                {
                    float cogs = total * 0.35f; // Tạm tính COGS trung bình 35% giá bán
                    shiftManager?.RecordOrderSale(total, cogs, 85f);
                    totalCustomersServedLifetime++;
                };
            }

            if (dramaManager != null)
            {
                dramaManager.OnDramaResolved += (choice) =>
                {
                    shopReputationScore = Math.Clamp(shopReputationScore + choice.reputationImpact, 0f, 100f);
                    if (shiftManager != null && shiftManager.currentLedger != null && choice.costImpactVND != 0)
                    {
                        shiftManager.currentLedger.overheadExpenses += Math.Abs(choice.costImpactVND);
                    }
                };
            }
        }

        private void HandleShiftStarted(ShiftType shift, int day)
        {
            if (trafficSpawner != null)
            {
                trafficSpawner.SetActiveShift(shift);
                trafficSpawner.StartSpawning();
            }

            // Kích hoạt biến cố ngẫu nhiên giữa ca nếu có
            if (dramaManager != null)
            {
                dramaManager.TryTriggerShiftDrama(shift, null);
            }
        }

        private void HandleShiftEnded(BrewAndDream.Core.Engines.ShiftLedger ledger)
        {
            if (trafficSpawner != null)
            {
                trafficSpawner.StopSpawning();
            }

            // Lưu trữ sổ sách ca và trạng thái game bền vững
            if (persistenceManager != null && shiftManager != null)
            {
                persistenceManager.RecordShiftLedger(shiftManager.currentDay, ledger);
                persistenceManager.SaveGameState(
                    shiftManager.currentDay,
                    shiftManager.storeTotalFunds,
                    shopReputationScore,
                    totalCustomersServedLifetime
                );
            }
        }

        public void RegisterCustomerFeedback(EvaluationOutput eval, string customerName, bool isRegular)
        {
            shopReputationScore = Math.Clamp(shopReputationScore + eval.reputationDelta, 0f, 100f);
            if (reviewAppUI != null && eval.willWriteReview)
            {
                reviewAppUI.AddReview(eval, customerName, isRegular);
            }

            if (persistenceManager != null && shiftManager != null && eval.willWriteReview)
            {
                persistenceManager.RecordCustomerReview(shiftManager.currentDay, customerName, eval, isRegular);
            }
        }
    }
}
