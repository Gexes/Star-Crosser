using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _moveDirection;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float reverseGravityMultiplier = 2.0f;
    private float _verticalVelocity;

    [SerializeField] private float jumpPower;
    [SerializeField] private float speed;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private int maxGlides = 1;
    [SerializeField] private float glideSpeed = 1.0f;
    [SerializeField] private float glideDuration = 2.0f;
    [SerializeField] private float maxFloatTime = 1.5f;

    private int _currentJumps;
    private int _glideCount;
    private bool _isGrounded;
    private bool _isFloating;
    private float _glideTimeRemaining;
    private float _floatTimer;

    private InputAction _jumpAction;
    private CinemachinePOV _cameraPOV;
    [SerializeField] private float mouseSensitivity = 5f;
    private float _rotationX = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        _jumpAction.started += StartJump;
        _jumpAction.canceled += EndJump;
    }

    private void Start()
    {
        var virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _cameraPOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
        UpdateGroundedStatus();

        if (_isFloating)
        {
            HandleFloat();
        }
        else
        {
            ApplyGravity();
        }

        MoveCharacter();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        _rotationX += mouseX;

        if (_cameraPOV != null)
        {
            _cameraPOV.m_HorizontalAxis.Value = _rotationX;
            _cameraPOV.m_VerticalAxis.Value = Mathf.Clamp(_cameraPOV.m_VerticalAxis.Value - mouseY, -90f, 90f);
        }

        transform.eulerAngles = new Vector3(0f, _rotationX, 0f);
    }

    private void UpdateGroundedStatus()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -1.0f;
            _currentJumps = 0;
            _glideCount = 0;
        }
    }

    private void StartJump(InputAction.CallbackContext context)
    {
        if (_currentJumps < maxJumps)
        {
            _verticalVelocity = jumpPower;
            _currentJumps++;
            _glideCount = 0; // Reset glides after jump
        }
        else if (!_isGrounded && _glideCount < maxGlides)
        {
            StartFloating();
        }
    }

    private void EndJump(InputAction.CallbackContext context)
    {
        StopFloating();
    }

    private void StartFloating()
    {
        if (_glideCount < maxGlides)
        {
            _isFloating = true;
            _glideTimeRemaining = glideDuration;
            _floatTimer = 0f;
            _glideCount++;
        }
    }

    private void HandleFloat()
    {
        _floatTimer += Time.deltaTime;
        _glideTimeRemaining -= Time.deltaTime;

        // Apply upward force to counter gravity while maintaining direction based on camera
        if (_floatTimer < maxFloatTime)
        {
            _verticalVelocity = Mathf.Lerp(_verticalVelocity, 0, 0.1f); // Gradual floating effect
        }
        else
        {
            _verticalVelocity = 0f;
        }

        // Move in the direction relative to the camera while floating
        Vector3 cameraRelativeDirection = Quaternion.Euler(0, _rotationX, 0) * new Vector3(_input.x, 0, _input.y);
        Vector3 floatDirection = cameraRelativeDirection * glideSpeed;

        _characterController.Move((floatDirection + Vector3.up * _verticalVelocity) * Time.deltaTime);

        if (_glideTimeRemaining <= 0f)
        {
            StopFloating();
        }
    }

    private void StopFloating()
    {
        _isFloating = false;
    }

    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            _verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        _moveDirection.y = _verticalVelocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    private void MoveCharacter()
    {
        Vector3 forwardMovement = transform.forward * _input.y;
        Vector3 strafeMovement = transform.right * _input.x;

        _moveDirection = (forwardMovement + strafeMovement).normalized * speed;
        _moveDirection.y = _verticalVelocity;

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void OnDisable()
    {
        _jumpAction.started -= StartJump;
        _jumpAction.canceled -= EndJump;
    }
}
