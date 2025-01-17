using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControler : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float jumpImpulse = 10f;

    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isFacingRight;

    private Joystick _joystick;
    private Rigidbody2D _rb;
    private HealthBar _healh;

    private Vector2 _moveInput;

    public bool IsMoving
    {
        get => _isMoving;
        private set => _isMoving = value;
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

            _isFacingRight = value;
        }
    }

    private void Awake()
    {
        _joystick = FindObjectOfType<Joystick>();
        _rb = GetComponent<Rigidbody2D>();
        _healh = GetComponent<HealthBar>();
    }

    private void Update()
    {
        HandleMovement(_moveInput);
        _healh.IsAlive();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        IsMoving = _moveInput != Vector2.zero;

        SetFacingDirection(_moveInput);
    }

    private void HandleMovement(Vector2 moveInput)
    {
        moveInput.x = _joystick.Horizontal;
        Vector2 targetVelocity = new Vector2(moveInput.x * walkSpeed, _rb.velocity.y);
        _rb.velocity = targetVelocity;
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if ((-moveInput.x < 0 && IsFacingRight) || (-moveInput.x > 0 && !IsFacingRight))
            IsFacingRight = !IsFacingRight;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
            _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
    }

    private bool IsGrounded() => Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
}