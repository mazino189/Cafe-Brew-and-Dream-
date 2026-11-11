# Brew & Dream – Tài liệu thiết kế game (GDD) v0.1

*Tên tạm · Bản nháp đầu tiên · 11/2026*

## 1. Tổng quan

**Pitch:** Game mô phỏng khởi nghiệp quán nước góc nhìn thứ nhất. Người chơi tự cấp vốn, chọn mô hình (xe nước hoặc quán), chọn chủ đề, tự đứng quầy pha chế và chịu trách nhiệm với đánh giá của những vị khách có tính cách riêng.

| Mục | Nội dung |
| --- | --- |
| Thể loại | Business sim / Cooking sim, góc nhìn thứ nhất |
| Nền tảng mục tiêu | PC (Steam, itch.io), mở rộng sau |
| Đối tượng | Người thích game quản lý, "cozy game", văn hóa cafe Việt |
| Thời lượng một lượt chơi | Khoảng 1 năm trong game (chia thành 4 chương) |
| Điểm khác biệt (USP) | NPC có tính cách độc lập, tự chấm điểm quán theo tiêu chí riêng; chọn vốn và mô hình khởi nghiệp; bản sắc cafe Việt |

**Trải nghiệm cốt lõi:** *"Tôi tự tay pha từng ly, và mỗi vị khách cảm nhận quán của tôi theo cách rất riêng."*

## 2. Kịch bản và cấu trúc game

**Bối cảnh:** Nhân vật chính vừa nghỉ việc văn phòng (hoặc vừa tốt nghiệp) và quyết định mở quán nước.

### 2.1 Màn mở đầu: chọn vốn, mô hình, chủ đề

- **Vốn tự cấp:** 30 triệu (khó) / 100 triệu (vừa) / 300 triệu (dễ). Vốn càng thấp, điểm thưởng cuối game càng cao.
- **Mô hình:** xe nước (vốn thấp, di động, phụ thuộc thời tiết, rủi ro bị đuổi chỗ bán) hoặc quán nhỏ (thuê mặt bằng, có chỗ ngồi, chi phí cố định cao).
- **Chủ đề:** cafe truyền thống Việt Nam, specialty coffee, trà sữa, matcha/Nhật, quán sách, quán học bài, quán acoustic.

### 2.2 Vòng lặp một ngày

1. **Sáng, chuẩn bị:** nhập nguyên liệu, kiểm tra kho, bật máy, dọn quán, chọn nhạc.
2. **Mở cửa:** khách vào, nhận order, pha chế theo thao tác góc nhìn thứ nhất.
3. **Giờ cao điểm:** xử lý hàng đợi, hết nguyên liệu, máy hỏng, khách phàn nàn.
4. **Tối, tổng kết:** doanh thu, chi phí, review khách, nâng cấp, thuê nhân viên.

### 2.3 Bốn chương

| Chương | Nội dung | Mục tiêu |
| --- | --- | --- |
| 1. Những ngày đầu | Ít khách, học pha chế, sai sót nhiều | Hòa vốn tháng đầu |
| 2. Có tiếng | Khách quen xuất hiện, review lan truyền, thuê nhân viên đầu tiên | Đạt điểm danh tiếng mốc 1 |
| 3. Thử thách | Đối thủ mở đối diện, giá nguyên liệu tăng, mùa mưa, thanh tra vệ sinh | Giữ lợi nhuận dương |
| 4. Bước ngoặt | Mở rộng chi nhánh, nhượng quyền, giữ quán nhỏ chất lượng, hoặc bán lại | Chọn hướng đi cuối game |

**Kết thúc đa dạng:** quán chuỗi, quán nghệ thuật, quán gia đình bền vững, thất bại rồi làm lại.

### 2.4 Sự kiện ngẫu nhiên

Mất điện, hàng xóm phàn nàn tiếng ồn, influencer ghé quán, đoàn khách đông bất ngờ, mưa lớn, bị kẹt hàng nhập, khách xin ngồi cả ngày, trộm vặt.

## 3. Kinh tế và mô hình quán

| Yếu tố | Xe nước | Quán nhỏ |
| --- | --- | --- |
| Vốn khởi điểm | Thấp | Cao (cọc, cải tạo, thiết bị) |
| Chi phí cố định | Thấp | Thuê mặt bằng, điện nước |
| Sức chứa | Mang đi | Có chỗ ngồi |
| Rủi ro chính | Thời tiết, bị đuổi chỗ | Vắng khách, chi phí cố định |
| Mở rộng | Lên thành quán | Thêm chi nhánh, nhượng quyền |

**Biến số kinh tế:** giá nguyên liệu dao động theo mùa, nhu cầu theo giờ/thời tiết/chủ đề, hao hụt nguyên liệu, lương nhân viên, thuế và phí, nâng cấp thiết bị.

## 4. Hệ thống NPC và chấm điểm

### 4.1 Thuộc tính tính cách (thang 1–5)

| Thuộc tính | Ý nghĩa |
| --- | --- |
| Kiên nhẫn | Chờ bao lâu thì bực |
| Khó tính | Ngưỡng chấm điểm khắt khe |
| Nhạy giá | Mức quan tâm đến giá |
| Lan truyền | Khả năng đăng review và kể cho người khác |
| Trọng số ưu tiên | Tốc độ, hương vị, thái độ, không gian, vệ sinh, giá (tổng 100%) |

### 4.2 Công thức chấm điểm (gợi ý)

`Hài lòng = Σ (trọng số_i × điểm_i) − phạt chờ + thưởng khách quen − (khó tính × độ lệch kỳ vọng)`

Kết quả quy ra 1–5 sao, kéo theo:

- Quay lại hoặc không, giới thiệu thêm khách.
- Để lại review trên app đánh giá trong game, ảnh hưởng danh tiếng khu vực.
- Khách quen mở khóa mini quest riêng.

**Ví dụ:** Khách ưu tiên tốc độ 50%, hương vị 30%, giá 20%. Nếu quán pha nhanh nhưng vị trung bình, khách vẫn chấm khá cao. Cùng ly đó với khách ưu tiên hương vị 60% thì chấm thấp.

### 4.3 Dữ liệu mẫu (JSON)

```json
{
  "id": "npc_minh_khoa",
  "name": "Minh Khoa",
  "patience": 4,
  "strictness": 2,
  "priceSensitivity": 5,
  "virality": 2,
  "weights": { "speed": 10, "taste": 20, "service": 10, "ambience": 20, "hygiene": 10, "price": 30 },
  "favoriteDrinks": ["bac_xiu", "tra_dao"],
  "visitHours": ["14:00-22:00"]
}
```

## 5. Character sheet NPC (10 mẫu đầu tiên)

### 5.1 Bảng chỉ số tổng hợp

| NPC | Kiên nhẫn | Khó tính | Nhạy giá | Lan truyền | Ưu tiên chính |
| --- | --- | --- | --- | --- | --- |
| Minh Khoa | 4 | 2 | 5 | 2 | Giá, không gian (wifi, ổ điện) |
| Chị Hạnh | 1 | 3 | 2 | 3 | Tốc độ |
| Cô Tư | 3 | 5 | 3 | 4 | Vệ sinh, thái độ |
| Anh Duy | 4 | 5 | 1 | 5 | Hương vị |
| Tuấn và Linh | 4 | 2 | 3 | 3 | Không gian, nhạc |
| Mai Anh | 3 | 3 | 2 | 5 | Không gian, ảnh đẹp |
| Chú Sáu | 5 | 1 | 4 | 3 | Thái độ, sự quen thuộc |
| Anh Tài | 2 | 2 | 4 | 2 | Tốc độ, giá |
| Thầy Phong | 5 | 3 | 3 | 2 | Yên tĩnh, hương vị |
| Hùng | 1 | 4 | 3 | 4 | Khó đoán (hay kiếm chuyện) |

### 5.2 Chi tiết từng nhân vật

**1. Minh Khoa – sinh viên IT**

- *Ngoại hình:* 20 tuổi, balo lớn, laptop dán sticker, áo hoodie, tai nghe trên cổ.
- *Tính cách:* hiền, ít nói, ngồi lâu, hay gọi một ly rồi ở cả buổi.
- *Gu:* bạc xỉu, trà đào; ghét quán ồn.
- *Hành vi:* tìm ổ cắm và wifi trước khi gọi món. Chiếm bàn lâu làm giảm sức chứa nhưng nếu hài lòng sẽ giới thiệu bạn bè sinh viên.
- *Thoại hài lòng:* "Quán này wifi mạnh ghê, mai mình rủ cả nhóm tới."
- *Thoại bực:* "Hết ổ cắm rồi hả anh? Pin mình còn 5% thôi..."

**2. Chị Hạnh – nhân viên văn phòng**

- *Ngoại hình:* 29 tuổi, váy công sở, giày cao gót, liên tục nhìn đồng hồ, cầm điện thoại.
- *Tính cách:* vội, thẳng tính, không thích chờ.
- *Gu:* cafe sữa đá mang đi, americano.
- *Hành vi:* xếp hàng giờ sáng; quá 3 phút là bắt đầu thở dài, quá 5 phút bỏ đi.
- *Thoại hài lòng:* "Nhanh và đúng vị, mai chị ghé nữa nhé."
- *Thoại bực:* "Em ơi chị trễ họp rồi, hủy giúp chị nhé."

**3. Cô Tư – khách lớn tuổi khó tính**

- *Ngoại hình:* 62 tuổi, áo bà ba hoặc áo dài cách tân, túi vải, mắt hay liếc ly và bàn.
- *Tính cách:* quan trọng vệ sinh và phép tắc, nói thẳng, nhưng đã tin thì rất trung thành.
- *Gu:* cafe phin truyền thống, trà nóng.
- *Hành vi:* kiểm tra ly, bàn, tay người pha. Thấy bẩn là chê to và kể cho cả xóm.
- *Thoại hài lòng:* "Cái quán này sạch sẽ, cậu chủ biết làm ăn."
- *Thoại bực:* "Cái ly này lau chưa vậy? Cô không uống đâu."

**4. Anh Duy – critic specialty coffee**

- *Ngoại hình:* 34 tuổi, râu gọn, tạp dề da, sổ tay nhỏ, hay ngửi ly trước khi uống.
- *Tính cách:* khắt khe, thích phân tích, không quan tâm giá.
- *Gu:* pour over, espresso single origin.
- *Hành vi:* review chi tiết, có thể kéo hoặc làm giảm danh tiếng với giới sành cafe.
- *Thoại hài lòng:* "Hậu vị sạch, độ chua cân bằng. Quán này có đầu tư thật."
- *Thoại bực:* "Nước quá nóng, cafe bị cháy rồi."

**5. Tuấn và Linh – cặp đôi hẹn hò**

- *Ngoại hình:* hai người 24–25 tuổi, ăn mặc chỉnh tề, chọn bàn góc.
- *Tính cách:* dễ chịu, quan tâm không khí và nhạc.
- *Gu:* trà trái cây, bánh ngọt, đồ uống đẹp để chụp ảnh.
- *Hành vi:* chê quán ồn hoặc nhạc không hợp; nếu hợp sẽ quay lại làm "quán kỷ niệm".
- *Thoại hài lòng:* "Ở đây yên và nhạc dễ chịu ghê."
- *Thoại bực:* "Anh ơi nhạc to quá, tụi em nói chuyện không nghe được."

**6. Mai Anh – TikToker / blogger ẩm thực**

- *Ngoại hình:* 23 tuổi, đầu tóc chỉn chu, giá đỡ điện thoại, ring light mini.
- *Tính cách:* hào hứng, để ý màu sắc và ảnh đẹp.
- *Gu:* đồ uống nhiều màu, topping lạ, chủ đề độc đáo.
- *Hành vi:* quay video trong quán. Review có thể kéo hàng chục khách mới, hoặc làm xấu danh tiếng nếu hỏng.
- *Thoại hài lòng:* "Góc này lên hình đẹp quá, mình đăng liền nhé!"
- *Thoại bực:* "Ly này nhìn không giống ảnh menu luôn..."

**7. Chú Sáu – khách quen hưu trí**

- *Ngoại hình:* 66 tuổi, áo sơ mi nhạt, mũ lưỡi trai, mang theo tờ báo.
- *Tính cách:* hiền, chuyện trò, trung thành.
- *Gu:* cafe đen đá, trà đá.
- *Hành vi:* đến cùng giờ mỗi sáng. Rất kiên nhẫn, nhưng nếu quán đổi vị quen thuộc sẽ nhận ra ngay. Mở khóa mini quest kể về khu phố.
- *Thoại hài lòng:* "Vẫn cái vị này, chú uống mấy chục năm rồi."
- *Thoại bực:* "Sao hôm nay con pha khác vậy? Chú thấy hơi lạ."

**8. Anh Tài – tài xế công nghệ**

- *Ngoại hình:* 31 tuổi, áo khoác xe công nghệ, mũ bảo hiểm, điện thoại gắn sẵn.
- *Tính cách:* thực tế, cần nghỉ nhanh, quan tâm giá và chỗ đậu xe.
- *Gu:* cafe sữa đá, nước ngọt.
- *Hành vi:* ghé giữa các cuốc, ngồi ngắn. Không kiên nhẫn nếu chờ lâu.
- *Thoại hài lòng:* "Giá hợp lý, ngồi mát. Cuốc sau anh ghé nữa."
- *Thoại bực:* "Chỗ đậu xe đâu rồi em ơi, anh bị phạt rồi."

**9. Thầy Phong – giáo viên, người viết sách**

- *Ngoại hình:* 45 tuổi, kính cận, cặp da, sách và sổ tay.
- *Tính cách:* điềm đạm, cần yên tĩnh để làm việc.
- *Gu:* trà ấm, cafe nhẹ, thích quán sách.
- *Hành vi:* ngồi lâu, chê quán ồn. Hợp nhất với chủ đề quán sách hoặc quán yên tĩnh; mở khóa đồ uống đặc biệt.
- *Thoại hài lòng:* "Không gian này làm việc rất tốt."
- *Thoại bực:* "Tiếng máy xay hơi ồn, tôi khó tập trung."

**10. Hùng – khách kiếm chuyện**

- *Ngoại hình:* 38 tuổi, mặt đỏ, nói to, hay chen hàng.
- *Tính cách:* nóng nảy, đòi hỏi vô lý, đôi khi gây rối.
- *Gu:* bất kỳ; thích giảm giá hoặc "thêm đá".
- *Hành vi:* cố tình phàn nàn để được miễn phí. Người chơi chọn: nhượng bộ, từ chối lịch sự, hoặc mời ra ngoài. Mỗi lựa chọn ảnh hưởng đến những khách khác đang chứng kiến.
- *Thoại hài lòng (hiếm):* "Ừ, chấp nhận được."
- *Thoại bực:* "Tôi uống ở đây mà bị đối xử vậy hả?"

## 6. Hướng nghệ thuật và thiết kế nhân vật

### 6.1 Phong cách

Đề xuất **stylized 3D / low-poly** với tỉ lệ hơi cường điệu (đầu to, nét mặt rõ) để tính cách NPC đọc được ngay qua ngoại hình và dáng đứng. Bảng màu ấm (nâu kem, xanh lá, cam đất), cảm giác "cozy", mang chất quán cafe Việt.

### 6.2 Nhân vật người chơi

Góc nhìn thứ nhất nên chỉ cần **đôi tay, tạp dề và phụ kiện**, đầu tư vào animation pha chế thay vì mô hình toàn thân.

### 6.3 Quy trình tạo NPC

1. Character sheet (mục 5) và concept 2D: góc trước, bên, sau, 3–4 biểu cảm.
2. Dựng mô hình 3D bằng Blender hoặc dùng công cụ dựng sẵn (VRoid, Ready Player Me).
3. Rig và animation: Mixamo hoặc Cascadeur.
4. **Hệ thống mô-đun:** thân, đầu, tóc, áo, phụ kiện ghép ngẫu nhiên để tạo nhiều khách từ ít bộ phận.

### 6.4 Định dạng file

| Loại | Định dạng |
| --- | --- |
| Nguồn 3D | .blend |
| Mô hình xuất | FBX hoặc glTF/GLB |
| Texture | PNG, 512–1024 px, dùng atlas |
| Concept 2D | PSD/KRA (nguồn), PNG (xuất) |
| Dữ liệu tính cách | JSON hoặc ScriptableObject/Resource |

## 7. Giao diện (UI) và prompt cho Google Stitch

### 7.1 Cách dùng Stitch trong dự án

Stitch tạo giao diện UI từ mô tả văn bản hoặc ảnh và cho xuất sang Figma hoặc mã HTML/CSS. Với game, nên dùng Stitch làm **bản mockup tham khảo**, sau đó chỉnh phong cách ở Figma và dựng lại bằng UI của engine (Unity UI Toolkit/uGUI hoặc Godot Control). Mã HTML từ Stitch không dùng trực tiếp trong engine. Hạn mức và tính năng của Stitch có thể thay đổi, nên kiểm tra trên trang chính thức.

**Quy trình:** viết phong cách chung → sinh từng màn hình → tạo biến thể → xuất Figma → chỉnh phong cách game (viền, icon vẽ tay) → xuất PNG/SVG → nhập vào engine.

### 7.2 Prompt phong cách chung (dùng làm tiền tố)

> Game UI for a cozy Vietnamese coffee shop management game. Landscape 16:9 desktop layout. Warm palette: cream (#F5EBDD), coffee brown (#6B4226), leaf green (#5B8C5A), terracotta accent (#D9824B). Rounded friendly sans-serif font, soft wood-texture panels, hand-drawn icons, subtle paper-like shadows. All on-screen text in Vietnamese.

### 7.3 Prompt cho từng màn hình

**Màn 1 – Menu chính**

> &#91;Tiền tố\] Main menu screen with game title "Brew & Dream" over an illustrated cafe storefront at dusk. Buttons: "Chơi mới", "Tiếp tục", "Cài đặt", "Thoát". Small settings icon and version label at the bottom.

**Màn 2 – Chọn vốn và mô hình**

> &#91;Tiền tố\] Setup screen with three capital option cards: "30 triệu – Khó", "100 triệu – Vừa", "300 triệu – Dễ", each with a short description and a score multiplier. Below, two large choice cards: "Xe nước" and "Quán nhỏ", with pros and cons lists. Button "Tiếp theo" at the bottom right.

**Màn 3 – Chọn chủ đề quán**

> &#91;Tiền tố\] Theme selection grid with 7 cards: Cafe truyền thống, Specialty, Trà sữa, Matcha Nhật, Quán sách, Quán học bài, Quán acoustic. Each card has an illustration, a color swatch and 2 tags (menu, kiểu khách). A preview panel on the right shows the selected theme's interior mood. Button "Bắt đầu kinh doanh".

**Màn 4 – Order và thu ngân (POS)**

> &#91;Tiền tố\] In-game POS overlay: a customer order ticket list on the left (drink name, size, topping, timer bar), a drink menu grid in the center, total price and "Thanh toán" button on the right. Timer bars change color from green to red as waiting time increases.

**Màn 5 – Kho và nhập hàng**

> &#91;Tiền tố\] Inventory and supply screen: ingredient table (cafe, sữa, đường, trà, trân châu, ly...) with stock bar, price, quantity stepper and "Nhập thêm" button. A top bar shows current cash. A warning banner for low stock items.

**Màn 6 – App đánh giá (Review)**

> &#91;Tiền tố\] In-game phone app showing customer reviews: avatar, name, star rating, short comment, tags (Nhanh, Sạch, Ngon, Ồn). Top summary card with average rating and reputation level. Filter tabs: "Tất cả", "5 sao", "Thấp".

**Màn 7 – Tổng kết cuối ngày**

> &#91;Tiền tố\] End-of-day summary screen: revenue, expenses, profit cards; a small bar chart of hourly customers; top 3 reviews of the day; reputation progress bar. Buttons "Nâng cấp quán" and "Ngày tiếp theo".

**Màn 8 – Nâng cấp và thuê nhân viên**

> &#91;Tiền tố\] Upgrade shop screen with two tabs: "Thiết bị & Nội thất" (item cards with price, effect, locked state) and "Nhân viên" (candidate cards with skill bars, salary, personality tag, "Thuê" button). A cash counter pinned to the top.

**Gợi ý:** sau khi có màn đầu ưng ý, dùng "tạo biến thể" và tham chiếu lại các màn đã tạo để các màn sau giữ cùng phong cách.

## 8. Công nghệ

| Hạng mục | Đề xuất |
| --- | --- |
| Engine | Unity (C#) hoặc Godot 4; Unreal nếu ưu tiên đồ họa cao |
| Điều khiển FPS và tương tác | Pick up, đặt, đổ, pha chế theo bước, animation tay |
| NPC AI | Behavior Tree hoặc State Machine (vào quán, xếp hàng, gọi món, ngồi, chờ, rời đi); NavMesh |
| Dữ liệu NPC và công thức | ScriptableObject/Resource, JSON |
| Kinh tế | Bảng nguyên liệu, công thức, giá, chi phí cố định, nhu cầu theo giờ và thời tiết |
| UI | UI Toolkit/uGUI (Unity) hoặc Control + Theme (Godot) |
| Lưu game | JSON hoặc SQLite |
| Âm thanh | Nhạc nền chọn được trong quán; FMOD/Wwise (tùy chọn) |
| Tùy chọn nâng cao | LLM sinh lời thoại và review (chỉ là lớp trang trí, logic chấm điểm vẫn là chỉ số xác định) |

## 9. Lộ trình

| Giai đoạn | Thời gian | Nội dung |
| --- | --- | --- |
| Prototype | 2–4 tuần | Một quầy, một đồ uống, 3 NPC, một vòng lặp ngày hoàn chỉnh |
| Vertical slice | 1–2 tháng | Chọn chủ đề, hệ thống đánh giá, kinh tế cơ bản, UI chính |
| Alpha | 2–3 tháng | Xe nước và quán, 10 NPC, sự kiện, nâng cấp, nhân viên |
| Beta | 1–2 tháng | Cân bằng độ khó, tối ưu, âm thanh, các cái kết |
| Phát hành | Sau Beta | Itch.io thử thị trường trước, sau đó Steam |

## 10. Rủi ro và câu hỏi mở

**Rủi ro**

- Pha chế FPS dễ lặp lại: cần đa dạng đồ uống, thao tác, và tự động hóa dần bằng máy hoặc nhân viên.
- NPC nhiều tính cách tốn công: bắt đầu với 5–6 mẫu rồi mở rộng.
- Phạm vi lớn: hoàn thiện một chủ đề (cafe Việt) trước khi thêm chủ đề khác.
- Giấy phép asset và nội dung do AI tạo: kiểm tra trước khi dùng thương mại.

**Câu hỏi mở cần quyết định**

- Engine chính thức: Unity hay Godot?
- Phong cách đồ họa chốt: low-poly hay stylized 3D?
- Làm solo hay theo nhóm, và quy mô ngân sách?
- Có dùng LLM cho lời thoại ở bản đầu hay không?
