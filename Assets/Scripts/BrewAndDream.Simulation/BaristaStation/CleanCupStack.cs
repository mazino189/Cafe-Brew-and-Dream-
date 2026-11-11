using System;
using UnityEngine;
using BrewAndDream.Simulation.Player;

namespace BrewAndDream.Simulation.BaristaStation
{
    /// <summary>
    /// Chồng ly sạch ở đầu quầy Bar.
    /// Cho phép người chơi lấy ly mới để bắt đầu pha chế món mới.
    /// Có cơ chế tự động Fallback tạo ly nếu chưa gán prefab trong Inspector.
    /// </summary>
    public class CleanCupStack : MonoBehaviour, IInteractable
    {
        [Header("Prefab ly sẽ tạo")]
        [SerializeField] private GameObject cupPrefab;

        public bool CanInteract => true;

        public string GetInteractionPrompt()
        {
            return "Nhấn [E] hoặc [Chuột Trái] để lấy ly sạch mới";
        }

        public void OnInteract(PlayerInteractionController player)
        {
            if (player.HeldObject == null)
            {
                GameObject newCup;
                if (cupPrefab != null)
                {
                    newCup = Instantiate(cupPrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity);
                }
                else
                {
                    // Fallback: Tự động dựng ly mẫu nếu chưa kéo Prefab
                    newCup = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    newCup.name = "Cup_Glass_Medium";
                    newCup.transform.position = transform.position + Vector3.up * 0.2f;
                    newCup.transform.localScale = new Vector3(0.12f, 0.15f, 0.12f);

                    var rb = newCup.AddComponent<Rigidbody>();
                    rb.mass = 0.2f;
                    rb.isKinematic = true;

                    var cupItem = newCup.AddComponent<CupItem>();
                    cupItem.cupName = "Ly Bạc Xỉu Sài Gòn";
                }

                player.HoldObject(newCup);
            }
        }

        public void OnAlternateInteract(PlayerInteractionController player) { }
    }
}
