# BREW & DREAM — TÀI LIỆU HƯỚNG DẪN PHÁT TRIỂN & KIẾN TRÚC CODE (ANTIGRAVITY BLUEPRINT)

Tài liệu này đóng gói toàn bộ kịch bản thiết kế, lựa chọn công nghệ, công thức toán học, bảng thông số cân bằng và mã nguồn C# cốt lõi của game **Brew & Dream**, dùng làm ngữ cảnh chuẩn (Single Source of Truth) để AI Coding Agent (Antigravity) dựng project và viết code hoàn chỉnh trên Unity.

---

## 1. TỔNG QUAN DỰ ÁN & TECH STACK

- **Tên dự án (tạm)**: Brew & Dream
- **Thể loại**: Mô phỏng khởi nghiệp quán nước góc nhìn thứ nhất (3D First-Person Beverage Shop / Tycoon Simulation).
- **Thời lượng trong game**: 1 năm chia làm 4 Chương, nhiều cái kết dựa trên lợi nhuận, danh tiếng và lựa chọn chiến lược.
- **Phong cách đồ họa**: Low-poly / Stylized (Blender + Kenney Assets), nhẹ máy và đồng bộ ánh sáng.

### Công nghệ & Thư viện bắt buộc
- **Game Engine**: Unity 6 (URP - Universal Render Pipeline), ngôn ngữ C#.
- **Camera & Tương tác FPS**: Cinemachine 3 + DOTween (diễn hoạt cầm ly, đong rót, đặt đồ thay vì dùng vật lý chất lỏng phức tạp).
- **Quản lý Dữ liệu**: `ScriptableObject` (kết hợp Odin Inspector nếu có).
- **AI & Di chuyển**: Unity AI Navigation (`NavMeshAgent`) + Hierarchical State Machine (FSM).
- **Cốt truyện & Hội thoại**: Yarn Spinner (hoặc Ink) cho mini-quest khách quen.
- **Lưu trữ & Kinh tế**: JSON (trạng thái game) + `SQLite-net` (lưu lịch sử review và biểu đồ doanh thu 1 năm).
- **Âm thanh**: FMOD (nhạc nền đổi theo chủ đề quán, âm thanh pha chế, tiếng mưa/giờ cao điểm).

---

## 2. CẤU TRÚC THƯ MỤC UNITY ĐỀ XUẤT

Antigravity phải tổ chức mã nguồn theo cấu trúc sau để tách biệt rõ Data, Logic và UI:

```text
Assets/
└── _Project/
    ├── Art/ (Models, Materials, Prefabs, Animations)
    ├── Audio/ (FMOD Banks / AudioClips)
    ├── Data/
    │   ├── Ingredients/      # ScriptableObject: IngredientDataSO
    │   ├── Recipes/          # ScriptableObject: DrinkRecipeSO
    │   ├── NPCPersonalities/ # ScriptableObject: NPCPersonalitySO (8 mẫu)
    │   └── ReviewTemplates/  # ScriptableObject: ReviewTemplateLibrarySO
    ├── Dialogue/             # Yarn Spinner (.yarn) scripts cho mini-quest
    └── Scripts/
        ├── Core/             # BrewAndDreamCoreData.cs, GameManager, SaveSystem
        ├── Crafting/         # FPSController, RaycastInteractor, DrinkCrafter
        ├── NPC/              # NPCAgent, NPCStateMachine, NPCSpawner
        ├── Economy/          # DayCycleManager, EconomyManager, RandomEventManager
        └── UI/               # POSMenuUI, ReviewAppUI, EndOfDaySummaryUI
```

---

## 3. KỊCH BẢN GAMEPLAY & VÒNG LẶP CHÍNH

### 3.1. Khởi đầu & Mở rộng
- **Chọn mức vốn tự cấp**:
  - `30.000.000 VNĐ` (Khó — điểm thưởng cuối game cao nhất).
  - `100.000.000 VNĐ` (Vừa).
  - `300.000.000 VNĐ` (Dễ).
- **Chọn mô hình**:
  - **Xe nước vỉa hè**: Vốn thấp, di động, phụ thuộc thời tiết (mưa giảm khách), menu giới hạn, rủi ro bị đuổi chỗ bán. Có thể nâng cấp lên Quán nhỏ.
  - **Quán nhỏ**: Thuê mặt bằng, có bàn ghế/wifi/ổ điện, chi phí cố định cao, mở khóa nâng cấp mở rộng chi nhánh.
- **Chọn chủ đề (`ShopTheme`)**: Cafe truyền thống Việt Nam (phin, bạc xỉu), Specialty coffee, Trà sữa, Matcha/Nhật, Quán sách, Quán sinh viên học bài, Quán acoustic. Chủ đề quyết định menu, nội thất, nhạc nền và tỷ lệ xuất hiện từng mẫu NPC.

### 3.2. Chu kỳ một ngày (4 Pha)
1. **Sáng (Chuẩn bị)**: Nhập nguyên liệu (chú ý hạn sử dụng `shelfLifeDays`), kiểm tra kho, bật máy, dọn dẹp vệ sinh (`shopHygieneScore01`), chọn playlist nhạc (`shopAmbianceMatch01`).
2. **Mở cửa (Pha chế FPS)**: Khách vào quán -> đứng quầy nhận order -> thao tác góc nhìn thứ nhất theo bước (`PlaceCup`, `PourLiquid`, `AddSolidOrPowder`, `BrewMachineOrPhin`, `StirOrShake`, `Serve`).
3. **Giờ cao điểm (Xử lý sự cố)**: Quản lý hàng đợi, xử lý sự cố (hết nguyên liệu, hỏng máy, khách phàn nàn) và **Sự kiện ngẫu nhiên** (mất điện, hàng xóm phàn nàn tiếng ồn, influencer ghé quán, khách say, trộm vặt, sinh viên ngồi cả ngày, đoàn khách đông bất ngờ).
4. **Tối (Tổng kết)**: Tính doanh thu trừ giá vốn (COGS) & chi phí cố định, đọc bài đăng trên **App Đánh Giá**, nâng cấp thiết bị/nội thất, thuê nhân viên (từ Chương 2).

### 3.3. Tiến trình 4 Chương (1 Năm)
- **Chương 1 – Những ngày đầu**: Ít khách, làm quen thao tác pha chế. Mục tiêu hòa vốn tháng đầu.
- **Chương 2 – Có tiếng**: Khách quen xuất hiện, review lan truyền, mở khóa thuê nhân viên.
- **Chương 3 – Thử thách**: Đối thủ mở quán đối diện, giá nguyên liệu tăng, mùa mưa, thanh tra vệ sinh.
- **Chương 4 – Bước ngoặt**: Chọn mở chuỗi chi nhánh, nhượng quyền, giữ mô hình boutique nhỏ mà chất, hoặc bán lại quán -> Dẫn tới các cái kết khác nhau.

---

## 4. HỆ THỐNG CHẤM ĐIỂM ĐỘ HÀI LÒNG NPC (TOÁN HỌC)

Mỗi lần phục vụ xong một ly nước, hệ thống tính điểm $S_{\text{total}} \in [0, 100]$ dựa trên 6 tiêu chí chuẩn hóa $S_i \in [0, 1]$:

1. **Tốc độ ($S_{\text{speed}}$)**:
   - Nếu thời gian chờ thực tế $t \le T_{\text{ideal}}$: $S_{\text{speed}} = 1$.
   - Nếu $t \ge T_{\text{max}}$: $S_{\text{speed}} = 0$.
   - Khoảng giữa: $S_{\text{speed}} = 1 - \left(\frac{t - T_{\text{ideal}}}{T_{\text{max}} - T_{\text{ideal}}}\right)^k$ (với $k = \text{waitDecayExponent}$).
2. **Chất lượng đồ uống ($S_{\text{quality}}$)**:
   - $S_{\text{quality}} = 0.70 \cdot E_{\text{craft}} + 0.30 \cdot M_{\text{taste}}$
   - $E_{\text{craft}}$: Điểm chính xác từng bước pha chế (sai số trong `allowedTolerance` đạt $1.0$, vượt `maxErrorThreshold` đạt $0.0$, sai thứ tự khi `requireStrictOrder = true` bị nhân $0.7$).
   - $M_{\text{taste}}$: Độ khớp 5 chiều hương vị (`sweetness`, `bitterness`, `creaminess`, `aroma`, `novelty`) qua khoảng cách Euclid chuẩn hóa theo `tasteTolerance`.
3. **Giá cả ($S_{\text{price}}$)**:
   - Nếu giá bán $P \le P_{\text{ref}}$ (`expectedBudgetVND`): $S_{\text{price}} = 1$.
   - Nếu $P > P_{\text{ref}}$: $S_{\text{price}} = \exp\left(-\beta \cdot \frac{P - P_{\text{ref}}}{P_{\text{ref}}}\right)$.
4. **Thái độ ($S_{\text{attitude}}$), Vệ sinh ($S_{\text{hygiene}}$), Không gian ($S_{\text{ambiance}}$)**: Lấy trực tiếp từ trạng thái quán trong đoạn $[0, 1]$.
5. **Tổng hợp theo trọng số ($w_i$) và Độ khó tính ($\alpha$)**:
   $$S_{\text{total}} = 100 \cdot \mu_{\text{crit}} \cdot \frac{\sum_{i=1}^{6} w_i \cdot (S_i)^{\alpha}}{\sum_{i=1}^{6} w_i}$$
   *(Trong đó $\mu_{\text{crit}} = 0.45$ nếu pha hỏng món $S_{\text{quality}} < 0.2$ hoặc khách hết sạch kiên nhẫn $S_{\text{speed}} \le 0.01$; ngược lại $\mu_{\text{crit}} = 1.0$).*
6. **Quy đổi đầu ra**:
   - **Số sao**: $\text{Stars} = \text{Clamp}(\lceil S_{\text{total}} / 20 \rceil, 1, 5)$.
   - **Quay lại**: `willReturn = (S_total >= satisfactionThreshold)`.
   - **Thay đổi danh tiếng**: $\Delta R = \frac{S_{\text{total}} - \theta_{\text{sat}}}{10} \cdot M_{\text{rep}}$.

---

## 5. BẢNG THÔNG SỐ CÂN BẰNG CHO 8 MẪU NPC (`NPCPersonalitySO`)

### 5.1. Trọng số 6 tiêu chí ưu tiên (`EvaluationWeights`)
| Mẫu NPC (`NPCArchetype`) | `speedWeight` | `qualityWeight` | `attitudeWeight` | `hygieneWeight` | `ambianceWeight` | `priceWeight` |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: |
| `StudentNeedWifi` | 0.05 | 0.15 | 0.10 | 0.15 | **0.30** | **0.25** |
| `BusyOfficeWorker` | **0.45** | **0.25** | 0.10 | 0.05 | 0.05 | 0.10 |
| `StrictAuntie` | 0.10 | 0.20 | **0.25** | **0.30** | 0.05 | 0.10 |
| `CoffeeCritic` | 0.05 | **0.50** | 0.10 | **0.20** | 0.10 | 0.05 |
| `DatingCouple` | 0.10 | 0.20 | 0.20 | 0.10 | **0.35** | 0.05 |
| `BloggerTikToker` | 0.10 | **0.30** | 0.15 | 0.10 | **0.30** | 0.05 |
| `FriendlyRegular` | 0.10 | **0.25** | **0.30** | 0.10 | 0.15 | 0.10 |
| `Troublemaker` | **0.30** | 0.15 | **0.35** | 0.10 | 0.05 | 0.05 |

### 5.2. Kiên nhẫn, Độ khó tính & Ngân sách
| Mẫu NPC | `idealWait` | `maxWait` | `waitDecay (k)` | `strictness (alpha)` | `satThreshold` | `expectedBudgetVND` | `priceBeta` |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :---: |
| `StudentNeedWifi` | 45s | 180s | 1.1 | 0.85 | 55 | 25.000 | 2.0 |
| `BusyOfficeWorker` | 20s | 60s | 2.2 | 1.10 | 60 | 40.000 | 0.9 |
| `StrictAuntie` | 25s | 75s | 1.8 | 1.75 | 72 | 30.000 | 1.6 |
| `CoffeeCritic` | 35s | 110s | 1.3 | 2.00 | 80 | 65.000 | 0.3 |
| `DatingCouple` | 40s | 150s | 1.2 | 1.00 | 62 | 55.000 | 0.6 |
| `BloggerTikToker` | 30s | 100s | 1.5 | 1.30 | 68 | 50.000 | 0.5 |
| `FriendlyRegular` | 40s | 160s | 1.0 | 0.75 | 50 | 35.000 | 1.0 |
| `Troublemaker` | 15s | 50s | 2.5 | 1.90 | 78 | 30.000 | 1.8 |

### 5.3. Gu vị (`FlavorProfile`) & Hành vi xã hội (`Social & Narrative`)
| Mẫu NPC | Sweet | Bitter | Cream | Aroma | Novelty | `tasteTol` | `socialType` | `baseReviewProb` | `repMult` | `visitsQuest` |
| :--- | :---: | :---: | :---: | :---: | :---: | :---: | :--- | :---: | :---: | :---: |
| `StudentNeedWifi` | 0.75 | 0.30 | 0.70 | 0.50 | 0.50 | 0.45 | `WordOfMouthSpreader` | 0.30 | 1.0x | 5 |
| `BusyOfficeWorker` | 0.35 | 0.80 | 0.40 | 0.70 | 0.20 | 0.40 | `SilentLeaver` | 0.15 | 0.8x | 6 |
| `StrictAuntie` | 0.50 | 0.65 | 0.50 | 0.60 | 0.10 | 0.25 | `WordOfMouthSpreader` | 0.50 | 1.8x | 8 |
| `CoffeeCritic` | 0.20 | 0.85 | 0.30 | 0.95 | 0.60 | 0.15 | `OnlineReviewer` | 0.95 | 4.0x | 3 |
| `DatingCouple` | 0.80 | 0.25 | 0.75 | 0.75 | 0.65 | 0.35 | `OnlineReviewer` | 0.45 | 1.2x | 4 |
| `BloggerTikToker` | 0.65 | 0.40 | 0.65 | 0.80 | 0.90 | 0.30 | `OnlineReviewer` | 0.90 | 3.5x | 3 |
| `FriendlyRegular` | 0.60 | 0.55 | 0.60 | 0.65 | 0.30 | 0.50 | `WordOfMouthSpreader` | 0.40 | 1.5x | 4 |
| `Troublemaker` | 0.50 | 0.50 | 0.50 | 0.50 | 0.40 | 0.20 | `OnlineReviewer` | 0.80 | 2.2x | 0 |

---

## 6. MÃ NGUỒN CỐT LÕI ĐÃ HOÀN THIỆN (REFERENCE CODE)

Antigravity sử dụng trực tiếp hai file C# dưới đây làm nền tảng cho `Assets/_Project/Scripts/Core/`, không được tự ý đổi tên các kiểu dữ liệu công khai (`DrinkRecipeSO`, `NPCPersonalitySO`, `EvaluationOutput`, `ReviewPostData`).

### 6.1. `BrewAndDreamCoreData.cs` (Dữ liệu & Bộ máy chấm điểm)

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrewAndDream.Core
{
    public enum ShopTheme
    {
        VietnameseTraditionalCoffee,
        SpecialtyCoffee,
        MilkTea,
        MatchaJapanese,
        BookCafe,
        StudentStudyCafe,
        AcousticCafe
    }

    public enum NPCArchetype
    {
        StudentNeedWifi,
        BusyOfficeWorker,
        StrictAuntie,
        CoffeeCritic,
        DatingCouple,
        BloggerTikToker,
        FriendlyRegular,
        Troublemaker
    }

    public enum SocialBehaviorType
    {
        OnlineReviewer,
        WordOfMouthSpreader,
        SilentLeaver
    }

    public enum CraftingActionType
    {
        PlaceCup,
        PourLiquid,
        AddSolidOrPowder,
        BrewMachineOrPhin,
        StirOrShake,
        Serve
    }

    [Serializable]
    public struct FlavorProfile
    {
        [Range(0f, 1f)] public float sweetness;
        [Range(0f, 1f)] public float bitterness;
        [Range(0f, 1f)] public float creaminess;
        [Range(0f, 1f)] public float aroma;
        [Range(0f, 1f)] public float novelty;
    }

    [Serializable]
    public struct EvaluationWeights
    {
        [Min(0f)] public float speedWeight;
        [Min(0f)] public float qualityWeight;
        [Min(0f)] public float attitudeWeight;
        [Min(0f)] public float hygieneWeight;
        [Min(0f)] public float ambianceWeight;
        [Min(0f)] public float priceWeight;

        public float TotalWeight =>
            Mathf.Max(0.0001f, speedWeight + qualityWeight + attitudeWeight + hygieneWeight + ambianceWeight + priceWeight);
    }

    [CreateAssetMenu(fileName = "NewIngredient", menuName = "BrewAndDream/Data/Ingredient Data")]
    public class IngredientDataSO : ScriptableObject
    {
        public string ingredientId;
        public string displayName;
        public Sprite icon;
        public Color liquidOrVisualColor = Color.white;
        public float unitCostVND;
        public int shelfLifeDays = 3;
    }

    [Serializable]
    public struct RecipeStep
    {
        public string stepDescription;
        public CraftingActionType actionType;
        public IngredientDataSO requiredIngredient;
        public float targetAmount;
        public float allowedTolerance;
        public float maxErrorThreshold;
        [Range(0.1f, 5f)] public float stepImportance;
        public bool requireStrictOrder;
    }

    [CreateAssetMenu(fileName = "NewDrinkRecipe", menuName = "BrewAndDream/Data/Drink Recipe")]
    public class DrinkRecipeSO : ScriptableObject
    {
        public string recipeId;
        public string drinkName;
        [TextArea(2, 4)] public string description;
        public Sprite menuIcon;
        public List<ShopTheme> suitableThemes = new List<ShopTheme>();
        public int referenceMarketPriceVND = 35000;
        public bool unlockedFromStart = true;
        public FlavorProfile idealFlavorProfile;
        public List<RecipeStep> craftingSteps = new List<RecipeStep>();

        public float CalculateBaseIngredientCost()
        {
            float totalCost = 0f;
            foreach (var step in craftingSteps)
            {
                if (step.requiredIngredient != null)
                    totalCost += step.requiredIngredient.unitCostVND * step.targetAmount;
            }
            return totalCost;
        }
    }

    [CreateAssetMenu(fileName = "NewNPCPersonality", menuName = "BrewAndDream/Data/NPC Personality")]
    public class NPCPersonalitySO : ScriptableObject
    {
        public string archetypeId;
        public string archetypeName;
        public NPCArchetype archetypeType;
        [TextArea(2, 4)] public string personalityDescription;
        public float idealWaitTimeSeconds = 30f;
        public float maxWaitTimeSeconds = 120f;
        [Range(0.5f, 3f)] public float waitDecayExponent = 1.5f;
        [Range(0.5f, 2.5f)] public float strictnessAlpha = 1.0f;
        [Range(30f, 85f)] public float satisfactionThreshold = 60f;
        public FlavorProfile preferredFlavor;
        [Range(0.1f, 1f)] public float tasteTolerance = 0.35f;
        public List<DrinkRecipeSO> favoriteDrinks = new List<DrinkRecipeSO>();
        public float expectedBudgetVND = 35000f;
        [Range(0.2f, 3f)] public float priceSensitivityBeta = 1.2f;
        public EvaluationWeights weights;
        public SocialBehaviorType socialType = SocialBehaviorType.OnlineReviewer;
        [Range(0f, 1f)] public float baseReviewProbability = 0.35f;
        [Range(0.5f, 5f)] public float reputationImpactMultiplier = 1.0f;
        public int visitsToUnlockMiniQuest = 5;
        public string yarnDialogueNodeName;
    }

    [Serializable]
    public struct CraftedStepLog
    {
        public int stepIndex;
        public float actualAmount;
        public bool wasCorrectOrder;
    }

    [Serializable]
    public struct ServiceEvaluationContext
    {
        public DrinkRecipeSO orderedRecipe;
        public List<CraftedStepLog> playerStepLogs;
        public FlavorProfile actualFlavorProfile;
        public float actualWaitTimeSeconds;
        public float sellingPriceVND;
        public float playerAttitudeScore01;
        public float shopHygieneScore01;
        public float shopAmbianceMatch01;
    }

    [Serializable]
    public struct EvaluationOutput
    {
        public float finalSatisfactionScore;
        public int starRating;
        public bool willReturn;
        public bool willWriteReview;
        public float reputationDelta;
        public string dominantPraiseCriterion;
        public string dominantComplaintCriterion;
    }

    public static class NPCSatisfactionCalculator
    {
        public static float CalculateCraftingExecution01(DrinkRecipeSO recipe, List<CraftedStepLog> logs)
        {
            if (recipe == null || recipe.craftingSteps.Count == 0) return 1f;
            if (logs == null || logs.Count == 0) return 0f;

            float weightedScoreSum = 0f;
            float totalImportance = 0f;

            for (int i = 0; i < recipe.craftingSteps.Count; i++)
            {
                RecipeStep step = recipe.craftingSteps[i];
                totalImportance += step.stepImportance;

                int foundIndex = logs.FindIndex(l => l.stepIndex == i);
                if (foundIndex < 0) continue;

                CraftedStepLog log = logs[foundIndex];
                float absError = Mathf.Abs(log.actualAmount - step.targetAmount);

                float stepAccuracy;
                if (absError <= step.allowedTolerance) stepAccuracy = 1f;
                else if (absError >= step.maxErrorThreshold) stepAccuracy = 0f;
                else
                {
                    float range = Mathf.Max(0.0001f, step.maxErrorThreshold - step.allowedTolerance);
                    stepAccuracy = 1f - ((absError - step.allowedTolerance) / range);
                }

                if (step.requireStrictOrder && !log.wasCorrectOrder) stepAccuracy *= 0.7f;
                weightedScoreSum += stepAccuracy * step.stepImportance;
            }

            return Mathf.Clamp01(weightedScoreSum / Mathf.Max(0.0001f, totalImportance));
        }

        public static float CalculateTasteMatch01(FlavorProfile actual, FlavorProfile preferred, float tolerance)
        {
            float dSweet = actual.sweetness - preferred.sweetness;
            float dBitter = actual.bitterness - preferred.bitterness;
            float dCream = actual.creaminess - preferred.creaminess;
            float dAroma = actual.aroma - preferred.aroma;
            float dNovelty = actual.novelty - preferred.novelty;

            float distance = Mathf.Sqrt((dSweet * dSweet + dBitter * dBitter + dCream * dCream + dAroma * dAroma + dNovelty * dNovelty) / 5f);
            if (distance <= tolerance * 0.5f) return 1f;
            float normalizedDiff = (distance - tolerance * 0.5f) / Mathf.Max(0.05f, 1f - tolerance * 0.5f);
            return Mathf.Clamp01(1f - normalizedDiff);
        }

        public static EvaluationOutput EvaluateService(NPCPersonalitySO npc, ServiceEvaluationContext ctx)
        {
            float sSpeed;
            if (ctx.actualWaitTimeSeconds <= npc.idealWaitTimeSeconds) sSpeed = 1f;
            else if (ctx.actualWaitTimeSeconds >= npc.maxWaitTimeSeconds) sSpeed = 0f;
            else
            {
                float ratio = (ctx.actualWaitTimeSeconds - npc.idealWaitTimeSeconds) /
                              Mathf.Max(1f, npc.maxWaitTimeSeconds - npc.idealWaitTimeSeconds);
                sSpeed = Mathf.Clamp01(1f - Mathf.Pow(ratio, npc.waitDecayExponent));
            }

            float eCraft = CalculateCraftingExecution01(ctx.orderedRecipe, ctx.playerStepLogs);
            float mTaste = CalculateTasteMatch01(ctx.actualFlavorProfile, npc.preferredFlavor, npc.tasteTolerance);
            float sQuality = Mathf.Clamp01(0.70f * eCraft + 0.30f * mTaste);

            float sAttitude = Mathf.Clamp01(ctx.playerAttitudeScore01);
            float sHygiene = Mathf.Clamp01(ctx.shopHygieneScore01);
            float sAmbiance = Mathf.Clamp01(ctx.shopAmbianceMatch01);

            float priceRatio = (ctx.sellingPriceVND - npc.expectedBudgetVND) / Mathf.Max(1000f, npc.expectedBudgetVND);
            float sPrice = priceRatio <= 0f ? 1f : Mathf.Clamp01(Mathf.Exp(-npc.priceSensitivityBeta * priceRatio));

            EvaluationWeights w = npc.weights;
            float wSum = w.TotalWeight;

            float cSpeed = Mathf.Pow(sSpeed, npc.strictnessAlpha);
            float cQuality = Mathf.Pow(sQuality, npc.strictnessAlpha);
            float cAttitude = Mathf.Pow(sAttitude, npc.strictnessAlpha);
            float cHygiene = Mathf.Pow(sHygiene, npc.strictnessAlpha);
            float cAmbiance = Mathf.Pow(sAmbiance, npc.strictnessAlpha);
            float cPrice = Mathf.Pow(sPrice, npc.strictnessAlpha);

            float weightedRaw = (
                w.speedWeight * cSpeed + w.qualityWeight * cQuality +
                w.attitudeWeight * cAttitude + w.hygieneWeight * cHygiene +
                w.ambianceWeight * cAmbiance + w.priceWeight * cPrice
            ) / wSum;

            float criticalMultiplier = (sQuality < 0.2f || sSpeed <= 0.01f) ? 0.45f : 1.0f;
            float finalScore100 = Mathf.Clamp(weightedRaw * criticalMultiplier * 100f, 0f, 100f);
            int stars = Mathf.Clamp(Mathf.CeilToInt(finalScore100 / 20f), 1, 5);
            bool willReturn = finalScore100 >= npc.satisfactionThreshold;

            float extremityBoost = Mathf.Abs(finalScore100 - 50f) / 50f;
            float reviewProb = Mathf.Clamp01(npc.baseReviewProbability * (0.6f + 0.8f * extremityBoost));
            bool willReview = UnityEngine.Random.value <= reviewProb;
            float repDelta = ((finalScore100 - npc.satisfactionThreshold) / 10f) * npc.reputationImpactMultiplier;

            (string praise, string complaint) = FindDominantCriteria(cSpeed, cQuality, cAttitude, cHygiene, cAmbiance, cPrice);

            return new EvaluationOutput
            {
                finalSatisfactionScore = finalScore100,
                starRating = stars,
                willReturn = willReturn,
                willWriteReview = willReview,
                reputationDelta = repDelta,
                dominantPraiseCriterion = praise,
                dominantComplaintCriterion = complaint
            };
        }

        private static (string praise, string complaint) FindDominantCriteria(
            float speed, float quality, float attitude, float hygiene, float ambiance, float price)
        {
            (string name, float val)[] items =
            {
                ("Tốc độ phục vụ", speed), ("Chất lượng đồ uống", quality),
                ("Thái độ phục vụ", attitude), ("Vệ sinh quán", hygiene),
                ("Không gian & Âm nhạc", ambiance), ("Giá cả", price)
            };

            string best = items[0].name; float maxVal = items[0].val;
            string worst = items[0].name; float minVal = items[0].val;

            for (int i = 1; i < items.Length; i++)
            {
                if (items[i].val > maxVal) { maxVal = items[i].val; best = items[i].name; }
                if (items[i].val < minVal) { minVal = items[i].val; worst = items[i].name; }
            }
            return (best, worst);
        }
    }
}
```

### 6.2. `BrewAndDreamReviewGenerator.cs` (Hệ thống sinh Review Hybrid)
- Đã cài đặt `ReviewPostData`, `ReviewTemplateLibrarySO`, và `BrewAndDreamReviewGenerator` hỗ trợ `GenerateReviewAsync(...)` chạy bất đồng bộ (0ms với Template Offline theo 8 giọng điệu NPC, tự động fallback khi tắt/lỗi LLM API, có LRU-like Cache 200 mục).

---

## 7. CHỈ DẪN TRIỂN KHAI CHO ANTIGRAVITY (CODING ROADMAP)

Khi yêu cầu Antigravity code tiếp các module còn lại của dự án, hãy thực hiện tuần tự theo 5 bước sau:

1. **Bước 1 – Khởi tạo Data & Presets (Editor Tool)**:
   - Tạo script `Assets/_Project/Editor/NPCPresetGenerator.cs` để tự động sinh 8 file `.asset` của `NPCPersonalitySO` theo đúng Bảng Mục 5.
   - Tạo bộ nguyên liệu mẫu (`CaPhePhin`, `SuaDac`, `SuaTuoi`, `DaVien`) và công thức `BacXiu` (`DrinkRecipeSO`).
2. **Bước 2 – Hệ thống Pha chế FPS (`Assets/_Project/Scripts/Crafting/`)**:
   - Xây dựng `FPSPlayerController` (di chuyển WASD + nhìn chuột bằng Cinemachine).
   - Xây dựng `RaycastInteractor` + `CraftingStation`: Bắt điểm đặt ly (`PlaceCup`), giữ chuột trái để rót chất lỏng/đong bột tăng `actualAmount` theo thời gian thực, dùng `DOTween` diễn hoạt nhấc/nghiêng bình rót, và ghi lại `List<CraftedStepLog>` để truyền vào `NPCSatisfactionCalculator.EvaluateService`.
3. **Bước 3 – Hệ thống AI NPC (`Assets/_Project/Scripts/NPC/`)**:
   - Xây dựng `NPCStateMachine` gồm các trạng thái: `Entering` -> `Queuing` -> `Ordering` -> `WaitingForDrink` (đếm `actualWaitTimeSeconds`) -> `ConsumingAtTable` -> `EvaluatingAndLeaving`.
   - Khi rời đi, gọi `NPCSatisfactionCalculator.EvaluateService`. Nếu `willWriteReview == true`, gọi `BrewAndDreamReviewGenerator.GenerateReviewAsync` và đẩy kết quả vào danh sách review trong ngày.
4. **Bước 4 – Vòng lặp Ngày, Kinh tế & Sự kiện (`Assets/_Project/Scripts/Economy/`)**:
   - Xây dựng `DayCycleManager` quản lý 4 pha (`MorningPrep`, `OpenShop`, `RushHour`, `NightSummary`) và tiến trình 4 Chương (365 ngày).
   - Xây dựng `EconomyManager` tính doanh thu, giá vốn nguyên liệu, chi phí mặt bằng/nhân viên, và lưu lịch sử bằng `SQLite-net` + JSON.
5. **Bước 5 – Giao diện Người dùng (`Assets/_Project/Scripts/UI/`)**:
   - **POS / Menu UI**: Cho phép chỉnh giá bán (`sellingPriceVND`) để người chơi tự cân đối lợi nhuận và độ nhạy cảm giá của khách.
   - **App Đánh Giá UI**: Hiển thị danh sách `ReviewPostData` với số sao, tên khách, nội dung review và `socialLikesCount`.
   - **End of Day Summary UI**: Báo cáo doanh thu, chi phí, biến động danh tiếng và nút nâng cấp/thuê nhân viên.
