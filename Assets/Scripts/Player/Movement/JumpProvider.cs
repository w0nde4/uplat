using UnityEngine;

public class JumpProvider
{
    private readonly JumpSettings jumpSettings;
    private readonly IPlayerInput input;
    private readonly Rigidbody2D rb;
    private readonly Collider2D col;

    private float jumpForce;
    private float gravityValue;

    private bool isJumping = false;
    private bool isFalling = false;
    private bool isGrounded = false;
    private bool jumpCancelled = false;
    private bool isAtApex = false;
    private bool isDashing;

    private float lastGroundedTime = 0f;
    private float lastJumpPressedTime = 0f;
    private float apexTimer = 0f;

    private readonly float defaultYVelocity = 1.33f; //calculated empirically, do not change

    public JumpProvider(Rigidbody2D rb, IPlayerInput playerInput, JumpSettings jumpSettings, Collider2D col)
    {
        this.rb = rb;
        this.input = playerInput;
        this.jumpSettings = jumpSettings;
        this.col = col;

        UpdateStartParameters();

        PlayerEvent.OnDashChanged += HandleDash;
    }

    ~JumpProvider()
    {
        PlayerEvent.OnDashChanged -= HandleDash;
    }

    public void Update()
    {
        if (!isDashing)
        {
            CheckGrounded();
            HandleJumpInput();
            UpdateTimers();
        }
    }

    public void FixedUpdate()
    {
        if (!isDashing)
        {
            ApplyGravity();
            HandleJump();
        }
    }

    private void HandleJump()
    {
        bool canJump = Time.time - lastGroundedTime <= jumpSettings.CoyoteTime;
        bool wantsToJump = Time.time - lastJumpPressedTime <= jumpSettings.JumpBufferTime;

        if (canJump && wantsToJump && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            isFalling = false;

            lastJumpPressedTime = 0f;
        }

        if (isJumping && !isFalling && !isAtApex && Mathf.Abs(rb.linearVelocity.y) < defaultYVelocity + 0.1f)
        {
            isAtApex = true;
            apexTimer = 0f;
        }

        if (isJumping && rb.linearVelocity.y < 0 && !isAtApex)
        {
            isJumping = false;
            isFalling = true;
        }

    }

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    private void HandleJumpInput()
    {
        if (input.JumpPressed)
        {
            lastJumpPressedTime = Time.time;
        }

        if (input.JumpReleased && isJumping && !isFalling)
        {
            jumpCancelled = true;
        }
    }

    private void UpdateStartParameters()
    {
        gravityValue = -(2 * jumpSettings.MaxJumpHeight) / Mathf.Pow(jumpSettings.JumpTimeToApex, 2);
        jumpForce = Mathf.Abs(gravityValue) * jumpSettings.JumpTimeToApex;
    }

    private void ApplyGravity()
    {
        float gravityScale = 1f;

        if (isAtApex)
        {
            gravityScale = 0f;
        }

        else if (isJumping && !isFalling && !jumpCancelled)
        {
            gravityScale = jumpSettings.JumpGravityMultiplier;
        }

        else if (jumpCancelled)
        {
            gravityScale = jumpSettings.JumpCancelGravityMultiplier;
        }

        else if (isFalling || rb.linearVelocity.y < 0)
        {
            gravityScale = jumpSettings.FallGravityMultiplier;

        }

        if (rb.linearVelocity.y < jumpSettings.MaxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSettings.MaxFallSpeed);
        }

        rb.linearVelocity += Vector2.up * gravityValue * gravityScale * Time.fixedDeltaTime;
    }

    private void UpdateTimers()
    {
        if (isAtApex)
        {
            apexTimer += Time.deltaTime;
            if (apexTimer >= jumpSettings.JumpApexHoldTime)
            {
                isAtApex = false;
                apexTimer = 0;
            }
        }
    }

    enum GroundRayDirections
    {
        Left,
        Right,
        Center
    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;

        float width = col.bounds.size.x * jumpSettings.RayWidth;

        var leftHit = CastGroundRay(width, GroundRayDirections.Left).collider;
        var rightHit = CastGroundRay(width, GroundRayDirections.Right).collider;
        var centerHit = CastGroundRay(width, GroundRayDirections.Center).collider;

        isGrounded = leftHit || rightHit || centerHit;

        if (wasGrounded != isGrounded)
        {
            PlayerEvent.GroundedChanged(isGrounded);
        }

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            isJumping = false;
            isFalling = false;
            jumpCancelled = false;
        }
    }

    private RaycastHit2D CastGroundRay(float width, GroundRayDirections direction)
    {
        RaycastHit2D hit;
        Vector3 offset = Vector3.zero;

        switch (direction)
        {
            case GroundRayDirections.Left:
                offset = -new Vector3(width / 2, 0);
                break;

            case GroundRayDirections.Right:
                offset = new Vector3(width / 2, 0);
                break;

            case GroundRayDirections.Center:
                offset = Vector3.zero;
                break;
        }

        hit = Physics2D.Raycast(rb.transform.position + offset, Vector2.down, jumpSettings.GroundCheckDistance, jumpSettings.GroundLayer);
        return hit;
    }

    public void DrawGizmos()
    {
        if (!Application.isPlaying) return;

        float width = col.bounds.size.x * jumpSettings.RayWidth;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.transform.position - new Vector3(width / 2, 0),
                        rb.transform.position - new Vector3(width / 2, 0) + Vector3.down * jumpSettings.GroundCheckDistance);
        Gizmos.DrawLine(rb.transform.position,
                         rb.transform.position + Vector3.down * jumpSettings.GroundCheckDistance);
        Gizmos.DrawLine(rb.transform.position + new Vector3(width / 2, 0),
                         rb.transform.position + new Vector3(width / 2, 0) + Vector3.down * jumpSettings.GroundCheckDistance);
    }
}