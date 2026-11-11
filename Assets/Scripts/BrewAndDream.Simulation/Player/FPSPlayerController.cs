using System;
using UnityEngine;

namespace BrewAndDream.Simulation.Player
{
    /// <summary>
    /// Bộ điều khiển di chuyển và góc nhìn thứ nhất (FPS Controller).
    /// Hỗ trợ WASD di chuyển, phím chuột xoay camera, Shift để chạy nhanh.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FPSPlayerController : MonoBehaviour
    {
        [Header("Tốc độ di chuyển")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float sprintSpeed = 6.0f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Camera & Độ nhạy chuột")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float mouseSensitivity = 2.0f;
        [SerializeField] private float maxLookAngle = 80f;

        private CharacterController _characterController;
        private float _verticalRotation = 0f;
        private Vector3 _velocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovement();
            HandleCursorToggle();
        }

        private void HandleMouseLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Xoay thân player theo trục Y (trái/phải)
            transform.Rotate(Vector3.up * mouseX);

            // Xoay camera theo trục X (ngước lên/cúi xuống)
            _verticalRotation -= mouseY;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -maxLookAngle, maxLookAngle);

            if (playerCamera != null)
            {
                playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
            }
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            _characterController.Move(moveDirection * (currentSpeed * Time.deltaTime));

            // Trọng lực đơn giản giữ người chơi bám mặt sàn quầy bar
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            _velocity.y += gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void HandleCursorToggle()
        {
            // Nhấn Alt hoặc Escape để mở khóa con trỏ chuột khi cần bấm UI
            if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }
}

