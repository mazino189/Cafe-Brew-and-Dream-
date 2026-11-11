using System;
using UnityEngine;
using UnityEngine.UI;
using BrewAndDream.Core.Engines;

namespace BrewAndDream.UI.FinancialReport
{
    /// <summary>
    /// Giao diện sổ sách tài chính kết ca (Ca trưa, Cuối ngày).
    /// Dựa trên màn hình: "s_s_ch_thu_chi_ca_tr_a_brew_dream" và "t_ng_k_t_cu_i_ng_y_12_brew_dream".
    /// </summary>
    public class ShiftLedgerUI : MonoBehaviour
    {
        [Header("UI Text References")]
        [SerializeField] private GameObject reportRoot;
        [SerializeField] private Text shiftTitleText;
        [SerializeField] private Text openingCashText;
        [SerializeField] private Text cashRevenueText;
        [SerializeField] private Text materialExpensesText;
        [SerializeField] private Text overheadExpensesText;
        [SerializeField] private Text netProfitText;
        [SerializeField] private Text completedOrdersText;
        [SerializeField] private Text avgSatisfactionText;

        public event Action OnAcknowledgeReport;

        public void DisplayLedger(ShiftLedger ledger)
        {
            if (reportRoot != null) reportRoot.SetActive(true);

            if (shiftTitleText != null) shiftTitleText.text = $"SỔ SÁCH TÀI CHÍNH — {ledger.shiftName.ToUpper()}";
            if (openingCashText != null) openingCashText.text = $"{ledger.openingCash:N0} đ";
            if (cashRevenueText != null) cashRevenueText.text = $"+{ledger.cashRevenue:N0} đ";
            if (materialExpensesText != null) materialExpensesText.text = $"-{ledger.rawMaterialExpenses:N0} đ";
            if (overheadExpensesText != null) overheadExpensesText.text = $"-{ledger.overheadExpenses:N0} đ";
            if (netProfitText != null)
            {
                netProfitText.text = $"{ledger.netProfit:N0} đ";
                netProfitText.color = ledger.netProfit >= 0 ? new Color(0.22f, 0.41f, 0.23f) : new Color(0.78f, 0.29f, 0.19f);
            }
            if (completedOrdersText != null) completedOrdersText.text = $"{ledger.ordersCompleted} đơn";
            if (avgSatisfactionText != null) avgSatisfactionText.text = $"{ledger.AverageSatisfaction:F1} / 100 điểm";
        }

        public void ConfirmAndClose()
        {
            if (reportRoot != null) reportRoot.SetActive(false);
            OnAcknowledgeReport?.Invoke();
        }
    }
}

