using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class WalkMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpCooldown = 0.5f; // Cooldown duration for the jump input

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    [Header("Control State")]
    [SerializeField] private ControlState controlState;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isJumping = false; // Flag to indicate jump status
    private Vector3 smoothVelocity;
    private float lastJumpTime = 0f; // Time of the last jump

    // Animation System
    private Animator animator;
    private string currentAnimation = "";

    private Vector2 movement; // Tracks player input for animations

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent tipping over
        animator = GetComponent<Animator>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!controlState.isGlidingEnabled) // Prevent movement if gliding is enabled
        {
            CheckGrounded();
            HandleMovement();
            HandleAnimations();
            HandleJump();
        }

        // Unlock the cursor if Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Relock the cursor if the left mouse button is pressed
        if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void CheckGrounded()
    {
        // Simple ground check using collider's contact points or other means
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.6f);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = true;
            lastJumpTime = Time.time;

            ChangeAnimation("Jump Start");
            StartCoroutine(CrossfadeToIdleAfterJump());
        }
        else if (isGrounded && isJumping)
        {
            StartCoroutine(ResetJumpState());
        }
    }

    private void HandleAnimations()
    {
        if (isJumping)
        {
            if (!IsAnimationPlaying("Jump Start"))
                ChangeAnimation("Jump Start");
            return;
        }

        // Check grounded state for walking or idle animations
        if (isGrounded)
        {
            if (movement.y > 0)
            {
                ChangeAnimation("Walk Forward");
            }
            else if (movement.y < 0)
            {
                ChangeAnimation("Walk Backward");
            }
            else if (movement.x > 0)
            {
                ChangeAnimation("Walk Right");
            }
            else if (movement.x < 0)
            {
                ChangeAnimation("Walk Left");
            }
            else
            {
                ChangeAnimation("IdleStill");
            }
        }
    }

    private IEnumerator CrossfadeToIdleAfterJump()
    {
        // Wait for the "Jump Start" animation to complete
        float jumpStartDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(jumpStartDuration);

        // Wait until grounded
        while (!isGrounded)
        {
            yield return null;
        }

        ChangeAnimation("IdleStill");
        isJumping = false;
    }

    private void HandleMovement()
    {
        if (controlState.isWalkingEnabled) // Only handle movement if walking is enabled
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            movement = new Vector2(moveX, moveZ);

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 targetDirection = forward * moveZ + right * moveX;

            if (targetDirection.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
                float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
            }

            Vector3 desiredVelocity = targetDirection * movementSpeed;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(desiredVelocity.x, rb.velocity.y, desiredVelocity.z), ref smoothVelocity, 0.1f);
        }
    }

    private bool IsAnimationPlaying(string animation)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animation);
    }

    public void ChangeAnimation(string animation, float crossfade = 0.1f)
    {
        if (currentAnimation != animation)
        {
            animator.CrossFade(animation, crossfade);
            currentAnimation = animation;
        }
    }

    private IEnumerator ResetJumpState()
    {
        float jumpStartDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(jumpStartDuration);
        isJumping = false;
    }

    public void EnableMovement(bool isEnabled)
    {
        enabled = isEnabled;
        rb.velocity = Vector3.zero; // Stop movement when disabled
    }
}
