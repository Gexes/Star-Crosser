using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HangGlider hangGlider;
    [SerializeField] private WalkMovement walkMovement;

    private bool isGliding = false;

    private void Start()
    {
        // Ensure the glider starts in an inactive state
        if (hangGlider != null)
        {
            hangGlider.ToggleGlider(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleGliding();
        }
    }

    private void ToggleGliding()
    {
        // Log the current state to help debug the issue
        Debug.Log($"Toggling gliding. Current isGliding: {isGliding}");

        isGliding = !isGliding;
        hangGlider.ToggleGlider(isGliding);

        // Log the state after toggling
        Debug.Log($"New isGliding state: {isGliding}");
    }
}
