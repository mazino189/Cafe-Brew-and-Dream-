using System;
using UnityEngine;
using UnityEngine.UI;

namespace BrewAndDream.UI.Dialogues
{
    [Serializable]
    public struct DramaChoice
    {
        public string choiceText;
        public string outcomeDescription;
        public float reputationImpact;
        public float costImpactVND;
    }

    /// <summary>
    /// Popup hộp thoại giải quyết sự cố / drama ngẫu nhiên.
    /// Dựa trên màn hình: "h_p_tho_i_s_ki_n_kh_ch_ki_m_chuy_n_brew_dream", "s_ki_n_b_t_ng_m_t_i_n_t_ng_t_brew_dream".
    /// </summary>
    public class DramaEventDialogUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject dialogRoot;
        [SerializeField] private Text eventTitleText;
        [SerializeField] private Text eventDescriptionText;
        [SerializeField] private Image eventIllustration;

        [Header("Các nút lựa chọn phương án (A, B, C)")]
        [SerializeField] private Button choiceButtonA;
        [SerializeField] private Text choiceTextA;
        [SerializeField] private Button choiceButtonB;
        [SerializeField] private Text choiceTextB;
        [SerializeField] private Button choiceButtonC;
        [SerializeField] private Text choiceTextC;

        public event Action<DramaChoice> OnChoiceSelected;

        public void ShowEvent(string title, string description, DramaChoice a, DramaChoice b, DramaChoice c)
        {
            if (dialogRoot != null) dialogRoot.SetActive(true);
            if (eventTitleText != null) eventTitleText.text = title;
            if (eventDescriptionText != null) eventDescriptionText.text = description;

            SetupButton(choiceButtonA, choiceTextA, a);
            SetupButton(choiceButtonB, choiceTextB, b);
            SetupButton(choiceButtonC, choiceTextC, c);
        }

        private void SetupButton(Button btn, Text label, DramaChoice choice)
        {
            if (btn == null) return;
            btn.onClick.RemoveAllListeners();
            if (label != null) label.text = choice.choiceText;
            btn.onClick.AddListener(() =>
            {
                if (dialogRoot != null) dialogRoot.SetActive(false);
                OnChoiceSelected?.Invoke(choice);
            });
        }
    }
}

