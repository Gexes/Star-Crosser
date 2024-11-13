using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public CharacterController playerController;
    public float reenableDelay = 2f;  // Delay to turn CharacterController back on

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object has the tag "SceneChanger"
        if (hit.gameObject.CompareTag("SceneChanger"))
        {
            Debug.Log("SceneChanger collided with");

            // Disable CharacterController
            playerController.enabled = false;

            // Load the next scene (index 1 in this case)
            SceneManager.LoadScene(1);

            // Re-enable the CharacterController after a delay
            StartCoroutine(ReenableCharacterController());
        }
    }

    private IEnumerator ReenableCharacterController()
    {
        yield return new WaitForSeconds(reenableDelay);
        playerController.enabled = true;
    }
}
