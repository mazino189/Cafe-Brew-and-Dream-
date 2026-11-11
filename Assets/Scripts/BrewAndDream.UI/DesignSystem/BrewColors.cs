using System;
using UnityEngine;

namespace BrewAndDream.UI.DesignSystem
{
    /// <summary>
    /// Bảng mã màu chuẩn trích xuất từ stitch_brew_dream_design_system / DESIGN.md
    /// Tone màu quán cafe ấm cúng: Espresso nâu đậm, Bọt sữa, Mật ong và Đỏ cảnh báo.
    /// </summary>
    public static class BrewColors
    {
        // Tone chính (Primary & Background)
        public static readonly Color Surface = new Color32(0xFF, 0xF8, 0xF2, 0xFF);          // #FFF8F2 Nền be kem ấm
        public static readonly Color EspressoDark = new Color32(0x2C, 0x18, 0x10, 0xFF);      // #2C1810 Nâu espresso đậm
        public static readonly Color Primary = new Color32(0x50, 0x2C, 0x12, 0xFF);           // #502C12 Nâu hạt rang
        public static readonly Color PrimaryContainer = new Color32(0x6B, 0x42, 0x26, 0xFF);  // #6B4226 Gỗ ấm
        
        // Điểm nhấn & Trạng thái (Accents & Feedback)
        public static readonly Color AmberHoney = new Color32(0xE5, 0xA9, 0x3C, 0xFF);        // #E5A93C Vàng mật ong
        public static readonly Color SecondaryGreen = new Color32(0x39, 0x69, 0x3B, 0xFF);    // #39693B Xanh lá mạ tươi
        public static readonly Color DangerRed = new Color32(0xC8, 0x4B, 0x31, 0xFF);         // #C84B31 Đỏ cảnh báo lỗi
        public static readonly Color MilkFoam = new Color32(0xFA, 0xF6, 0xF0, 0xFF);          // #FAF6F0 Trắng bọt sữa
        
        // Đường viền & Card panel
        public static readonly Color Outline = new Color32(0x83, 0x74, 0x6C, 0xFF);           // #83746C Viền xám ấm
        public static readonly Color ContainerLow = new Color32(0xFC, 0xF2, 0xE4, 0xFF);      // #FCF2E4 Thẻ nội dung nhạt
    }
}

