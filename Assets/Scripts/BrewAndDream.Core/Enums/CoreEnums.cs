namespace BrewAndDream.Core.Enums
{
    public enum ShopTheme
    {
        VietnameseTraditionalCoffee, // Cafe truyền thống Việt Nam (phin, bạc xỉu)
        SpecialtyCoffee,             // Specialty coffee
        MilkTea,                     // Trà sữa
        MatchaJapanese,              // Matcha / Nhật
        BookCafe,                    // Quán sách
        StudentStudyCafe,            // Quán cho sinh viên học bài
        AcousticCafe                 // Quán acoustic
    }

    public enum NPCArchetype
    {
        StudentNeedWifi,     // Sinh viên cần wifi và ổ điện
        BusyOfficeWorker,    // Dân văn phòng vội
        StrictAuntie,        // Bà cô khó tính
        CoffeeCritic,        // Critic cafe
        DatingCouple,        // Cặp đôi hẹn hò
        BloggerTikToker,     // Blogger / TikToker
        FriendlyRegular,     // Khách quen dễ thương
        Troublemaker         // Khách kiếm chuyện
    }

    public enum SocialBehaviorType
    {
        OnlineReviewer,      // Hay review trên app đánh giá trong game
        WordOfMouthSpreader, // Hay kể bạn bè (tăng lượng khách mới hôm sau)
        SilentLeaver         // Im lặng bỏ đi nếu không hài lòng
    }

    public enum CraftingActionType
    {
        PlaceCup,            // Đặt ly
        PourLiquid,          // Đong / đổ chất lỏng (ml)
        AddSolidOrPowder,    // Thêm bột / đá / topping (gram hoặc viên)
        BrewMachineOrPhin,   // Bấm máy chiết xuất hoặc ủ phin (giây)
        StirOrShake,         // Khuấy hoặc lắc đều (giây)
        Serve                // Đặt lên khay giao khách
    }

    public enum ShiftType
    {
        Morning,             // Sáng: 06:30 - 11:30 (Takeaway, cà phê phin vội)
        NoonRush,            // Trưa cao điểm: 11:30 - 14:00 (Cơm trưa, nước uống nhanh)
        AfternoonEvening     // Chiều - Tối: 14:00 - 22:00 (Học tập, Acoustic chill)
    }
}

