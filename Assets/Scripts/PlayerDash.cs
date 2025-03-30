using System;
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
    private Vector2 direction;

    private void OnEnable()
    { 
        PlayerEvent.OnGroundedChanged += HandleGrounded;    
    }

    private void OnDisable()
    {
        PlayerEvent.OnGroundedChanged -= HandleGrounded;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && 
            Time.time > lastDashTime + cooldown && !isDashing &&
            (isGrounded || allowAirDash))
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        float horizontalVelocity = rb.linearVelocity.x;
        float startTime = Time.time;
        direction = horizontalVelocity < 0 ? Vector2.left : Vector2.right;

        isDashing = true;
        PlayerEvent.DashChanged(isDashing);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = gravityMultiplierDuringDash * originalGravity;

        rb.linearVelocity = new Vector2(direction.x * force, rb.linearVelocity.y * verticalVelocityRetention);

        while (Time.time < startTime + duration && isDashing)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayer);

            if (hit.collider != null)
            {
                break;
            }

            rb.linearVelocity = new Vector2(direction.x * force, rb.linearVelocity.y * verticalVelocityRetention);

            yield return null;
        }

        isDashing = false;
        PlayerEvent.DashChanged(isDashing);

        rb.gravityScale = originalGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * endDashVelocityMultiplier, rb.linearVelocity.y);

        lastDashTime = Time.time;
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
            Gizmos.DrawRay(transform.position, direction * 0.5f);
        }
    }
}