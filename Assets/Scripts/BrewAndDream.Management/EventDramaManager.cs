using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.UI.Dialogues;

namespace BrewAndDream.Management
{
    [Serializable]
    public class DramaEventDefinition
    {
        public string eventId;
        public string title;
        [TextArea(2, 4)] public string description;
        public ShiftType applicableShift;
        [Range(0f, 1f)] public float triggerChance = 0.25f;

        public DramaChoice choiceA;
        public DramaChoice choiceB;
        public DramaChoice choiceC;
    }

    /// <summary>
    /// Bộ điều phối các tình huống Drama và Biến cố ngẫu nhiên trong quán.
    /// Dựa trên kịch bản: Cúp điện đột ngột, Máy xay cà phê quá tải bốc khói, 
    /// Shipper giao trễ ca trưa, Khách hàng khó tính bắt đền, và Reviewer 1 sao.
    /// </summary>
    public class EventDramaManager : MonoBehaviour
    {
        [Header("UI Dialog")]
        [SerializeField] private DramaEventDialogUI dramaDialog;

        [Header("Kho biến cố kịch bản")]
        public List<DramaEventDefinition> scriptedEvents = new List<DramaEventDefinition>();

        public event Action<DramaChoice> OnDramaResolved;

        private void Awake()
        {
            InitializeDefaultScriptedEvents();
        }

        private void InitializeDefaultScriptedEvents()
        {
            // 1. Biến cố: Máy xay cà phê quá tải bốc khói (Ca trưa)
            scriptedEvents.Add(new DramaEventDefinition
            {
                eventId = "drama_grinder_overheat",
                title = "SỰ CỐ: MÁY XAY CÀ PHÊ BỐC KHÓI!",
                description = "Giờ cao điểm trưa, máy xay cà phê phát tiếng kêu lạ rồi khét lẹt bốc khói. Hơn 5 khách văn phòng đang sốt ruột chờ ly cà phê mang đi.",
                applicableShift = ShiftType.NoonRush,
                triggerChance = 0.3f,
                choiceA = new DramaChoice
                {
                    choiceText = "A. Tắt máy ngay, chuyển sang pha phin truyền thống & xin lỗi khách",
                    outcomeDescription = "Khách chờ hơi lâu một chút nhưng thông cảm vì hương vị phin đậm đà.",
                    reputationImpact = +0.5f,
                    costImpactVND = 0
                },
                choiceB = new DramaChoice
                {
                    choiceText = "B. Ép máy chạy tiếp để kịp đơn giao khách",
                    outcomeDescription = "Máy xay cháy hẳn mô tơ! Bột cà phê bị khét làm khách chê thậm tệ.",
                    reputationImpact = -3.0f,
                    costImpactVND = -2500000 // Tốn 2.5 triệu thay motor
                },
                choiceC = new DramaChoice
                {
                    choiceText = "C. Hoàn tiền 100% kèm voucher giảm 50% cho lần ghé sau",
                    outcomeDescription = "Khách vui vẻ rời đi, khen ngợi thái độ phục vụ chuyên nghiệp của quán.",
                    reputationImpact = +2.0f,
                    costImpactVND = -150000
                }
            });

            // 2. Biến cố: Cúp điện bất ngờ ca tối (Acoustic chill)
            scriptedEvents.Add(new DramaEventDefinition
            {
                eventId = "drama_blackout_evening",
                title = "BIẾN CỐ: MẤT ĐIỆN TOÀN KHU VỰC!",
                description = "Đang trong ca tối lãng mạn, toàn bộ khu phố mất điện đột ngột. Máy pha không hoạt động được, quầy pha chế tối om.",
                applicableShift = ShiftType.AfternoonEvening,
                triggerChance = 0.25f,
                choiceA = new DramaChoice
                {
                    choiceText = "A. Thắp nến thơm toàn quán & chuyển sang đêm nhạc mộc mạc",
                    outcomeDescription = "Không gian trở nên siêu lãng mạn! Khách check-in rần rần trên mạng xã hội.",
                    reputationImpact = +4.0f,
                    costImpactVND = -50000 // Tiền mua nến
                },
                choiceB = new DramaChoice
                {
                    choiceText = "B. Đóng cửa quán sớm, xin lỗi và mời khách về",
                    outcomeDescription = "Mất toàn bộ doanh thu ca tối, khách hơi hụt hẫng.",
                    reputationImpact = -1.0f,
                    costImpactVND = 0
                },
                choiceC = new DramaChoice
                {
                    choiceText = "C. Bật máy phát điện mini dự phòng để chạy tủ lạnh và quạt",
                    outcomeDescription = "Quán duy trì hoạt động bình thường nhưng tiếng máy phát điện hơi ồn.",
                    reputationImpact = +0.5f,
                    costImpactVND = -120000 // Tiền xăng
                }
            });

            // 3. Biến cố: Khách khó tính bắt bẻ vị ngọt & đe dọa bóc phốt 1 sao
            scriptedEvents.Add(new DramaEventDefinition
            {
                eventId = "drama_angry_customer_sweetness",
                title = "DRAMA: BÀ CÔ KHÓ TÍNH ĐÒI BÓC PHỐT!",
                description = "Một vị khách lớn tiếng tại quầy: 'Trà này ngọt như chè! Quán làm ăn cẩu thả coi thường sức khỏe khách hàng, tôi sẽ lên mạng bóc phốt!'",
                applicableShift = ShiftType.Morning,
                triggerChance = 0.2f,
                choiceA = new DramaChoice
                {
                    choiceText = "A. Nhã nhặn lắng nghe, pha tặng ngay ly mới ít đường đúng gu",
                    outcomeDescription = "Khách dịu giọng, cảm kích sự chu đáo và trở thành khách ruột.",
                    reputationImpact = +2.5f,
                    costImpactVND = -15000 // Chi phí ly nước pha lại
                },
                choiceB = new DramaChoice
                {
                    choiceText = "B. Đôi co tranh cãi, khẳng định quán đã đong đúng định lượng",
                    outcomeDescription = "Khách đập bàn bỏ về và viết bài bóc phốt 1 sao dài đặc trên Food Reviewer.",
                    reputationImpact = -5.0f,
                    costImpactVND = 0
                },
                choiceC = new DramaChoice
                {
                    choiceText = "C. Mời khách vào bàn yên tĩnh, xin lỗi chân thành và tặng bánh ngọt",
                    outcomeDescription = "Khách hài lòng vì được tôn trọng, vui vẻ thanh toán thêm tiền bánh.",
                    reputationImpact = +1.5f,
                    costImpactVND = -25000
                }
            });
        }

        public void TryTriggerShiftDrama(ShiftType shift, Action<DramaChoice> onResolved)
        {
            var candidates = scriptedEvents.FindAll(e => e.applicableShift == shift);
            if (candidates.Count == 0) return;

            // Chọn ngẫu nhiên biến cố
            int index = UnityEngine.Random.Range(0, candidates.Count);
            var selected = candidates[index];

            if (UnityEngine.Random.value <= selected.triggerChance)
            {
                if (dramaDialog != null)
                {
                    dramaDialog.ShowEvent(
                        selected.title,
                        selected.description,
                        selected.choiceA,
                        selected.choiceB,
                        selected.choiceC
                    );

                    dramaDialog.OnChoiceSelected += (choice) =>
                    {
                        OnDramaResolved?.Invoke(choice);
                        onResolved?.Invoke(choice);
                    };
                }
            }
        }
    }
}

