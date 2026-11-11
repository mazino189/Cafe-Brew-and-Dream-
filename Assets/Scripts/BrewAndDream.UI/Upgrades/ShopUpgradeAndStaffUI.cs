using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrewAndDream.UI.Upgrades
{
    public enum UpgradeCategory
    {
        CoffeeMachine, // Nâng cấp máy pha/máy xay
        Interior,      // Bàn ghế, máy lạnh, wifi tốc độ cao
        StaffHiring    // Tuyển Barista bán thời gian, phục vụ
    }

    [Serializable]
    public class UpgradeItem
    {
        public string id;
        public string title;
        public string description;
        public double costVND;
        public UpgradeCategory category;
        public bool isPurchased;
    }

    /// <summary>
    /// Giao diện Nâng cấp thiết bị, nội thất quán và Tuyển nhân viên.
    /// Dựa trên màn hình: "n_ng_c_p_qu_n_thu_nh_n_vi_n_brew_dream".
    /// </summary>
    public class ShopUpgradeAndStaffUI : MonoBehaviour
    {
        [Header("UI Root")]
        [SerializeField] private GameObject upgradeRoot;

        [Header("Danh sách nâng cấp")]
        [SerializeField] private Transform itemsContainer;
        [SerializeField] private GameObject upgradeCardPrefab;

        public event Action<UpgradeItem> OnPurchaseUpgrade;

        public void OpenUpgradeShop()
        {
            if (upgradeRoot != null) upgradeRoot.SetActive(true);
        }

        public void CloseUpgradeShop()
        {
            if (upgradeRoot != null) upgradeRoot.SetActive(false);
        }

        public void PurchaseItem(UpgradeItem item)
        {
            if (!item.isPurchased)
            {
                item.isPurchased = true;
                OnPurchaseUpgrade?.Invoke(item);
            }
        }
    }
}

