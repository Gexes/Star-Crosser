using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Transform target; // Component to rotate, defaults to this object's transform
    [SerializeField, Range(0f, 5000f)] private float rotationSpeed = 10f; // Speed of rotation, adjustable via slider

    private void Start()
    {
        // Default to this object's transform if no target is assigned
        if (target == null)
        {
            target = transform;
        }
    }

    private void Update()
    {
        // Rotate the target on the Y-axis at the specified speed
        target.Rotate(0, rotationSpeed * Time.deltaTime, 0, 0);
    }

    
}
