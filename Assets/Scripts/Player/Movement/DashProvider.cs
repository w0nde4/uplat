using System.Collections;
using UnityEngine;

public class DashProvider
{
    private readonly Rigidbody2D rb;
    private readonly IPlayerInput input;
    private readonly DashSettings settings;
    private readonly IDirectionable direction;
    
    private float originalGravityScale;
    private bool isDashing = false;
    private bool isGrounded = true;
    private float lastDashTime;
    private Vector2 dashDirection;
    private Coroutine dashCoroutine;
    private MonoBehaviour coroutineRunner;
    private Collider2D collider;

    public DashProvider(
        Rigidbody2D rb, 
        IPlayerInput input, 
        DashSettings settings, 
        IDirectionable directionable, 
        MonoBehaviour monoBehaviour,
        Collider2D collider
        )
    {
        this.rb = rb;
        this.input = input;
        this.settings = settings;
        this.direction = directionable;
        this.coroutineRunner = monoBehaviour;
        this.collider = collider;

        originalGravityScale = rb.gravityScale;

        PlayerEvent.OnGroundedChanged += HandleGrounded;
    }

    ~DashProvider()
    {
        PlayerEvent.OnGroundedChanged -= HandleGrounded;
    }

    public void Update()
    {
        if (CanDash() && input.DashPressed)
        {
            dashCoroutine = coroutineRunner.StartCoroutine(PerformDash());
        }
    }

    private bool CanDash()
    {
        return Time.time > lastDashTime + settings.Cooldown &&
            !isDashing &&
            (isGrounded || settings.AllowAirDash);
    }

    private IEnumerator PerformDash()
    {
        PrepareForDash();
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + settings.Duration && !IsObstacleAhead())
        {
            ApplyDashForce();
            yield return null;
        }

        EndDash();
    }

    private void PrepareForDash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        collider.isTrigger = true;

        PlayerEvent.DashChanged(isDashing);

        dashDirection = direction.GetFacingDirection();
        rb.gravityScale = settings.GravityMultiplierDuringDash * originalGravityScale;

        ApplyDashForce();
    }

    private void ApplyDashForce()
    {
        rb.linearVelocity = new Vector2(dashDirection.x * settings.Force, rb.linearVelocity.y * settings.VerticalVelocityRetention);
    }

    private bool IsObstacleAhead()
    {
        return Physics2D.Raycast(rb.transform.position, dashDirection, settings.ObstacleDetectionDistance, settings.ObstacleLayer);
    }

    private void EndDash()
    {
        isDashing = false;
        collider.isTrigger = false;

        PlayerEvent.DashChanged(isDashing);

        rb.gravityScale = originalGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * settings.EndDashVelocityMultiplier, rb.linearVelocity.y);
    }

    private void HandleGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    public void DrawGizmos()
    {
        if (isDashing)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(rb.transform.position, dashDirection * 0.5f);
        }
    }
}