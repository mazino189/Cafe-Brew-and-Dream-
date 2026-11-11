using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrewAndDream.Narrative
{
    [Serializable]
    public class MiniQuestDefinition
    {
        public string questId;
        public string questTitle;
        public string customerArchetypeId;
        public int requiredVisits = 5;
        public string openingDialogue;
        public string targetRecipeId; // Món đồ uống đặc biệt NPC nhờ pha
        public string rewardDescription;
        public bool isCompleted = false;
    }

    /// <summary>
    /// Quản lý mạch truyện và chuỗi nhiệm vụ mini-quest của khách quen.
    /// Dựa trên Blueprint: Khi khách quen đạt ngưỡng hài lòng, mở khóa câu chuyện riêng.
    /// </summary>
    public class CustomerNarrativeManager : MonoBehaviour
    {
        public static CustomerNarrativeManager Instance { get; private set; }

        public List<MiniQuestDefinition> activeQuests = new List<MiniQuestDefinition>();
        public Dictionary<string, int> customerVisitCounters = new Dictionary<string, int>();

        public event Action<MiniQuestDefinition> OnQuestUnlocked;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            InitializeScriptedQuests();
        }

        private void InitializeScriptedQuests()
        {
            activeQuests.Add(new MiniQuestDefinition
            {
                questId = "quest_critic_nostalgia",
                questTitle = "Ký ức tách cà phê phin phố cổ",
                customerArchetypeId = "StrictAuntie",
                requiredVisits = 3,
                openingDialogue = "Hồi xưa ở phố cũ, tôi hay uống cà phê phin nhỏ từng giọt chậm rãi. Cháu có pha lại được đúng vị ấy không?",
                targetRecipeId = "recipe_cf_phin_truyenthong",
                rewardDescription = "Mở khóa Danh hiệu 'Quán quen phố thị' & Tăng 15% khách lớn tuổi"
            });

            activeQuests.Add(new MiniQuestDefinition
            {
                questId = "quest_blogger_viral",
                questTitle = "Món nước triệu view TikTok",
                customerArchetypeId = "BloggerTikToker",
                requiredVisits = 4,
                openingDialogue = "Quán bạn có view hoàng hôn chill quá! Làm cho mình một ly nước phân tầng bắt mắt để mình lên clip nhé!",
                targetRecipeId = "recipe_tra_dao_cam_sa",
                rewardDescription = "Quán viral trên mạng xã hội: Lượng khách tăng gấp đôi trong 2 ngày tiếp theo"
            });
        }

        public void RecordCustomerVisit(string archetypeId)
        {
            if (!customerVisitCounters.ContainsKey(archetypeId))
                customerVisitCounters[archetypeId] = 0;

            customerVisitCounters[archetypeId]++;

            var quest = activeQuests.Find(q => q.customerArchetypeId == archetypeId && !q.isCompleted);
            if (quest != null && customerVisitCounters[archetypeId] >= quest.requiredVisits)
            {
                OnQuestUnlocked?.Invoke(quest);
            }
        }
    }
}

