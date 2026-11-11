using System;

namespace BrewAndDream.Persistence
{
    /// <summary>
    /// Bản ghi lịch sử tài chính từng ca bán (Lưu trữ bền vững trong SQLite).
    /// </summary>
    public class ShiftLedgerRecord
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string ShiftName { get; set; }
        public double OpeningCash { get; set; }
        public double CashRevenue { get; set; }
        public double RawMaterialExpenses { get; set; }
        public double OverheadExpenses { get; set; }
        public double NetProfit { get; set; }
        public int OrdersCompleted { get; set; }
        public int OrdersFailed { get; set; }
        public float AverageSatisfaction { get; set; }
        public string Timestamp { get; set; }
    }

    /// <summary>
    /// Bản ghi lịch sử review khách hàng đã đăng lên app trong suốt 365 ngày.
    /// </summary>
    public class CustomerReviewRecord
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public string CustomerName { get; set; }
        public int StarRating { get; set; }
        public string ReviewContent { get; set; }
        public string DominantPraise { get; set; }
        public string DominantComplaint { get; set; }
        public bool IsRegular { get; set; }
    }

    /// <summary>
    /// Trạng thái lưu game tổng thể (Game Save State).
    /// </summary>
    public class SaveGameState
    {
        public int CurrentDay { get; set; } = 1;
        public double StoreTotalFunds { get; set; } = 50000000;
        public float ShopReputationScore { get; set; } = 75.0f;
        public int TotalCustomersServedLifetime { get; set; } = 0;
        public string LastSavedTimestamp { get; set; }
    }
}

