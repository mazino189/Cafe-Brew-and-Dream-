using System;
using System.Collections.Generic;
using UnityEngine;
using BrewAndDream.Core.Enums;
using BrewAndDream.Core.Models;
using BrewAndDream.Simulation.Customers;
using BrewAndDream.UnityBridge;

namespace BrewAndDream.Management
{
    [Serializable]
    public struct SpawnWaveConfig
    {
        public ShiftType shift;
        public float spawnIntervalSeconds; // Tần suất xuất hiện khách (giây/khách)
        public int maxConcurrentCustomers; // Số khách tối đa cùng lúc trong quán
        public List<NPCPersonalitySOBridge> candidateArchetypes; // Các mẫu khách hay vào ca này
    }

    [Serializable]
    public class NPCPersonalitySOBridge
    {
        public NPCPersonalityModel personalityData;
        public GameObject customerPrefab;
    }

    /// <summary>
    /// Bộ sinh khách hàng tự động theo nhịp độ từng ca trong ngày:
    /// - Ca Sáng: Chủ yếu dân văn phòng mua vội mang đi.
    /// - Ca Trưa: Cao điểm dồn dập, sinh viên và dân văn phòng ăn trưa ghé qua.
    /// - Ca Tối: Thưa thớt hơn nhưng ngồi lâu, chuộng không gian acoustic/chill.
    /// </summary>
    public class CustomerTrafficSpawner : MonoBehaviour
    {
        [Header("Điểm neo bản đồ quán")]
        [SerializeField] private Transform spawnDoorPoint;
        [SerializeField] private Transform counterQueuePoint;
        [SerializeField] private Transform exitDoorPoint;
        [SerializeField] private List<Transform> availableTablePoints = new List<Transform>();

        [Header("Cấu hình làn sóng khách 3 ca")]
        [SerializeField] private List<SpawnWaveConfig> shiftWaveConfigs = new List<SpawnWaveConfig>();

        private ShiftType _activeShift = ShiftType.Morning;
        private bool _isSpawningActive = false;
        private float _spawnTimer = 0f;
        private List<CustomerAI> _activeCustomers = new List<CustomerAI>();

        public void SetActiveShift(ShiftType shift)
        {
            _activeShift = shift;
            _spawnTimer = 0f;
        }

        public void StartSpawning()
        {
            _isSpawningActive = true;
        }

        public void StopSpawning()
        {
            _isSpawningActive = false;
        }

        private void Update()
        {
            if (!_isSpawningActive) return;

            // Dọn dẹp danh sách khách đã rời quán
            _activeCustomers.RemoveAll(c => c == null);

            var config = GetConfigForShift(_activeShift);
            if (_activeCustomers.Count >= config.maxConcurrentCustomers) return;

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= config.spawnIntervalSeconds)
            {
                _spawnTimer = 0f;
                SpawnCustomer(config);
            }
        }

        private void SpawnCustomer(SpawnWaveConfig config)
        {
            if (config.candidateArchetypes == null || config.candidateArchetypes.Count == 0) return;

            int randomIndex = UnityEngine.Random.Range(0, config.candidateArchetypes.Count);
            var chosen = config.candidateArchetypes[randomIndex];
            if (chosen.customerPrefab == null) return;

            Vector3 spawnPos = spawnDoorPoint != null ? spawnDoorPoint.position : transform.position;
            GameObject npcObj = Instantiate(chosen.customerPrefab, spawnPos, Quaternion.identity);
            
            var ai = npcObj.GetComponent<CustomerAI>();
            if (ai != null)
            {
                ai.personality = chosen.personalityData;
                ai.counterOrderPoint = counterQueuePoint;
                ai.exitDoorPoint = exitDoorPoint;
                ai.assignedTablePoint = GetRandomFreeTable();
                _activeCustomers.Add(ai);
            }
        }

        private Transform GetRandomFreeTable()
        {
            if (availableTablePoints.Count == 0) return null;
            int idx = UnityEngine.Random.Range(0, availableTablePoints.Count);
            return availableTablePoints[idx];
        }

        private SpawnWaveConfig GetConfigForShift(ShiftType shift)
        {
            var found = shiftWaveConfigs.Find(c => c.shift == shift);
            if (found.candidateArchetypes != null) return found;

            // Cấu hình mặc định nếu chưa gán Inspector
            return new SpawnWaveConfig
            {
                shift = shift,
                spawnIntervalSeconds = shift == ShiftType.NoonRush ? 15f : 30f,
                maxConcurrentCustomers = shift == ShiftType.NoonRush ? 8 : 4,
                candidateArchetypes = new List<NPCPersonalitySOBridge>()
            };
        }
    }
}
