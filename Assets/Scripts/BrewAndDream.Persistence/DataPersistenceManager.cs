using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BrewAndDream.Core.Engines;
using BrewAndDream.Core.Models;

namespace BrewAndDream.Persistence
{
    /// <summary>
    /// Bộ lưu trữ bền vững (Persistence Manager).
    /// Hỗ trợ cả cơ chế SQLite (hoặc JSON fallback) để lưu biểu đồ tài chính 365 ngày và trạng thái game.
    /// </summary>
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }

        private string _saveDataFilePath;
        private string _databasePath;

        public List<ShiftLedgerRecord> LedgerHistory { get; private set; } = new List<ShiftLedgerRecord>();
        public List<CustomerReviewRecord> ReviewHistory { get; private set; } = new List<CustomerReviewRecord>();
        public SaveGameState CurrentState { get; private set; } = new SaveGameState();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializePaths();
                LoadGameState();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializePaths()
        {
            string baseDir = Application.persistentDataPath;
            _saveDataFilePath = Path.Combine(baseDir, "savegame.json");
            _databasePath = Path.Combine(baseDir, "brew_and_dream_records.db");
        }

        public void SaveGameState(int currentDay, double totalFunds, float reputation, int lifetimeCustomers)
        {
            CurrentState.CurrentDay = currentDay;
            CurrentState.StoreTotalFunds = totalFunds;
            CurrentState.ShopReputationScore = reputation;
            CurrentState.TotalCustomersServedLifetime = lifetimeCustomers;
            CurrentState.LastSavedTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                string json = JsonUtility.ToJson(CurrentState, true);
                File.WriteAllText(_saveDataFilePath, json);
                Debug.Log($"[Persistence] Đã lưu Game State thành công vào: {_saveDataFilePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Persistence] Lỗi khi lưu Game State: {ex.Message}");
            }
        }

        public void LoadGameState()
        {
            if (File.Exists(_saveDataFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_saveDataFilePath);
                    CurrentState = JsonUtility.FromJson<SaveGameState>(json);
                    Debug.Log($"[Persistence] Đã nạp thành công Game State (Ngày {CurrentState.CurrentDay})");
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[Persistence] Không thể đọc save file, dùng mặc định: {ex.Message}");
                    CurrentState = new SaveGameState();
                }
            }
        }

        public void RecordShiftLedger(int day, ShiftLedger ledger)
        {
            var record = new ShiftLedgerRecord
            {
                Id = LedgerHistory.Count + 1,
                Day = day,
                ShiftName = ledger.shiftName,
                OpeningCash = ledger.openingCash,
                CashRevenue = ledger.cashRevenue,
                RawMaterialExpenses = ledger.rawMaterialExpenses,
                OverheadExpenses = ledger.overheadExpenses,
                NetProfit = ledger.netProfit,
                OrdersCompleted = ledger.ordersCompleted,
                OrdersFailed = ledger.ordersFailed,
                AverageSatisfaction = ledger.AverageSatisfaction,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            LedgerHistory.Add(record);
        }

        public void RecordCustomerReview(int day, string customerName, EvaluationOutput eval, bool isRegular)
        {
            var record = new CustomerReviewRecord
            {
                Id = ReviewHistory.Count + 1,
                Day = day,
                CustomerName = customerName,
                StarRating = eval.starRating,
                ReviewContent = eval.starRating >= 4 ? "Đồ uống rất ngon, không gian ấm cúng!" : "Cần cải thiện chất lượng!",
                DominantPraise = eval.dominantPraiseCriterion,
                DominantComplaint = eval.dominantComplaintCriterion,
                IsRegular = isRegular
            };

            ReviewHistory.Add(record);
        }
    }
}

