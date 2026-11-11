# BREW & DREAM — HƯỚNG DẪN RUN, BUILD & PHÁT TRIỂN DỰ ÁN (DEVELOPER MANUAL)

Dự án **Brew & Dream** là game mô phỏng khởi nghiệp quán nước góc nhìn thứ nhất (3D First-Person Beverage Shop Tycoon) kết hợp quản lý kinh tế và cốt truyện tương tác.

---

## 1. YÊU CẦU MÔI TRƯỜNG & PHẦN MỀM (SYSTEM REQUIREMENTS)

1. **Unity Engine**: **Unity 6 LTS** (hoặc Unity 2023.2+ trở lên).
2. **Render Pipeline**: **Universal Render Pipeline (URP)**.
3. **IDE / Code Editor**: Visual Studio 2022 / VS Code / JetBrains Rider.
4. **Unity Packages khuyến nghị**:
   * `Cinemachine` (Phiên bản 3.x)
   * `AI Navigation` (Unity com.unity.ai.navigation)
   * `Input System` (com.unity.inputsystem)
   * `DOTween (Demigiant)` (Cài từ Unity Asset Store để hỗ trợ diễn hoạt rót chất lỏng mượt mà)

---

## 2. CÁCH IMPORT & THIẾT LẬP PROJECT TRONG UNITY

### Bước 1: Mở hoặc Tạo mới Unity Project
1. Mở **Unity Hub** $\to$ Chọn **New Project**.
2. Chọn Template: **3D (URP)**.
3. Đặt tên: `BrewAndDream` (hoặc mở trực tiếp thư mục workspace hiện tại).

### Bước 2: Bố trí Thư mục Source Code
Toàn bộ mã nguồn C# đã được tổ chức phân tầng độc lập trong thư mục `src/`:
* Copy thư mục `src/` vào trong `Assets/_Project/Scripts/` của Unity Project.
* Cấu trúc tương ứng:
  ```text
  Assets/
  └── _Project/
      └── Scripts/
          ├── BrewAndDream.Core/           # Logic thuần C#, Models, Thuật toán chấm điểm
          ├── BrewAndDream.Simulation/     # FPS Controller, Quầy bar, AI NavMesh Khách hàng
          ├── BrewAndDream.UI/             # Canvas uGUI, POS, Kho, Sổ sách, Design System
          ├── BrewAndDream.Management/     # GameManager, ShiftManager, TrafficSpawner, Drama
          ├── BrewAndDream.Persistence/    # Quản lý lưu trữ file savegame.json và lịch sử sổ sách
          ├── BrewAndDream.Narrative/      # Mini-quest khách quen và hội thoại
          ├── BrewAndDream.Audio/          # Quản lý SFX pha chế và BGM theo 3 ca
          └── BrewAndDream.EditorTools/    # Công cụ Auto Scene Builder 1-Click
  ```

---

## 3. CÁCH RUN DỰ ÁN (CHẠY TRONG UNITY EDITOR)

Nhờ có công cụ tự động hóa **AutoSceneBuilder** đã được tích hợp sẵn trong `src/BrewAndDream.EditorTools/AutoSceneBuilder.cs`, bạn có thể tạo ngay Scene chơi hoàn chỉnh chỉ với 1 click:

1. Trên thanh Menu trên cùng của Unity Editor, bấm chọn:
   $$\text{\textbf{Tools}} \longrightarrow \text{\textbf{Brew \& Dream}} \longrightarrow \text{\textbf{Setup Default Coffee Shop Scene}}$$
2. Unity Editor sẽ ngay lập tức tự động sinh ra đầy đủ cấu trúc phân cấp (Hierarchy):
   * `[GameManagers]`: Chứa `GameManager`, `ShiftManager`, `CustomerTrafficSpawner`, `EventDramaManager`, `DataPersistenceManager`, `CoffeeShopAudioManager`.
   * `[Barista_Counter_Station]`: Quầy pha chế gồm Trạm rót sữa đặc, Máy pha Espresso, Thùng đá, Trạm khuấy, Bồn rửa bẩn và Chồng ly mới.
   * `[FPS_Player]`: Camera góc nhìn thứ nhất với bộ điều khiển tương tác `PlayerInteractionController` và vị trí cầm vật dụng `HandHoldPosition`.
   * `[UI_HUD_Canvas]`: Tâm ngắm Crosshair và chỉ dẫn phím bấm thời gian thực.
3. Bấm nút **Play (▶️)** trên Unity Editor để bắt đầu trải nghiệm ngay!

---

## 4. HƯỚNG DẪN ĐIỀU KHIỂN TRONG GAME (GAMEPLAY CONTROLS)

| Thao tác | Phím bấm / Chuột | Tác dụng |
| :--- | :---: | :--- |
| **Quét nhìn quầy bar** | Di chuyển chuột | Điều khiển góc nhìn FPS người pha chế. |
| **Tương tác chính** | **Chuột trái** hoặc phím **E** | Lấy ly mới, đặt ly vào trạm, bấm máy pha, rót sữa, giao nước cho khách. |
| **Tương tác phụ** | **Chuột phải** hoặc phím **Q** | Nhặt ly ra khỏi trạm pha, đặt ly xuống bàn, huỷ tương tác. |
| **Đong rót sữa / Siro** | **Nhấn giữ Chuột trái** | Giữ chuột để rót chính xác số ml mục tiêu, thả chuột để ngắt dòng chảy. |
| **Hủy ly pha lỗi** | Cầm ly $\to$ Bấm vào Bồn rửa | Đổ bỏ ly nước hỏng để làm lại ly mới. |

---

## 5. CÁCH BUILD GAME RA BẢN STANDALONE (.EXE CHO WINDOWS)

1. Mở Unity Editor $\to$ Vào **File** $\to$ **Build Profiles** (hoặc **Build Settings** `Ctrl + Shift + B`).
2. Nhấn nút **Add Open Scenes** để đưa Scene chính vào danh sách build.
3. Chọn Platform: **Windows, Mac, Linux** $\to$ Target Platform: **Windows (x86_64)**.
4. Cấu hình đồ họa:
   * Color Space: **Linear**.
   * Scripting Backend: **IL2CPP** (hoặc Mono để build nhanh).
5. Nhấn **Build** $\to$ Chọn thư mục xuất ra (ví dụ: `Builds/BrewAndDream_v1.0/`).
6. Sau khi Unity hoàn tất, mở file `BrewAndDream.exe` để thưởng thức game!

---

## 6. KIỂM THỬ ĐỘC LẬP BẰNG SIMULATION RUNNER (TEST LOGIC KHÔNG CẦN UNITY)

Nếu bạn muốn chạy thử nghiệm nhanh thuật toán tính toán chấm điểm sai số, chi phí $COGS$ và xác suất hỏng hóc máy móc:
* Mã nguồn kiểm thử đã có sẵn tại [Program.cs](file:///mnt/d/Brew%20and%20dream/src/BrewAndDream.Test/Program.cs).
* Bạn có thể mở trực tiếp file này trong Visual Studio / VS Code và nhấn **Run** (Console Application) để theo dõi kết quả in ra màn hình terminal.

