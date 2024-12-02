using UnityEngine;

[CreateAssetMenu(fileName = "ControlState", menuName = "States/ControlState", order = 0)]
public class ControlState : ScriptableObject
{
    [Header("Control State Settings")]
    [SerializeField] public bool isWalkingEnabled = true; // Default value
    [SerializeField] public bool isGlidingEnabled = false; // Default value

    // Properties for controlled access
    public bool IsWalkingEnabled
    {
        get { return isWalkingEnabled; }
        set { isWalkingEnabled = value; }
    }

    public bool IsGlidingEnabled
    {
        get { return isGlidingEnabled; }
        set { isGlidingEnabled = value; }
    }
}
