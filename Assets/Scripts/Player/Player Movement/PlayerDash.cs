using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash Properties")]
    [SerializeField] private float force = 24f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private bool allowAirDash = false;

    [Header("Physics Properties")]
    [SerializeField] private float gravityMultiplierDuringDash = 0.1f;
    [SerializeField] private float verticalVelocityRetention = 0.5f;
    [SerializeField] private float endDashVelocityMultiplier = 0.5f;

    [Header("Collision Detection")]
    [SerializeField] private float obstacleDetectionDistance = 0.5f;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private float originalGravityScale;

    private bool isDashing = false;
    private bool isGrounded = true;

    private float lastDashTime;
    private Vector2 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    private void OnEnable()
    {
        PlayerEvent.OnGroundedChanged += HandleGrounded;
    }

    private void OnDisable()
    {
        PlayerEvent.OnGroundedChanged -= HandleGrounded;
    }

    private void Update()
    {
        if (CanDash() && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(PerformDash());
        }
    }

    private bool CanDash()
    {
        return Time.time > lastDashTime + cooldown &&
            !isDashing &&
            (isGrounded || allowAirDash);
    }

    private IEnumerator PerformDash()
    {
        PrepareForDash();
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + duration && !IsObstacleAhead())
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

        PlayerEvent.DashChanged(isDashing);

        dashDirection = GetDashDirection();
        rb.gravityScale = gravityMultiplierDuringDash * originalGravityScale;

        ApplyDashForce();
    }

    private void ApplyDashForce()
    {
        rb.linearVelocity = new Vector2(dashDirection.x * force, rb.linearVelocity.y * verticalVelocityRetention);
    }

    private bool IsObstacleAhead()
    {
        return Physics2D.Raycast(transform.position, dashDirection, obstacleDetectionDistance, obstacleLayer);
    }

    private void EndDash()
    {
        isDashing = false;
        PlayerEvent.DashChanged(isDashing);

        rb.gravityScale = originalGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * endDashVelocityMultiplier, rb.linearVelocity.y);
    }

    private Vector2 GetDashDirection()
    {
        if (TryGetComponent(out IDirectionable directionable))
        {
            return directionable.GetFacingDirection();
        }
        return transform.right;
    }

    private void HandleGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    private void OnDrawGizmos()
    {
        if (isDashing)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, dashDirection * 0.5f);
        }
    }
}