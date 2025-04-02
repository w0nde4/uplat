using UnityEngine;

public abstract class AttackStrategy : ScriptableObject
{
    [SerializeField] protected int baseDamage = 10;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected LayerMask targetLayer;

    public abstract void PerformAttack(GameObject attacker, FSMState state, int comboStep);

    public virtual int CalculateDamage(FSMState state, int comboStep)
    {
        float comboMultiplier = 1.0f + (comboStep * 0.2f);
        return Mathf.RoundToInt(baseDamage * comboMultiplier);
    }

    public virtual void ApplyDamage(GameObject target, GameObject attacker, int damage)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();

        if (damagable != null && damagable.IsAlive())
        {
            Debug.Log(target.name + " получил " + damage + " урона.");
            damagable.TakeDamage(damage, attacker);
        }
    }

    public virtual Collider2D[] GetAttackedEnemies(Vector2 attackerPosition)
    {
        if (attackerPosition == null) return null;

        float attackRange = GetAttackRange();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackerPosition, attackRange, targetLayer);
        Debug.Log("Найдено врагов: " + hitEnemies.Length);
        return hitEnemies;
    }

    public virtual float GetAttackRange()
    {
        return attackRange;
    }
}
