using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BrewAndDream.Core.Models;
using BrewAndDream.UnityBridge;

namespace BrewAndDream.Core
{
    [Serializable]
    public class ReviewPostData
    {
        public string authorName;
        public int starRating;
        public string content;
        public DateTime timestamp;
        public int socialLikesCount;
    }

    [CreateAssetMenu(fileName = "ReviewTemplateLibrarySO", menuName = "BrewAndDream/ScriptableObjects/Review Template Library")]
    public class ReviewTemplateLibrarySO : ScriptableObject
    {
        public List<string> fiveStarTemplates = new List<string>
        {
            "Quán pha cực ngon! Đặc biệt thích {praise}.",
            "Hương vị chuẩn gu, không gian ấm cúng. Điểm 10 chất lượng!",
            "10/10 không có nhưng! Chắc chắn sẽ dẫn bạn bè ghé lại quán."
        };

        public List<string> fourStarTemplates = new List<string>
        {
            "Khá ổn áp, đồ uống vừa miệng. Sẽ quay lại.",
            "Nước ngon, phục vụ ổn, chỉ cần chú ý {complaint} một chút là hoàn hảo."
        };

        public List<string> threeStarTemplates = new List<string>
        {
            "Tạm được, trải nghiệm ở mức trung bình.",
            "Uống cũng được nhưng {complaint} cần cải thiện hơn."
        };

        public List<string> lowStarTemplates = new List<string>
        {
            "Khá thất vọng, {complaint} tệ quá!",
            "Không hợp gu chút nào, chờ lâu mà đồ uống lại không ưng ý.",
            "Rất tệ, {complaint} làm mình mất hứng hoàn toàn. Sẽ không ghé lại!"
        };
    }

    public static class BrewAndDreamReviewGenerator
    {
        private static readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        private const int MaxCacheSize = 200;

        public static ReviewTemplateLibrarySO TemplateLibrary { get; set; }

        public static async Task<ReviewPostData> GenerateReviewAsync(NPCPersonalitySO npc, EvaluationOutput eval)
        {
            string author = npc != null ? npc.archetypeName : "Khách vãng lai";
            string key = $"{author}_{eval.starRating}_{eval.dominantPraiseCriterion}_{eval.dominantComplaintCriterion}";

            if (_cache.TryGetValue(key, out var cachedContent))
            {
                return new ReviewPostData
                {
                    authorName = author,
                    starRating = eval.starRating,
                    content = cachedContent,
                    timestamp = DateTime.UtcNow,
                    socialLikesCount = UnityEngine.Random.Range(2, 45)
                };
            }

            await Task.Yield();

            string rawTemplate = GetTemplateForRating(eval.starRating);
            string review = rawTemplate
                .Replace("{praise}", string.IsNullOrEmpty(eval.dominantPraiseCriterion) ? "hương vị" : eval.dominantPraiseCriterion)
                .Replace("{complaint}", string.IsNullOrEmpty(eval.dominantComplaintCriterion) ? "thời gian chờ" : eval.dominantComplaintCriterion);

            if (_cache.Count >= MaxCacheSize)
            {
                var keys = new List<string>(_cache.Keys);
                if (keys.Count > 0) _cache.Remove(keys[0]);
            }
            _cache[key] = review;

            return new ReviewPostData
            {
                authorName = author,
                starRating = eval.starRating,
                content = review,
                timestamp = DateTime.UtcNow,
                socialLikesCount = UnityEngine.Random.Range(1, 30)
            };
        }

        private static string GetTemplateForRating(int starRating)
        {
            if (TemplateLibrary != null)
            {
                List<string> pool = starRating switch
                {
                    5 => TemplateLibrary.fiveStarTemplates,
                    4 => TemplateLibrary.fourStarTemplates,
                    3 => TemplateLibrary.threeStarTemplates,
                    _ => TemplateLibrary.lowStarTemplates
                };

                if (pool != null && pool.Count > 0)
                {
                    return pool[UnityEngine.Random.Range(0, pool.Count)];
                }
            }

            return starRating switch
            {
                5 => "Quán pha cực ngon! Đặc biệt thích {praise}.",
                4 => "Khá ổn áp, đồ uống vừa miệng. Sẽ quay lại.",
                3 => "Bình thường, {complaint} cần cải thiện một chút.",
                2 => "Không hợp gu lắm, phục vụ chưa được tốt.",
                _ => "Rất thất vọng về {complaint}! Chắc chắn không quay lại."
            };
        }
    }
}
