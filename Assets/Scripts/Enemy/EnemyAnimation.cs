using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DamageHandler))]
[RequireComponent(typeof(DeathHandler))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private DamageHandler damageHandler;
    private DeathHandler deathHandler;
    private EnemyAI enemyAI;

    private readonly string attackTrigger = "AttackTrigger";
    private readonly string deathTrigger = "DeathTrigger";
    private readonly string damagedTrigger = "DamagedTrigger";
    private readonly string isDead = "IsDead";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        damageHandler = GetComponent<DamageHandler>();
        deathHandler = GetComponent<DeathHandler>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void OnEnable()
    {
        EnemyEvent.OnAttackStart += HandleAttackStarted;
        deathHandler.OnDeath += HandleDeath;
        damageHandler.OnDamageRecieved += HandleDamageRecieved;
    }

    private void OnDisable()
    {
        EnemyEvent.OnAttackStart -= HandleAttackStarted;
        deathHandler.OnDeath -= HandleDeath;
        damageHandler.OnDamageRecieved -= HandleDamageRecieved;
    }

    private void Update()
    {
        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        Flip(rb.linearVelocity.x);

        animator.SetFloat("HorizontalVelocity", horizontalSpeed);
    }

    private void Flip(float horizontalVelocity)
    {
        var checkState = enemyAI.GetCurrentState() is AttackState;
        if (Mathf.Abs(horizontalVelocity) > 0.01f && !checkState)
        {
            sr.flipX = horizontalVelocity < 0;
        }
    }

    private void HandleDeath()
    {
        animator.SetTrigger(deathTrigger);
        animator.SetBool(isDead, true);
    }

    private void HandleDamageRecieved(GameObject obj)
    {
        animator.SetTrigger(damagedTrigger);
    }

    private void HandleAttackStarted()
    {
        animator.SetTrigger(attackTrigger);
    }
}
