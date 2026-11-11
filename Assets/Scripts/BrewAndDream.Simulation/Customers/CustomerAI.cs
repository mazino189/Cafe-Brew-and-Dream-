using System;
using UnityEngine;
using UnityEngine.AI;
using BrewAndDream.Core.Models;
using BrewAndDream.Core.Engines;
using BrewAndDream.Simulation.BaristaStation;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.Customers
{
    public enum CustomerState
    {
        WalkingToCounter, // Đang đi tới quầy order
        Ordering,         // Đang chọn món / chờ thu ngân
        WaitingForDrink,  // Đang chờ pha nước
        DrinkingAtTable,  // Ngồi thưởng thức tại bàn
        LeavingShop       // Rời quán
    }

    /// <summary>
    /// AI Khách hàng sử dụng Unity NavMesh và Máy trạng thái FSM.
    /// Tích hợp tính kiên nhẫn, nhận đồ uống và chấm điểm dịch vụ.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class CustomerAI : MonoBehaviour, IInteractable
    {
        [Header("Tính cách & Nhu cầu")]
        public NPCPersonalityModel personality;
        public DrinkRecipeModel orderedRecipe;

        [Header("Mục tiêu di chuyển")]
        public Transform counterOrderPoint;
        public Transform assignedTablePoint;
        public Transform exitDoorPoint;

        [Header("Trạng thái hiện tại")]
        public CustomerState currentState = CustomerState.WalkingToCounter;
        public float waitTimer = 0f;
        public bool hasLeftReview = false;

        private NavMeshAgent _agent;
        private bool _hasOrdered = false;

        public bool CanInteract => currentState == CustomerState.WaitingForDrink;

        public event Action<EvaluationOutput> OnServiceEvaluated;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            MoveTo(counterOrderPoint);
        }

        private void Update()
        {
            switch (currentState)
            {
                case CustomerState.WalkingToCounter:
                    if (HasReachedDestination())
                    {
                        currentState = CustomerState.Ordering;
                    }
                    break;

                case CustomerState.Ordering:
                    // Đợi nhân viên hoặc người chơi nhận order
                    if (!_hasOrdered)
                    {
                        _hasOrdered = true;
                        currentState = CustomerState.WaitingForDrink;
                    }
                    break;

                case CustomerState.WaitingForDrink:
                    waitTimer += Time.deltaTime;
                    if (personality != null && waitTimer >= personality.maxWaitTimeSeconds)
                    {
                        // Hết kiên nhẫn, bỏ về
                        LeaveWithAngryReview();
                    }
                    break;

                case CustomerState.DrinkingAtTable:
                    // Thưởng thức trong 15s rồi ra về
                    waitTimer += Time.deltaTime;
                    if (waitTimer >= 15f)
                    {
                        currentState = CustomerState.LeavingShop;
                        MoveTo(exitDoorPoint);
                    }
                    break;

                case CustomerState.LeavingShop:
                    if (HasReachedDestination())
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        public string GetInteractionPrompt()
        {
            if (currentState == CustomerState.WaitingForDrink)
            {
                return $"Giao đồ uống cho khách: {personality?.archetypeName ?? "Khách"}";
            }
            return string.Empty;
        }

        public void OnInteract(PlayerInteractionController player)
        {
            if (currentState == CustomerState.WaitingForDrink && player.HeldObject != null)
            {
                var cup = player.HeldObject.GetComponent<CupItem>();
                if (cup != null)
                {
                    // Người chơi giao ly nước cho khách
                    player.ReleaseHeldObject();
                    ReceiveDrink(cup);
                }
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player) { }

        private void ReceiveDrink(CupItem cup)
        {
            cup.MarkAsServed();
            Destroy(cup.gameObject);

            // Chấm điểm dịch vụ
            var context = new ServiceEvaluationContext
            {
                orderedRecipe = orderedRecipe,
                playerStepLogs = cup.executedLogs,
                actualFlavorProfile = cup.currentFlavorProfile,
                actualWaitTimeSeconds = waitTimer,
                sellingPriceVND = orderedRecipe != null ? orderedRecipe.referenceMarketPriceVND : 30000,
                playerAttitudeScore01 = 0.9f,
                shopHygieneScore01 = 0.85f,
                shopAmbianceMatch01 = 0.8f
            };

            var evalOutput = NPCSatisfactionCalculator.EvaluateService(personality, context);
            OnServiceEvaluated?.Invoke(evalOutput);

            // Chuyển sang bàn ngồi
            if (assignedTablePoint != null)
            {
                currentState = CustomerState.DrinkingAtTable;
                waitTimer = 0f;
                MoveTo(assignedTablePoint);
            }
            else
            {
                currentState = CustomerState.LeavingShop;
                MoveTo(exitDoorPoint);
            }
        }

        private void LeaveWithAngryReview()
        {
            currentState = CustomerState.LeavingShop;
            MoveTo(exitDoorPoint);

            var context = new ServiceEvaluationContext
            {
                orderedRecipe = orderedRecipe,
                playerStepLogs = null,
                actualWaitTimeSeconds = waitTimer,
                sellingPriceVND = 30000,
                playerAttitudeScore01 = 0.2f,
                shopHygieneScore01 = 0.5f,
                shopAmbianceMatch01 = 0.5f
            };

            var evalOutput = NPCSatisfactionCalculator.EvaluateService(personality, context);
            OnServiceEvaluated?.Invoke(evalOutput);
        }

        private void MoveTo(Transform target)
        {
            if (target != null && _agent != null && _agent.isOnNavMesh)
            {
                _agent.SetDestination(target.position);
            }
        }

        private bool HasReachedDestination()
        {
            if (_agent == null) return true;
            return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
        }
    }
}

