using System;
using System.IO.IsolatedStorage;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 20f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float moveInput;
    private float currentSpeed;

    private bool isDashing;
    private bool isFlipped;
    private bool lastFlip;

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
        sr = GetComponent<SpriteRenderer>();
        isFlipped = sr.flipX;
        currentSpeed = 0;
    }

    private void Update()
    {
        if (!isDashing)
        {
            moveInput = Input.GetAxis("Horizontal");
            Move();
            HandleFlip();
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
        bool changingDirection = (moveInput > 0 && rb.linearVelocity.x < 0) || (moveInput < 0 && rb.linearVelocity.x > 0);

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            float targetSpeed = maxSpeed * MathF.Abs(moveInput);
            float accelerationRate = changingDirection ? deceleration : acceleration;

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelerationRate * Time.fixedDeltaTime);
        }

        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        float direction = moveInput != 0 ? Mathf.Sign(moveInput) : 0;
        rb.linearVelocity = new Vector2(direction * currentSpeed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            bool previousFlipState = isFlipped;

            if (moveInput > 0)
            {
                sr.flipX = false;
                isFlipped = false;
            }
            else if (moveInput < 0)
            {
                sr.flipX = true;
                isFlipped = true;
            }

            if (previousFlipState != isFlipped)
            {
                PlayerEvent.FlipChanged(isFlipped);
            }
        }
    }

    private void Move()
    {
        
    }

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }
}
