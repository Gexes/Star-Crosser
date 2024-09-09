using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float JumpPower;
    [SerializeField] private float speed;
    private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, y:0.0f, z:_input.y);
    }
    void Start()
    {

    }

    //Update is a lot like Start, but it automatically gets triggered once per frame
    //Most of an object's code will be called from Update--it controls things that happen in real time
    private void Update()
    {
      ApplyGravity();
      ApplyRotation();
      ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        _characterController.Move(motion: _direction * speed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        _characterController.Move(motion: _direction * speed * Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
        if (_numberOfJumps == 0) StartCoroutine(routine: WaitForLanding());
        _numberOfJumps++;
        _velocity += JumpPower;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0;
    }

    public void jumpHold(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        _gravity = -2.0f;
        if (gravityMultiplier == 1) StartCoroutine(routine: WaitForLanding());

      
    }

    private bool IsGrounded() => _characterController.isGrounded;
}
