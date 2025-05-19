using System;
using UnityEngine;

public class RunProvider : IDirectionable
{
    private readonly Rigidbody2D rb;
    private readonly IPlayerInput input;
    private readonly RunSettings settings;

    private bool isDashing;
    private float moveInput;
    private float previousMoveInput = 0f;
    private float currentSpeed;
    private float currentDirection;
    private float lastDirection = 1;

    public RunProvider(Rigidbody2D rb, IPlayerInput input, RunSettings settings)
    {
        this.rb = rb;
        this.input = input;
        this.settings = settings;

        currentSpeed = 0;

        PlayerEvent.OnDashChanged += HandleDash;
    }

    ~RunProvider()
    {
        PlayerEvent.OnDashChanged -= HandleDash;
    }

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    public void Update()
    {
        previousMoveInput = moveInput;
        moveInput = input.HorizontalInput;
    }

    public void FixedUpdate()
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
            float targetSpeed = settings.MaxSpeed * MathF.Abs(moveInput);

            if(abruptDirectionChange)
            {
                currentSpeed = Mathf.Max(currentSpeed, targetSpeed);
                currentDirection = Mathf.Sign(moveInput);
            }

            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, settings.Acceleration * Time.fixedDeltaTime);
                UpdateDirection();
            }
        }

        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, settings.Deceleration * Time.deltaTime);
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
