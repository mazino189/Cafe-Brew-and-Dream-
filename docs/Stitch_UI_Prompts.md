# Brew & Dream — Bộ Prompt Tiếng Anh Cho Google Stitch (UI Design)

Bộ prompt này được thiết kế bám sát **GDD v0.1** của **Brew & Dream**. Khi sử dụng [Google Stitch](https://stitch.withgoogle.com/), hãy luôn dán **Global Style Prefix** ở đầu mỗi prompt (hoặc tạo màn hình đầu tiên rồi dùng tính năng *Generate Variations / Reference* để giữ đồng bộ phong cách qua Figma trước khi đưa vào Unity UI Toolkit/uGUI).

---

## 0. Global Style Prefix (Dán ở đầu mọi Prompt)

```text
Game UI design for "Brew & Dream", a cozy Vietnamese coffee shop & beverage startup simulation game. Landscape 16:9 desktop viewport (1920x1080). Warm cozy palette: cream background (#F5EBDD), roasted coffee brown (#6B4226), leaf green (#5B8C5A), terracotta accent (#D9824B). Rounded friendly sans-serif typography, soft warm wood-grain and textured paper card panels, hand-drawn line icons, subtle soft shadows, clean game-ready HUD hierarchy. All visible UI labels, buttons, and sample text must be in Vietnamese.
```

---

## 1. Màn hình Chính (Main Menu)

```text
[Global Style Prefix]
Main menu screen over a stylized, warm-lit illustration of a cozy Vietnamese street-side coffee shop storefront at dusk with glowing lanterns, wooden counter, and potted plants. Top-left or upper-center stylized game logo reading "Brew & Dream". Clean vertical stack of rounded wooden/paper buttons on the left: "Chơi mới" (primary terracotta accent), "Tiếp tục", "Cài đặt", and "Thoát". Small settings gear icon and "v0.1" version badge at the bottom-right corner.
```

---

## 2. Chọn Vốn & Mô Hình Khởi Nghiệp (Capital & Model Setup)

```text
[Global Style Prefix]
Game setup screen titled "Khởi nghiệp quán nước". Top section features three horizontal selection cards for starting capital: "30 triệu – Khó" (with a "x2.0 Điểm thưởng" badge), "100 triệu – Vừa" ("x1.5 Điểm thưởng" badge, selected state with green border), and "300 triệu – Dễ" ("x1.0 Điểm thưởng" badge), each showing a short Vietnamese description and coin stack icon. Bottom section features two large comparison cards: "Xe nước" (pros: vốn thấp, di động; cons: phụ thuộc thời tiết, dễ bị đuổi chỗ) and "Quán nhỏ" (pros: có chỗ ngồi, doanh thu cao; cons: chi phí cố định cao). Bottom-right primary button "Tiếp theo".
```

---

## 3. Chọn Chủ Đề Quán (Shop Theme Selection)

```text
[Global Style Prefix]
Shop theme selection screen titled "Chọn chủ đề quán". Left two-thirds features a clean 7-card grid: "Cafe truyền thống", "Specialty Coffee", "Trà sữa", "Matcha Nhật", "Quán sách", "Quán học bài", and "Quán Acoustic". Each card displays a hand-drawn drink/interior icon, a 3-dot color swatch, and 2 small pill tags (menu type & target customer). Right one-third displays a detailed preview panel showing an illustrated interior mood of the selected theme, unlocked signature drinks, and preferred NPC types. Bottom-right primary button "Bắt đầu kinh doanh".
```

---

## 4. Quầy Order & Thu Ngân Trong Game (In-Game POS Overlay)

```text
[Global Style Prefix]
Semi-transparent in-game POS and order management overlay on top of a blurred 3D cafe counter background. Left column: scrollable list of active customer order tickets showing NPC portrait (e.g., Minh Khoa, Chị Hạnh), drink name, size, topping notes, and a color-coded patience timer bar transitioning from leaf green to terracotta red. Center panel: categorized grid of Vietnamese drink buttons ("Cà phê sữa đá", "Bạc xỉu", "Trà đào", "Matcha Latte") with hand-drawn icons and prices. Right column: receipt summary showing itemized total, cash/QR toggle, and a large green "Thanh toán" button.
```

---

## 5. Quản Lý Kho & Nhập Hàng (Inventory & Supply)

```text
[Global Style Prefix]
Inventory and morning supply screen titled "Kho & Nhập nguyên liệu". Top header bar pins current cash balance ("12.450.000 đ") and day counter ("Ngày 14 – Chương 1"). Below the header is a terracotta alert banner warning "Sắp hết hàng: Sữa đặc, Đá viên". Main content is a clean paper-styled table listing ingredients (Cà phê hạt, Sữa đặc, Sữa tươi, Đường, Trà đen, Trân châu, Ly nhựa) with stock level progress bars, unit price, shelf-life indicator ("Hạn: 2 ngày"), "-" / "+" quantity steppers, and an "Nhập thêm" action button on each row.
```

---

## 6. App Đánh Giá Khách Hàng (In-Game Review Phone App)

```text
[Global Style Prefix]
An in-game smartphone UI mockup centered on a warm desk background, displaying a local food & drink review app titled "Quán Quen Review". Top summary card shows overall shop rating ("4.6 / 5.0" stars) and neighborhood reputation progress bar. Below are three filter tabs: "Tất cả", "5 sao", and "Thấp". Scrollable feed of review cards featuring stylized NPC avatars, names (e.g., "Anh Duy – Critic", "Cô Tư", "Mai Anh"), 1-to-5 star ratings, short Vietnamese comments, and colored sentiment tags ("Nhanh", "Sạch", "Ngon", "Ồn").
```

---

## 7. Tổng Kết Cuối Ngày (End-of-Day Summary)

```text
[Global Style Prefix]
End-of-day financial and reputation report screen titled "Tổng kết Ngày 14". Top row displays 3 rounded KPI cards: "Doanh thu" (green), "Chi phí" (terracotta), and "Lợi nhuận ròng" (coffee brown). Middle-left features a clean vertical bar chart of hourly customer traffic (07:00 to 22:00). Middle-right shows "Top 3 Đánh giá nổi bật" compact cards. Bottom section displays the neighborhood reputation progress bar toward the next chapter milestone, alongside two action buttons on the bottom-right: "Nâng cấp quán" and "Ngày tiếp theo".
```

---

## 8. Nâng Cấp Quán & Thuê Nhân Viên (Upgrades & Staff)

```text
[Global Style Prefix]
Shop management and upgrade screen with a pinned top cash counter ("28.500.000 đ") and two large top tabs: "Thiết bị & Nội thất" and "Nhân viên". Under the active view, a responsive card grid shows equipment/furniture upgrades (e.g., "Máy pha Espresso", "Ổ điện & Wifi tốc độ cao", "Máy lạnh", "Góc check-in") with illustration, price tag, stat effect badge ("+20% Không gian"), and locked/purchased states; plus a preview design for staff candidate cards showing skill bars (Pha chế, Tốc độ, Thái độ), daily salary, personality tag, and a green "Thuê" button.
```

---

## 9. Hộp Thoại Sự Kiện & Xử Lý Tình Huống (Bonus: Random Events / Troublemaker Modal)

```text
[Global Style Prefix]
In-game narrative event modal card centered over a dimmed cafe scene. Header badge in terracotta reading "Sự kiện bất ngờ". Left side shows a large stylized 2D portrait of an angry or dramatic customer ("Hùng – Khách kiếm chuyện") or an event illustration (power outage / rainstorm). Right side presents the situation text in Vietnamese ("Ly này nhạt thếch, bớt tiền hoặc pha lại miễn phí đi!") and three distinct interactive choice buttons stacked vertically: "Nhượng bộ (Miễn phí - Mất tiền)", "Từ chối lịch sự (Giữ uy tín)", and "Mời ra ngoài (Rủi ro review xấu)", each with subtle impact hint icons.
```
