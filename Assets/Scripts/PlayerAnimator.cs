using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private readonly string isDashing = "IsDashing";
    private readonly string isGrounded = "IsGrounded";
    private readonly string attackTrigger = "AttackTrigger";
    private readonly string comboStep = "ComboStep";

    private PlayerAttack playerAttack;

    private int currentComboStep = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        PlayerEvent.OnAttackStart += HandleAttackStart;
        PlayerEvent.OnAttackEnd += HandleAttackEnd;
        PlayerEvent.OnDashChanged += HandleDashChanged;
        PlayerEvent.OnGroundedChanged += HandleGroundedChanged;
    }

    private void OnDisable()
    {
        PlayerEvent.OnAttackStart -= HandleAttackStart;
        PlayerEvent.OnAttackEnd -= HandleAttackEnd;
        PlayerEvent.OnDashChanged -= HandleDashChanged;
        PlayerEvent.OnGroundedChanged -= HandleGroundedChanged;
    }

    private void Update()
    {
        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        float verticalVelocity = rb.linearVelocity.y;
        Flip(rb.linearVelocity.x);

        animator.SetFloat("HorizontalVelocity", horizontalSpeed);
        animator.SetFloat("VerticalVelocity", verticalVelocity);
    }

    private void Flip(float horizontalVelocity)
    {
        if (Mathf.Abs(horizontalVelocity) > 0.01f)
        {
            sr.flipX = horizontalVelocity < 0;
        }
    }

    private void HandleAttackStart(int currentComboStep)
    {
        this.currentComboStep = currentComboStep;
        animator.SetInteger(comboStep, currentComboStep);
        animator.SetTrigger(attackTrigger);
    }

    private void HandleAttackEnd()
    {
        currentComboStep = 0;
        animator.SetInteger(comboStep, currentComboStep);
    }

    private void HandleDashChanged(bool isDashing)
    {
        animator.SetBool(this.isDashing, isDashing);
    }

    private void HandleGroundedChanged(bool isGrounded)
    {
        animator.SetBool(this.isGrounded, isGrounded);
    }

    public void OnAttackAnimationHit() // ???
    {
        Debug.Log("Attack hit frame reached in animation");
    }
}
