using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour, IDirectionable
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 20f;

    private bool isDashing;

    private Rigidbody2D rb;

    private float moveInput;
    private float previousMoveInput = 0f;
    private float currentSpeed;
    private float currentDirection;
    private float lastDirection = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = 0;
    }

    private void OnEnable()
    {
        PlayerEvent.OnDashChanged += HandleDash;
    }

    private void OnDisable()
    {
        PlayerEvent.OnDashChanged -= HandleDash;
    }

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    private void Update()
    {
        previousMoveInput = moveInput;
        moveInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            ApplyMovement();
        }
    }

    private void ApplyMovement()
    {
        bool abruptDirectionChange = 
            (previousMoveInput * moveInput < 0) && 
            (Mathf.Abs(previousMoveInput) > 0.8f) && 
            (Mathf.Abs(moveInput) > 0.8f);

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            float targetSpeed = maxSpeed * MathF.Abs(moveInput);

            if(abruptDirectionChange)
            {
                currentSpeed = Mathf.Max(currentSpeed, targetSpeed);
                currentDirection = Mathf.Sign(moveInput);
            }

            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
                UpdateDirection();
            }
        }

        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            UpdateDirection();
        }

        float horizontalSpeed = currentSpeed * currentDirection;
        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
    }

    private void UpdateDirection()
    {
        if (moveInput != 0)
        {
            currentDirection = Mathf.Sign(moveInput);
        }

        else
        {
            if (currentDirection != 0) lastDirection = currentDirection;
            currentDirection = 0;
        }
    }

    public Vector2 GetFacingDirection()
    {
        float xDirection = currentDirection != 0 ? currentDirection : lastDirection;
        return new Vector2 (xDirection, 0f);
    }
}
