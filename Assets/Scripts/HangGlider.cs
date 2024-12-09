using UnityEngine;

public class HangGlider : MonoBehaviour
{
    [Header("Glider Settings")]
    [SerializeField] private float glideSpeed = 10f;
    [SerializeField] private float descendRate = 2f;
    [SerializeField] private WalkMovement walkMovement;
    [SerializeField] private ControlState controlState;

    private Rigidbody rb;
    private Transform playerTransform;

    public bool IsGliderActive { get; private set; } = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        SetGliderVisibility(false);
    }

    public void ToggleGlider(bool activate)
    {
        if (activate == IsGliderActive) return;

        IsGliderActive = activate;
        rb.isKinematic = !activate;
        SetGliderVisibility(activate);

        walkMovement.EnableMovement(!activate);
        controlState.isWalkingEnabled = !activate;
        controlState.isGlidingEnabled = activate;

        // Attach or detach player
        if (activate)
        {
            playerTransform = walkMovement.transform;
            playerTransform.SetParent(transform); // Attach to glider
        }
        else
        {
            if (playerTransform != null)
            {
                playerTransform.SetParent(null); // Detach from glider
            }
        }
    }

    private void Update()
    {
        if (IsGliderActive)
        {
            HandleGliderMovement();
        }
    }

    private void HandleGliderMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Movement forward/backward and side to side
        Vector3 forwardMovement = transform.forward * vertical * glideSpeed * Time.deltaTime;
        Vector3 swayMovement = transform.right * horizontal * glideSpeed * Time.deltaTime;

        // Apply gravity-like effect for descent
        Vector3 downwardMovement = Vector3.down * descendRate * Time.deltaTime;

        // Update the Rigidbody velocity
        rb.velocity = forwardMovement + downwardMovement + swayMovement;
    }

    private void SetGliderVisibility(bool isVisible)
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = isVisible;
        }
    }
}
