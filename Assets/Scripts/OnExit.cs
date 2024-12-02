using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExit : StateMachineBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Name of the animation to transition to when 'Jump Start' begins.")]
    public string transitionToAnimation = "IdleStill"; // Set default to "IdleStill"

    // OnStateEnter is called when a transition starts and the state machine begins evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ensure we only execute when the "Jump Start" animation begins
        if (stateInfo.IsName("Jump Start"))
        {
            // Find the WalkMovement script on the same GameObject
            WalkMovement walkMovement = animator.GetComponent<WalkMovement>();
            if (walkMovement != null)
            {
                // Call the ChangeAnimation method to transition to the specified animation
                walkMovement.ChangeAnimation(transitionToAnimation);
            }
        }
    }
}
