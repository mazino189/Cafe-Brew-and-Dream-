using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrewAndDream.Core.Models;

namespace BrewAndDream.UI.POS
{
    [Serializable]
    public class OrderItemEntry
    {
        public DrinkRecipeModel recipe;
        public int quantity;
        public float subtotal => (recipe != null ? recipe.referenceMarketPriceVND : 0) * quantity;
    }

    /// <summary>
    /// Giao diện máy tính tiền POS thu ngân trong game.
    /// Dựa trên màn hình: "qu_y_order_thu_ng_n_trong_game_in_game_pos_overlay".
    /// </summary>
    public class POSOverlayUI : MonoBehaviour
    {
        [Header("Danh sách order hiện tại")]
        public List<OrderItemEntry> currentCart = new List<OrderItemEntry>();

        [Header("UI References")]
        [SerializeField] private GameObject overlayRoot;
        [SerializeField] private Text orderNumberText;
        [SerializeField] private Text totalPriceText;
        [SerializeField] private Transform orderListContainer;
        [SerializeField] private GameObject orderItemPrefab;

        public event Action<List<OrderItemEntry>, float> OnOrderCompleted;

        public void OpenPOS(int orderNumber)
        {
            if (overlayRoot != null) overlayRoot.SetActive(true);
            if (orderNumberText != null) orderNumberText.text = $"ĐƠN HÀNG #{orderNumber:D4}";
            currentCart.Clear();
            UpdateCartUI();
        }

        public void ClosePOS()
        {
            if (overlayRoot != null) overlayRoot.SetActive(false);
        }

        public void AddRecipeToCart(DrinkRecipeModel recipe)
        {
            var existing = currentCart.Find(item => item.recipe.recipeId == recipe.recipeId);
            if (existing != null)
            {
                existing.quantity++;
            }
            else
            {
                currentCart.Add(new OrderItemEntry { recipe = recipe, quantity = 1 });
            }
            UpdateCartUI();
        }

        public void ConfirmPayment()
        {
            float total = 0f;
            foreach (var item in currentCart)
            {
                total += item.subtotal;
            }

            OnOrderCompleted?.Invoke(currentCart, total);
            ClosePOS();
        }

        private void UpdateCartUI()
        {
            float total = 0f;
            foreach (var item in currentCart)
            {
                total += item.subtotal;
            }

            if (totalPriceText != null)
            {
                totalPriceText.text = $"{total:N0} đ";
            }
        }
    }
}

