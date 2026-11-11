using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrewAndDream.Core.Models;

namespace BrewAndDream.UI.Reviews
{
    [Serializable]
    public struct CustomerReviewItem
    {
        public string customerName;
        public string avatarIconName;
        public int starRating;
        public string reviewContent;
        public string timestampText;
        public bool isRegular;
    }

    /// <summary>
    /// Giao diện ứng dụng đánh giá quán & danh sách khách quen (Food Reviewer App).
    /// Dựa trên màn hình: "app_nh_gi_kh_ch_h_ng_qu_n_quen_review".
    /// </summary>
    public class CustomerReviewAppUI : MonoBehaviour
    {
        [Header("UI Root")]
        [SerializeField] private GameObject appRoot;

        [Header("Chỉ số tổng quan")]
        [SerializeField] private Text averageRatingText; // Ví dụ: "4.8 ★"
        [SerializeField] private Text totalReviewsCountText;
        [SerializeField] private Text regularCustomersCountText;

        [Header("Danh sách review")]
        [SerializeField] private Transform reviewListContainer;
        [SerializeField] private GameObject reviewCardPrefab;

        private List<CustomerReviewItem> _allReviews = new List<CustomerReviewItem>();

        public void OpenApp()
        {
            if (appRoot != null) appRoot.SetActive(true);
            RefreshReviewList();
        }

        public void CloseApp()
        {
            if (appRoot != null) appRoot.SetActive(false);
        }

        public void AddReview(EvaluationOutput eval, string customerName, bool isRegular)
        {
            string content = eval.starRating switch
            {
                5 => $"Quán pha cực ngon! Đặc biệt thích {eval.dominantPraiseCriterion}.",
                4 => $"Khá ổn áp, đồ uống vừa miệng. Sẽ quay lại.",
                3 => $"Bình thường, {eval.dominantComplaintCriterion} cần cải thiện chút.",
                2 => $"Không hợp gu lắm, phục vụ hơi lâu.",
                _ => $"Rất thất vọng về {eval.dominantComplaintCriterion}! Chắc chắn không quay lại."
            };

            var item = new CustomerReviewItem
            {
                customerName = customerName,
                starRating = eval.starRating,
                reviewContent = content,
                timestampText = "Hôm nay",
                isRegular = isRegular
            };

            _allReviews.Insert(0, item);
            RefreshReviewList();
        }

        private void RefreshReviewList()
        {
            if (_allReviews.Count == 0) return;

            float sum = 0f;
            int regularCount = 0;
            foreach (var r in _allReviews)
            {
                sum += r.starRating;
                if (r.isRegular) regularCount++;
            }

            float avg = sum / _allReviews.Count;
            if (averageRatingText != null) averageRatingText.text = $"{avg:F1} ★";
            if (totalReviewsCountText != null) totalReviewsCountText.text = $"({_allReviews.Count} đánh giá)";
            if (regularCustomersCountText != null) regularCustomersCountText.text = $"{regularCount} khách quen";
        }
    }
}

