using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour, IDirectionable
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 20f;

    private Rigidbody2D rb;

    private float moveInput;
    private float currentSpeed;

    private bool isDashing;

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
        float horizontalSpeed = currentSpeed * direction;
        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);
    }

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    public Vector2 GetFacingDirection()
    {
        return new Vector2(moveInput, 0f);
    }
}
