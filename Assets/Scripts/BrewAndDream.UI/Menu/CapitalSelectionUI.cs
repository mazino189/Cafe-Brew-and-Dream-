using System;
using UnityEngine;
using UnityEngine.UI;

namespace BrewAndDream.UI.Menu
{
    public enum CapitalChoice
    {
        Hard30M,   // 30 triệu (Khó - x2.0 điểm)
        Medium100M, // 100 triệu (Vừa - x1.5 điểm)
        Easy300M   // 300 triệu (Dễ - x1.0 điểm)
    }

    /// <summary>
    /// Màn hình Chọn vốn khởi nghiệp & Mô hình quán nước.
    /// Dựa trên màn hình: "kh_i_nghi_p_qu_n_n_c_ch_n_v_n_m_h_nh".
    /// </summary>
    public class CapitalSelectionUI : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject menuRoot;

        [Header("Các Card chọn vốn")]
        [SerializeField] private Button cardHardBtn;
        [SerializeField] private Button cardMediumBtn;
        [SerializeField] private Button cardEasyBtn;

        [Header("Nút Bắt đầu")]
        [SerializeField] private Button startJourneyBtn;
        [SerializeField] private Text selectedSummaryText;

        private CapitalChoice _selectedChoice = CapitalChoice.Medium100M;
        public event Action<CapitalChoice, double> OnGameStartedWithCapital;

        private void Start()
        {
            if (cardHardBtn != null) cardHardBtn.onClick.AddListener(() => SelectCapital(CapitalChoice.Hard30M));
            if (cardMediumBtn != null) cardMediumBtn.onClick.AddListener(() => SelectCapital(CapitalChoice.Medium100M));
            if (cardEasyBtn != null) cardEasyBtn.onClick.AddListener(() => SelectCapital(CapitalChoice.Easy300M));
            if (startJourneyBtn != null) startJourneyBtn.onClick.AddListener(ConfirmAndStart);

            SelectCapital(CapitalChoice.Medium100M);
        }

        public void SelectCapital(CapitalChoice choice)
        {
            _selectedChoice = choice;
            if (selectedSummaryText != null)
            {
                switch (choice)
                {
                    case CapitalChoice.Hard30M:
                        selectedSummaryText.text = "Vốn: 30,000,000 đ (Khó) — Hệ số điểm x2.0";
                        break;
                    case CapitalChoice.Medium100M:
                        selectedSummaryText.text = "Vốn: 100,000,000 đ (Vừa) — Hệ số điểm x1.5";
                        break;
                    case CapitalChoice.Easy300M:
                        selectedSummaryText.text = "Vốn: 300,000,000 đ (Dễ) — Hệ số điểm x1.0";
                        break;
                }
            }
        }

        private void ConfirmAndStart()
        {
            double capital = _selectedChoice switch
            {
                CapitalChoice.Hard30M => 30000000,
                CapitalChoice.Medium100M => 100000000,
                CapitalChoice.Easy300M => 300000000,
                _ => 100000000
            };

            if (menuRoot != null) menuRoot.SetActive(false);
            OnGameStartedWithCapital?.Invoke(_selectedChoice, capital);
        }
    }
}

