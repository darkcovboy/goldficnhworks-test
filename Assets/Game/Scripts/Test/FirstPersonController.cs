using UnityEngine;

namespace Game.Scripts.Test
{
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float _walkSpeed = 3.0f;
        [SerializeField] private float _runSpeed = 6.0f;
        [SerializeField] private float _jumpForce = 7.0f;
        [SerializeField] private float _gravity = 20.0f;

        [Header("Look")] [SerializeField] private float _mouseSensitivity = 2.0f;
        [SerializeField] private float _verticalLookLimit = 80.0f;

        [Header("Components")] [SerializeField]
        private Camera _playerCamera;

        [SerializeField] private CharacterController _characterController;

        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX = 0;
        private bool _canMove = true;

        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }

        private void Start()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();

            if (_playerCamera == null)
                _playerCamera = GetComponentInChildren<Camera>();

            LockCursor();
        }

        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
            HandleCursorToggle();
        }

        private void HandleMovement()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeedX = _canMove ? (isRunning ? _runSpeed : _walkSpeed) * Input.GetAxis("Vertical") : 0;
            float currentSpeedY = _canMove ? (isRunning ? _runSpeed : _walkSpeed) * Input.GetAxis("Horizontal") : 0;

            float movementDirectionY = _moveDirection.y;
            _moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);

            if (Input.GetButton("Jump") && _canMove && _characterController.isGrounded)
            {
                _moveDirection.y = _jumpForce;
            }
            else
            {
                _moveDirection.y = movementDirectionY;
            }

            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _gravity * Time.deltaTime;
            }

            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            if (!_canMove) return;

            _rotationX += -Input.GetAxis("Mouse Y") * _mouseSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _mouseSensitivity, 0);
        }

        private void HandleCursorToggle()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleCursor();
            }
        }

        private void ToggleCursor()
        {
            _canMove = !_canMove;

            if (_canMove)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}