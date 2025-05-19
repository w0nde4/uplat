using UnityEngine;

public abstract class AttackStrategy : ScriptableObject
{
    [Header("Базовые настройки атаки")]
    [SerializeField] protected int baseDamage = 10;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform customAttackOrigin;

    protected GameObject owner;
    protected Transform attackOrigin;
    protected bool isAttacking;

    public virtual void Initialize(GameObject owner)
    {
        isAttacking = false;
        this.owner = owner;

        if (customAttackOrigin != null)
        {
            attackOrigin = customAttackOrigin;
        }
        else
        {
            var attackPoint = owner.transform.Find("AttackPoint");
            attackOrigin = attackPoint != null ? attackPoint : owner.transform;
        }
    }

    public virtual void TryPerformAttack()
    {
        if (isAttacking || owner == null) return;
        PerformAttack();
    }

    protected virtual void PerformAttack()
    {
        isAttacking = true;
    }

    public virtual void UpdateStrategy(float deltaTime) { }

    public virtual void OnAttackEnd()
    {
        isAttacking = false;
    }

    public virtual void ApplyDamageToTargets()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            attackOrigin.position,
            attackRange,
            enemyLayer
        );

        foreach (var hitCollider in hitColliders)
        {
            ApplyDamageToTarget(hitCollider.gameObject, CalculateDamage());
        }
    }

    protected virtual void ApplyDamageToTarget(GameObject target, int damage)
    {
        var damageable = target.GetComponent<IDamagable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, owner);
        }
        else
        {
            Debug.LogWarning($"Объект {target.name} не может получать урон (отсутствует компонент IDamageable)");
        }
    }

    protected virtual int CalculateDamage()
    {
        return baseDamage;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        if (attackOrigin == null) return;

        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(attackOrigin.position, Vector3.forward, attackRange);
    }
#endif
}
