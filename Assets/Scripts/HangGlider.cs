using UnityEngine;

public class HangGlider : MonoBehaviour
{
    [Header("Glider Settings")]
    [SerializeField] private float glideSpeed = 10f;
    [SerializeField] private float descendRate = 2f;
    [SerializeField] private WalkMovement walkMovement;
    [SerializeField] private Transform locationSpawner;
    [SerializeField] private ControlState controlState;

    public bool IsGliderActive { get; private set; } = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        SetGliderVisibility(false);
        SetControlState(walking: true, gliding: false);
    }

    public void ToggleGlider(bool activate)
    {
        if (activate == IsGliderActive) return;

        IsGliderActive = activate;
        Debug.Log($"Glider toggled: {IsGliderActive}");

        rb.isKinematic = !IsGliderActive;
        SetGliderVisibility(IsGliderActive);
        walkMovement.EnableMovement(!IsGliderActive);
        SetControlState(walking: !IsGliderActive, gliding: IsGliderActive);
    }

    private void Update()
    {
        if (IsGliderActive)
        {
            HandleGliderMovement();
            transform.position = locationSpawner.position;
        }
    }

    private void HandleGliderMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.forward * vertical * glideSpeed * Time.deltaTime +
                           Vector3.down * descendRate * Time.deltaTime +
                           transform.right * horizontal * glideSpeed * Time.deltaTime;

        rb.velocity = movement;

        // Apply rotation based on horizontal input
        float rotationSpeed = 100f;
        transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
    }

    private void SetGliderVisibility(bool isVisible)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = isVisible;
    }

    private void SetControlState(bool walking, bool gliding)
    {
        controlState.isWalkingEnabled = walking;
        controlState.isGlidingEnabled = gliding;
    }
}
