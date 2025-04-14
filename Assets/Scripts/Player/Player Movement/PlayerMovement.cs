using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour, IDirectionable
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 20f;

    private float speedMultiplier = 1f;
    private Rigidbody2D rb;
    private float moveInput;
    private float currentSpeed;
    private float currentDirection;
    private float lastDirection = 1;
    private bool isDashing;
    private float previousMoveInput = 0f;

    public float SpeedMultiplier
    {
        get
        {
            return speedMultiplier;
        }
        set
        {
            if (value >= 1) speedMultiplier = value;
            else speedMultiplier = 1f;
        }
    }

    private void OnEnable()
    {
        PlayerEvent.OnDashChanged += HandleDash;
    }

    private void OnDisable()
    {
        PlayerEvent.OnDashChanged -= HandleDash;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = 0;
    }

    private void Update()
    {
        if (!isDashing)
        {
            previousMoveInput = moveInput;
            moveInput = Input.GetAxis("Horizontal");
        }
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

        float horizontalSpeed = currentSpeed * currentDirection * speedMultiplier;
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

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    public Vector2 GetFacingDirection()
    {
        float xDirection = currentDirection != 0 ? currentDirection : lastDirection;
        return new Vector2 (xDirection, 0f);
    }
}
