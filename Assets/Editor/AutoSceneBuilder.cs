#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using BrewAndDream.Management;
using BrewAndDream.Simulation.Player;
using BrewAndDream.Simulation.BaristaStation;
using BrewAndDream.UI.HUD;
using BrewAndDream.Audio;
using BrewAndDream.Persistence;

namespace BrewAndDream.EditorTools
{
    /// <summary>
    /// Công cụ dựng tự động toàn bộ Main Cafe Scene cho Brew & Dream chỉ với 1 Click chuột!
    /// Menu: Tools -> Brew & Dream -> Setup Default Coffee Shop Scene
    /// </summary>
    public static class AutoSceneBuilder
    {
        private const string SceneFolderPath = "Assets/_Project/Scenes";
        private const string ScenePath = SceneFolderPath + "/MainCafeScene.unity";

        [MenuItem("Tools/Brew & Dream/Setup Default Coffee Shop Scene")]
        public static void BuildDefaultScene()
        {
            Debug.Log("[Brew & Dream] Bắt đầu tự động kiến tạo Scene quán cà phê...");

            // 0. Tạo scene mới hoặc mở scene
            if (!Directory.Exists(SceneFolderPath))
            {
                Directory.CreateDirectory(SceneFolderPath);
            }

            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // 1. Tạo Root GameManagers
            var managersRoot = new GameObject("[GameManagers]");
            managersRoot.AddComponent<GameManager>();
            managersRoot.AddComponent<ShiftManager>();
            var spawner = managersRoot.AddComponent<CustomerTrafficSpawner>();
            managersRoot.AddComponent<EventDramaManager>();
            managersRoot.AddComponent<DataPersistenceManager>();
            managersRoot.AddComponent<CoffeeShopAudioManager>();

            // 2. Tạo Môi trường quán (Floor & Lighting)
            var envRoot = new GameObject("[Environment]");
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Cafe_Floor";
            floor.transform.SetParent(envRoot.transform);
            floor.transform.position = Vector3.zero;
            floor.transform.localScale = new Vector3(3f, 1f, 3f); // 30m x 30m

            // 3. Tạo Barista Counter Station
            var counterRoot = new GameObject("[Barista_Counter_Station]");
            var counterTable = GameObject.CreatePrimitive(PrimitiveType.Cube);
            counterTable.name = "Counter_Table";
            counterTable.transform.SetParent(counterRoot.transform);
            counterTable.transform.position = new Vector3(0f, 0.45f, 2f);
            counterTable.transform.localScale = new Vector3(4f, 0.9f, 1f);

            // Trạm 1: Sữa đặc
            var milkStation = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            milkStation.name = "Station_CondensedMilk";
            milkStation.transform.SetParent(counterRoot.transform);
            milkStation.transform.position = new Vector3(-1.2f, 1.05f, 2f);
            milkStation.transform.localScale = new Vector3(0.2f, 0.25f, 0.2f);
            var milkComp = milkStation.AddComponent<LiquidDispenserStation>();
            milkComp.ingredientId = "ing_condensed_milk";
            milkComp.ingredientName = "Sữa đặc Ông Thọ";

            // Trạm 2: Đá viên
            var iceStation = GameObject.CreatePrimitive(PrimitiveType.Cube);
            iceStation.name = "Station_IceDispenser";
            iceStation.transform.SetParent(counterRoot.transform);
            iceStation.transform.position = new Vector3(-0.6f, 1.05f, 2f);
            iceStation.transform.localScale = new Vector3(0.35f, 0.3f, 0.35f);
            var iceComp = iceStation.AddComponent<IceAndSolidDispenser>();
            iceComp.ingredientId = "ing_ice";
            iceComp.ingredientName = "Đá viên sạch";

            // Trạm 3: Sữa tươi
            var freshMilkStation = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            freshMilkStation.name = "Station_FreshMilk";
            freshMilkStation.transform.SetParent(counterRoot.transform);
            freshMilkStation.transform.position = new Vector3(0.0f, 1.05f, 2f);
            freshMilkStation.transform.localScale = new Vector3(0.2f, 0.3f, 0.2f);
            var freshMilkComp = freshMilkStation.AddComponent<LiquidDispenserStation>();
            freshMilkComp.ingredientId = "ing_fresh_milk";
            freshMilkComp.ingredientName = "Sữa tươi thanh trùng";

            // Trạm 4: Cà phê phin
            var phinStation = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            phinStation.name = "Station_PhinCoffee";
            phinStation.transform.SetParent(counterRoot.transform);
            phinStation.transform.position = new Vector3(0.6f, 1.05f, 2f);
            phinStation.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            var phinComp = phinStation.AddComponent<LiquidDispenserStation>();
            phinComp.ingredientId = "ing_phin_coffee";
            phinComp.ingredientName = "Cà phê phin Robusta";

            // Trạm 5: Máy pha Espresso
            var espressoStation = GameObject.CreatePrimitive(PrimitiveType.Cube);
            espressoStation.name = "Station_EspressoMachine";
            espressoStation.transform.SetParent(counterRoot.transform);
            espressoStation.transform.position = new Vector3(1.3f, 1.15f, 2f);
            espressoStation.transform.localScale = new Vector3(0.5f, 0.5f, 0.4f);
            espressoStation.AddComponent<EspressoMachineStation>();

            // Trạm 6: Que khuấy / Shaker
            var stirStation = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stirStation.name = "Station_StirringShaker";
            stirStation.transform.SetParent(counterRoot.transform);
            stirStation.transform.position = new Vector3(1.8f, 1.0f, 2f);
            stirStation.transform.localScale = new Vector3(0.15f, 0.2f, 0.15f);
            stirStation.AddComponent<StirringStation>();

            // Trạm 7: Bồn rửa & thùng rác
            var sinkStation = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sinkStation.name = "Station_SinkAndTrash";
            sinkStation.transform.SetParent(counterRoot.transform);
            sinkStation.transform.position = new Vector3(-1.8f, 0.95f, 2f);
            sinkStation.transform.localScale = new Vector3(0.4f, 0.3f, 0.4f);
            sinkStation.AddComponent<TrashAndWashSink>();

            // Trạm 8: Chồng ly sạch
            var cupStack = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cupStack.name = "Station_CleanCupStack";
            cupStack.transform.SetParent(counterRoot.transform);
            cupStack.transform.position = new Vector3(-0.9f, 1.05f, 2.3f);
            cupStack.transform.localScale = new Vector3(0.18f, 0.35f, 0.18f);
            cupStack.AddComponent<CleanCupStack>();

            // 4. Tạo Player First-Person
            var playerRoot = new GameObject("[FPS_Player]");
            playerRoot.transform.position = new Vector3(0f, 1f, 0.5f);
            var charController = playerRoot.AddComponent<CharacterController>();
            charController.height = 1.8f;
            charController.radius = 0.4f;
            playerRoot.AddComponent<FPSPlayerController>();
            var interactor = playerRoot.AddComponent<PlayerInteractionController>();

            // Camera góc nhìn thứ nhất
            var existingCam = Camera.main;
            GameObject cameraObj;
            if (existingCam != null)
            {
                cameraObj = existingCam.gameObject;
                cameraObj.transform.SetParent(playerRoot.transform);
            }
            else
            {
                cameraObj = new GameObject("Main Camera");
                cameraObj.transform.SetParent(playerRoot.transform);
                cameraObj.AddComponent<Camera>();
                cameraObj.tag = "MainCamera";
            }
            cameraObj.transform.localPosition = new Vector3(0, 0.75f, 0);

            var handHold = new GameObject("HandHoldPosition");
            handHold.transform.SetParent(cameraObj.transform);
            handHold.transform.localPosition = new Vector3(0.35f, -0.25f, 0.65f);

            // 5. Tạo Điểm Di Chuyển NPC (Waypoints)
            var waypointsRoot = new GameObject("[NPC_Waypoints]");
            var orderPoint = new GameObject("Point_OrderQueue");
            orderPoint.transform.SetParent(waypointsRoot.transform);
            orderPoint.transform.position = new Vector3(0f, 0f, 2.8f);

            var tablePoint1 = new GameObject("Point_Table_01");
            tablePoint1.transform.SetParent(waypointsRoot.transform);
            tablePoint1.transform.position = new Vector3(-3f, 0f, 5f);

            var exitPoint = new GameObject("Point_ExitDoor");
            exitPoint.transform.SetParent(waypointsRoot.transform);
            exitPoint.transform.position = new Vector3(5f, 0f, -2f);

            spawner.spawnPoint = exitPoint.transform;
            spawner.queueTargetPoint = orderPoint.transform;
            spawner.exitTargetPoint = exitPoint.transform;

            // 6. Tạo Canvas HUD & UI Crosshair
            var canvasRoot = new GameObject("[UI_HUD_Canvas]");
            var canvas = canvasRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasRoot.AddComponent<CanvasScaler>();
            canvasRoot.AddComponent<GraphicRaycaster>();
            var hudView = canvasRoot.AddComponent<InteractionHUDView>();

            // Crosshair Image
            var crosshairObj = new GameObject("Crosshair");
            crosshairObj.transform.SetParent(canvasRoot.transform);
            var crosshairImg = crosshairObj.AddComponent<Image>();
            crosshairImg.rectTransform.anchoredPosition = Vector2.zero;
            crosshairImg.rectTransform.sizeDelta = new Vector2(8f, 8f);

            // Prompt Panel
            var promptObj = new GameObject("PromptText");
            promptObj.transform.SetParent(canvasRoot.transform);
            var text = promptObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 20;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.rectTransform.anchoredPosition = new Vector2(0f, -50f);
            text.rectTransform.sizeDelta = new Vector2(600f, 40f);

            // Lưu Scene
            EditorSceneManager.SaveScene(newScene, ScenePath);

            // Đăng ký scene vào EditorBuildSettings
            var scenes = EditorBuildSettings.scenes;
            bool alreadyInBuild = false;
            foreach (var s in scenes)
            {
                if (s.path == ScenePath) { alreadyInBuild = true; break; }
            }

            if (!alreadyInBuild)
            {
                var newBuildScenes = new EditorBuildSettingsScene[scenes.Length + 1];
                scenes.CopyTo(newBuildScenes, 0);
                newBuildScenes[scenes.Length] = new EditorBuildSettingsScene(ScenePath, true);
                EditorBuildSettings.scenes = newBuildScenes;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeGameObject = managersRoot;
            Debug.Log($"[Brew & Dream] >> HOÀN TẤT SETUP SCENE! Đã lưu: {ScenePath}. Nhấn Play để trải nghiệm!");
        }
    }
}
#endif
