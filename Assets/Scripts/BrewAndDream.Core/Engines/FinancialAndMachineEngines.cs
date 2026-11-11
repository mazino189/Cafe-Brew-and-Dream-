using System;
using System.Collections.Generic;

namespace BrewAndDream.Core.Engines
{
    [Serializable]
    public class ShiftLedger
    {
        public string shiftName;
        public double openingCash;
        public double cashRevenue;
        public double rawMaterialExpenses;
        public double overheadExpenses; // tiền điện, nước, phạt
        public double closingCash => openingCash + cashRevenue - rawMaterialExpenses - overheadExpenses;
        public double netProfit => cashRevenue - rawMaterialExpenses - overheadExpenses;

        public int ordersCompleted;
        public int ordersFailed;
        public float totalSatisfactionSum;

        public float AverageSatisfaction => ordersCompleted > 0 ? (totalSatisfactionSum / ordersCompleted) : 0f;
    }

    public static class MachineMaintenanceEngine
    {
        /// <summary>
        /// Tính tỷ lệ hỏng hóc hoặc quá tải của máy móc (máy xay cafe, máy pha espresso)
        /// </summary>
        public static float CalculateBreakdownChance(int dailyCycles, int maxSafeCycles, float maintenanceCondition01)
        {
            float baseRate = 0.02f;
            float usageFactor = (float)dailyCycles / Math.Max(1, maxSafeCycles);
            float wear = (float)Math.Pow(usageFactor, 2.0);
            float maintenanceEffect = Math.Clamp(1f - maintenanceCondition01, 0f, 1f);

            return Math.Clamp(baseRate + (wear * 0.4f * maintenanceEffect), 0f, 1f);
        }
    }
}

