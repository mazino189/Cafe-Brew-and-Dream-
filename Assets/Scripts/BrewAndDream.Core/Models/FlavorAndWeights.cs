using System;

namespace BrewAndDream.Core.Models
{
    /// <summary>
    /// Hồ sơ hương vị chuẩn hóa trên thang [0, 1] để so khớp giữa Đồ uống và Gu vị của NPC.
    /// </summary>
    [Serializable]
    public struct FlavorProfile
    {
        public float sweetness;  // Độ ngọt [0, 1]
        public float bitterness; // Độ đắng / đậm cafe hoặc trà [0, 1]
        public float creaminess; // Độ béo ngậy (sữa đặc, kem béo) [0, 1]
        public float aroma;      // Hương thơm đặc trưng [0, 1]
        public float novelty;    // Độ độc lạ (0: truyền thống quen thuộc, 1: sáng tạo lạ miệng)

        public FlavorProfile(float sweetness, float bitterness, float creaminess, float aroma, float novelty)
        {
            this.sweetness = Math.Clamp(sweetness, 0f, 1f);
            this.bitterness = Math.Clamp(bitterness, 0f, 1f);
            this.creaminess = Math.Clamp(creaminess, 0f, 1f);
            this.aroma = Math.Clamp(aroma, 0f, 1f);
            this.novelty = Math.Clamp(novelty, 0f, 1f);
        }
    }

    /// <summary>
    /// Trọng số 6 tiêu chí đánh giá của mỗi NPC (Tốc độ, Chất lượng, Thái độ, Vệ sinh, Không gian, Giá cả).
    /// </summary>
    [Serializable]
    public struct EvaluationWeights
    {
        public float speedWeight;
        public float qualityWeight;
        public float attitudeWeight;
        public float hygieneWeight;
        public float ambianceWeight;
        public float priceWeight;

        public float TotalWeight =>
            Math.Max(0.0001f, speedWeight + qualityWeight + attitudeWeight + hygieneWeight + ambianceWeight + priceWeight);
    }
}

