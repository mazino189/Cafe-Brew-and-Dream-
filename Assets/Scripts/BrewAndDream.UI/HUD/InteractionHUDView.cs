using System;
using UnityEngine;
using UnityEngine.UI;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.UI.HUD
{
    /// <summary>
    /// Hiển thị tâm ngắm (Crosshair) và nhãn hướng dẫn phím bấm khi nhìn vào các trạm pha chế.
    /// Tương ứng với hướng dẫn UI: "Nhấn E để tương tác", "Chuột phải để đặt xuống".
    /// </summary>
    public class InteractionHUDView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private PlayerInteractionController playerInteraction;
        [SerializeField] private GameObject promptPanel;
        [SerializeField] private Text promptText;
        [SerializeField] private Image crosshairImage;

        [Header("Cấu hình màu sắc (Theo Design System)")]
        [SerializeField] private Color normalCrosshairColor = new Color(1f, 1f, 1f, 0.7f);
        [SerializeField] private Color hoverCrosshairColor = new Color(0.9f, 0.66f, 0.24f, 1f); // #E5A93C Amber Honey

        private void OnEnable()
        {
            if (playerInteraction != null)
            {
                playerInteraction.OnHoverPromptChanged += HandlePromptChanged;
            }
        }

        private void OnDisable()
        {
            if (playerInteraction != null)
            {
                playerInteraction.OnHoverPromptChanged -= HandlePromptChanged;
            }
        }

        private void HandlePromptChanged(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                if (promptPanel != null) promptPanel.SetActive(false);
                if (crosshairImage != null) crosshairImage.color = normalCrosshairColor;
            }
            else
            {
                if (promptPanel != null) promptPanel.SetActive(true);
                if (promptText != null) promptText.text = prompt;
                if (crosshairImage != null) crosshairImage.color = hoverCrosshairColor;
            }
        }
    }
}

