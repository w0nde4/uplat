using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpTime = 0.35f; 
    [SerializeField] private float coyoteTime = 0.15f; 
    [SerializeField] private float jumpBufferTime = 0.2f; 

    [Header("Gravity Settings")]
    [SerializeField] private float gravityScale = 6f;
    [SerializeField] private float fallGravityMultiplier = 1.5f; 
    [SerializeField] private float apexGravityMultiplier = 0.5f; 
    [SerializeField] private float apexVelocityThreshold = 0.1f;
    [SerializeField] private float fallingVelocityThreshold = -0.5f;

    [Header("Jump Cancel")]
    [SerializeField] private float cancelRate = 100f; 

    [Header("Fall Settings")]
    [SerializeField] private float fallClamp = 10f;

    [Header("Ground Check")]
    [SerializeField] private float rayWidth;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f; 

    private Rigidbody2D rb;

    private bool isJumping;
    private bool isJumpCancelled;
    private bool isGrounded;
    private bool isDashing;

    private float jumpTimeCounter;
    private float lastGroundedTime;
    private float lastJumpPressedTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    private void OnEnable()
    {
        PlayerEvent.OnDashChanged += HandleDash;
    }

    private void OnDisable()
    {
        PlayerEvent.OnDashChanged -= HandleDash;
    }

    void Update()
    {
        if(!isDashing)
        {
            CheckGround();
            HandleJumpInput();
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            ApplyJump();
            ApplyFallPhysics();
            ApplyApexModifiers();
        }
    }

    private void HandleDash(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;
        
        float width = GetComponent<Collider2D>().bounds.size.x * rayWidth;

        var leftHit = CastGroundRay(width, GroundRayDirections.Left).collider;
        var rightHit = CastGroundRay(width, GroundRayDirections.Right).collider;
        var centerHit = CastGroundRay(width, GroundRayDirections.Center).collider;

        isGrounded = leftHit != null || rightHit != null || centerHit != null;

        if (wasGrounded != isGrounded)
        {
            PlayerEvent.GroundedChanged(isGrounded);
        }

        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            isJumping = false;
        }
    }

    enum GroundRayDirections
    {
        Left,
        Right,
        Center
    }

    private RaycastHit2D CastGroundRay(float width, GroundRayDirections direction)
    {
        RaycastHit2D hit;
        Vector3 offset = Vector3.zero; 

        switch(direction)
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

        hit = Physics2D.Raycast(transform.position + offset, Vector2.down, groundCheckDistance, groundLayer);
        return hit;
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastJumpPressedTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumpCancelled = true;
        }
    }

    private void ApplyJump()
    {
        float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        if (Time.time - lastGroundedTime <= coyoteTime && Time.time - lastJumpPressedTime <= jumpBufferTime)
        {
            PerformJump(jumpForce);
            lastJumpPressedTime = 0;
        }

        if (isJumping && jumpTimeCounter > 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpTimeCounter -= Time.fixedDeltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    private void PerformJump(float jumpForce)
    {
        isJumpCancelled = false;
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        isJumping = true;
        jumpTimeCounter = jumpTime;
    }

    private void ApplyFallPhysics()
    {
        if (rb.linearVelocity.y < fallingVelocityThreshold)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else if (isJumpCancelled && isJumping)
        {
            rb.gravityScale = gravityScale * cancelRate * Time.deltaTime;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

        if (rb.linearVelocity.y < -fallClamp)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fallClamp);
        }
    }

    private void ApplyApexModifiers()
    {
        if (isJumping && Mathf.Abs(rb.linearVelocity.y) < apexVelocityThreshold)
        {
            rb.gravityScale = gravityScale * apexGravityMultiplier;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down + new Vector3(rayWidth/2,0) * groundCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down - new Vector3(rayWidth / 2, 0) * groundCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
