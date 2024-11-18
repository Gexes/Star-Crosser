using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript1 : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _moveDirection;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    [SerializeField] private float gravityReverser = -20f; // New variable for custom gravity when shift is held
    private float _verticalVelocity;

    [SerializeField] private float jumpPower;
    [SerializeField] private float speed;
    [SerializeField] private int maxJumps = 2;

    private InputAction _moveAction;
    private int _currentJumps;
    private bool _isGrounded;

    private InputAction _jumpAction;
    private CinemachinePOV _cameraPOV;
    [SerializeField] private float mouseSensitivity = 5f;
    private float _rotationX = 0f;

    private int _extraJumps = 0;

    // Add this to check for Shift key
    private bool isGravityFlipped = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        var playerInput = GetComponent<PlayerInput>();
        _jumpAction = playerInput.actions["Jump"];
        _jumpAction.started += StartJump;

        // Set up the Move action
        _moveAction = playerInput.actions["Move"];
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
        // Update the movement input
        _input = _moveAction.ReadValue<Vector2>();

        // Flip gravity when holding down shift
        if (Keyboard.current.shiftKey.isPressed)
        {
            isGravityFlipped = true;
        }
        else
        {
            isGravityFlipped = false;
        }

        HandleMouseLook();
        UpdateGroundedStatus();
        ApplyGravity();
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
        }
    }

    private void StartJump(InputAction.CallbackContext context)
    {
        if (_currentJumps < maxJumps + _extraJumps)
        {
            _verticalVelocity = jumpPower;
            _currentJumps++;
        }
    }

    private void ApplyGravity()
    {
        // Use the gravityReverser when shift is held down
        if (isGravityFlipped)
        {
            _verticalVelocity += gravityReverser * gravityMultiplier * Time.deltaTime; // Apply custom gravity
        }
        else
        {
            if (!_isGrounded)
            {
                _verticalVelocity += gravity * gravityMultiplier * Time.deltaTime; // Normal gravity
            }
            else
            {
                _verticalVelocity = -1.0f; // Ensure grounded state doesn't apply gravity
            }
        }

        _moveDirection.y = _verticalVelocity;
    }

    private void MoveCharacter()
    {
        Vector3 forwardMovement = transform.forward * _input.y;
        Vector3 strafeMovement = transform.right * _input.x;

        _moveDirection = (forwardMovement + strafeMovement).normalized * speed;
        _moveDirection.y = _verticalVelocity;

        // Apply movement to the CharacterController
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private void OnDisable()
    {
        _jumpAction.started -= StartJump;
    }

    // Methods for power-ups
    public void AddExtraJump(int jumps)
    {
        _extraJumps += jumps;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Detect power-ups upon collision with CharacterController
        PowerUp powerUp = hit.collider.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            powerUp.ActivatePowerUp(_characterController);  // Pass player's CharacterController to activate power-up
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a PowerUp component
        PowerUp powerUp = other.GetComponent<PowerUp>();
        if (powerUp != null)
        {
            // Activate the power-up, passing the player's CharacterController
            powerUp.ActivatePowerUp(_characterController);
        }
    }
}
