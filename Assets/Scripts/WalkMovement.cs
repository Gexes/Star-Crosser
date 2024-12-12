using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class WalkMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpCooldown = 0.5f;

    [Header("Glider Turning Settings")]
    [Range(10f, 50f)][SerializeField] private float turnSpeed = 3f; // Turning speed adjustable in the Inspector


    [Header("Glider Settings")]
    [SerializeField] private float glideSpeed = 10f;
    [SerializeField] private float descendRate = 2f;

    [Header("Glider Speed Settings")]
    [SerializeField] private float accelerationRate = 5f; // How quickly the glider accelerates when W is pressed
    [SerializeField] private float decelerationRate = 2f; // How quickly the glider decelerates when S is pressed
    [SerializeField] private float minSpeed = 1f; // Minimum speed to prevent complete stop
    [SerializeField] private float maxSpeed = 20f; // Maximum speed the glider can reach when accelerating


    [Header("Glider Upward Settings")]
    [SerializeField] private float upwardVelocityIncreaseRate = 2f; // Rate of increase for upward velocity
    [SerializeField] private float maxUpwardVelocity = 10f;
    private float currentUpwardVelocity = 0f; // Tracks current upward velocity

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    [Header("Control State")]
    [SerializeField] private ControlState controlState;

    [Header("Glider Component")]
    [SerializeField] private GameObject glider; // Reference to the glider GameObject

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isJumping = false;
    private Vector3 smoothVelocity;
    private float lastJumpTime = 0f;

    // Animation System
    private Animator animator;
    private string currentAnimation = "";
    private Vector2 movement; // Tracks player input for animations

    private bool isGliderActive = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        animator = GetComponent<Animator>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure the glider starts hidden
        if (glider != null)
        {
            glider.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleGlider(!isGliderActive);
        }

        if (isGliderActive)
        {
            HandleGliderMovement();
        }
        else
        {
            CheckGrounded();
            HandleMovement();
            HandleAnimations();
            HandleJump();
        }

        // Unlock and relock the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void CheckGrounded()
    {
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
        float jumpStartDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(jumpStartDuration);

        while (!isGrounded)
        {
            yield return null;
        }

        ChangeAnimation("IdleStill");
        isJumping = false;
    }

    private void HandleMovement()
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

    private void HandleGliderMovement()
    {
        // Disable gravity during gliding
        rb.useGravity = false;

        // Read input for horizontal (A/D) and vertical (W/S) controls
        float horizontal = Input.GetAxis("Horizontal"); // A & D for turning (yaw)
        float vertical = Input.GetAxis("Vertical");     // W for acceleration, S for deceleration

        // Handle acceleration when W is pressed
        if (vertical > 0) // W Key: Accelerate
        {
            glideSpeed += vertical * Time.deltaTime * accelerationRate;
            glideSpeed = Mathf.Clamp(glideSpeed, minSpeed, maxSpeed); // Apply acceleration cap
        }
        else if (vertical < 1) // S Key: Decelerate
        {
            glideSpeed -= decelerationRate * Time.deltaTime;
            glideSpeed = Mathf.Max(glideSpeed, 5f); // Ensure it doesn't decelerate below 5
        }
        else
        {
            // Decay to base value if no keys are pressed
            glideSpeed = Mathf.Lerp(glideSpeed, minSpeed, Time.deltaTime * decelerationRate);
        }

        // Apply forward movement based on the new calculated glideSpeed
        Vector3 forwardMovement = transform.forward * glideSpeed;

        // Handle upward and downward movement separately
        HandleUpwardMovement();

        // Update the yaw rotation based on A/D input
        float yawChange = horizontal * turnSpeed * Time.deltaTime; // Smooth turning
        float newYaw = transform.eulerAngles.y + yawChange; // Add yaw change without clamping
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newYaw, transform.rotation.eulerAngles.z);

        // Combine forward movement with upward velocity
        Vector3 gliderVelocity = forwardMovement + new Vector3(0f, currentUpwardVelocity - descendRate, 0f);

        // Apply the combined velocity to Rigidbody
        rb.velocity = gliderVelocity;

        // Visual tilting for banking motion (Z-axis tilt)
        float tiltAngle = horizontal * 30f; // Adjust tilt sensitivity
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -tiltAngle);
    }

    private void HandleUpwardMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Increase upward velocity up to the maximum allowed
            currentUpwardVelocity = Mathf.Min(currentUpwardVelocity + upwardVelocityIncreaseRate * Time.deltaTime, maxUpwardVelocity);
        }
        else
        {
            // Gradually reduce upward velocity when Spacebar is released
            currentUpwardVelocity = Mathf.Max(currentUpwardVelocity - upwardVelocityIncreaseRate * Time.deltaTime, 0f);
        }
    }

    private void ToggleGlider(bool activate)
    {
        if (activate == isGliderActive) return;

        isGliderActive = activate;
        controlState.isWalkingEnabled = !activate;
        controlState.isGlidingEnabled = activate;

        if (glider != null)
        {
            glider.SetActive(activate); // Toggle glider visibility
        }

        if (activate)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX;

            ChangeAnimation("Flying");
        }
        else
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            ChangeAnimation("IdleStill");
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
}
