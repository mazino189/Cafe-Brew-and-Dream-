using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrewAndDream.Core.Models;

namespace BrewAndDream.UI.Inventory
{
    /// <summary>
    /// Bảng quản lý kho nguyên vật liệu và đặt hàng từ nhà cung cấp.
    /// Dựa trên màn hình: "kho_nh_p_nguy_n_li_u_brew_dream".
    /// </summary>
    public class InventoryWarehouseUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject warehouseRoot;
        [SerializeField] private Text totalInventoryCostText;
        [SerializeField] private Text warningExpiringText;
        [SerializeField] private Button orderSupplierButton;

        public event Action<string, float> OnPurchaseRestock; // ingredientId, amount

        public void OpenWarehouse()
        {
            if (warehouseRoot != null) warehouseRoot.SetActive(true);
            RefreshWarehouseDisplay();
        }

        public void CloseWarehouse()
        {
            if (warehouseRoot != null) warehouseRoot.SetActive(false);
        }

        public void RefreshWarehouseDisplay()
        {
            // Cập nhật thông số tồn kho & cảnh báo cận date
        }

        public void RequestRestock(string ingredientId, float amount)
        {
            OnPurchaseRestock?.Invoke(ingredientId, amount);
            RefreshWarehouseDisplay();
        }
    }
}

