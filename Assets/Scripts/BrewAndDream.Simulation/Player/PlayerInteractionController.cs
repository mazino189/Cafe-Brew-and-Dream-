using System;
using UnityEngine;

namespace BrewAndDream.Simulation.Player
{
    /// <summary>
    /// Bộ điều khiển tương tác góc nhìn thứ nhất (FPS Raycast Interactor).
    /// Hỗ trợ quét vật thể quầy bar, nhặt/cầm vật dụng và thao tác đong rót.
    /// </summary>
    public class PlayerInteractionController : MonoBehaviour
    {
        [Header("Cấu hình Raycast")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float reachDistance = 2.2f;
        [SerializeField] private LayerMask interactableLayer;

        [Header("Vị trí cầm đồ vật trên tay (Hold Position)")]
        [SerializeField] private Transform handHoldTransform;
        [SerializeField] private float smoothHoldSpeed = 15f;

        private IInteractable _currentLookTarget;
        private GameObject _heldObject;

        public event Action<string> OnHoverPromptChanged;
        public GameObject HeldObject => _heldObject;

        private void Reset()
        {
            playerCamera = Camera.main;
        }

        private void Update()
        {
            PerformRaycast();
            HandleInput();
        }

        private void LateUpdate()
        {
            UpdateHeldObjectPosition();
        }

        private void PerformRaycast()
        {
            if (playerCamera == null) return;

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, reachDistance, interactableLayer))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null && interactable.CanInteract)
                {
                    if (_currentLookTarget != interactable)
                    {
                        _currentLookTarget = interactable;
                        OnHoverPromptChanged?.Invoke(_currentLookTarget.GetInteractionPrompt());
                    }
                    return;
                }
            }

            if (_currentLookTarget != null)
            {
                _currentLookTarget = null;
                OnHoverPromptChanged?.Invoke(string.Empty);
            }
        }

        private void HandleInput()
        {
            // Tương tác chính: Chuột trái hoặc E
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
            {
                _currentLookTarget?.OnInteract(this);
            }

            // Tương tác phụ: Chuột phải hoặc Q (Ví dụ: đặt đồ xuống khay, hủy cầm)
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Q))
            {
                _currentLookTarget?.OnAlternateInteract(this);
            }
        }

        public void HoldObject(GameObject obj)
        {
            _heldObject = obj;
            if (obj.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }
        }

        public GameObject ReleaseHeldObject()
        {
            var obj = _heldObject;
            if (obj != null)
            {
                if (obj.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.isKinematic = false;
                    rb.detectCollisions = true;
                }
                _heldObject = null;
            }
            return obj;
        }

        private void UpdateHeldObjectPosition()
        {
            if (_heldObject != null && handHoldTransform != null)
            {
                _heldObject.transform.position = Vector3.Lerp(
                    _heldObject.transform.position,
                    handHoldTransform.position,
                    Time.deltaTime * smoothHoldSpeed
                );
                _heldObject.transform.rotation = Quaternion.Slerp(
                    _heldObject.transform.rotation,
                    handHoldTransform.rotation,
                    Time.deltaTime * smoothHoldSpeed
                );
            }
        }
    }
}

