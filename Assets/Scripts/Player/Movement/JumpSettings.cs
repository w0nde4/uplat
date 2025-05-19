using UnityEngine;

[CreateAssetMenu(fileName = "JumpSettings", menuName = "Player/Jump Settings")]
public class JumpSettings : ScriptableObject
{
    [Header("Jump Settings")]
    [SerializeField] private float maxJumpHeight = 4f;
    [SerializeField] private float jumpTimeToApex = 0.4f;
    [SerializeField] private float jumpApexHoldTime = 0.1f;
    [SerializeField] private float maxFallSpeed = -20f;

    [Header("Special Jump Features")]
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpCancelGravityMultiplier = 2.5f;

    [Header("Gravity Settings")]
    [SerializeField] private float jumpGravityMultiplier = 0.5f;
    [SerializeField] private float fallGravityMultiplier = 2.0f;

    [Header("Ground Check")]
    [SerializeField] private float rayWidth = 0.8f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    public float MaxJumpHeight => maxJumpHeight;
    public float JumpTimeToApex => jumpTimeToApex;
    public float JumpApexHoldTime => jumpApexHoldTime;
    public float MaxFallSpeed => maxFallSpeed;
    public float CoyoteTime => coyoteTime;
    public float JumpBufferTime => jumpBufferTime;
    public float JumpCancelGravityMultiplier => jumpCancelGravityMultiplier;
    public float JumpGravityMultiplier => jumpGravityMultiplier;
    public float FallGravityMultiplier => fallGravityMultiplier;
    public float RayWidth => rayWidth;
    public float GroundCheckDistance => groundCheckDistance;
    public LayerMask GroundLayer => groundLayer;
}
