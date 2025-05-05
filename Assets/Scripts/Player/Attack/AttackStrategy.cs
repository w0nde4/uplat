using UnityEngine;

public abstract class AttackStrategy : ScriptableObject
{
    [SerializeField] protected int baseDamage = 10;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected LayerMask targetLayer;

    public abstract void PerformAttack(GameObject attacker);

    public virtual int CalculateDamage(float damageMultiplier)
    {
        return Mathf.RoundToInt(baseDamage * damageMultiplier);
    }

    public virtual void ApplyDamage(GameObject target, GameObject attacker, int damage)
    {
        if(target.TryGetComponent(out IDamagable damagable))
        {
            Debug.Log(target.name + " получил " + damage + " урона.");
            damagable.TakeDamage(damage, attacker);
        }
    }

    public virtual Collider2D[] GetAttackedEnemies(Vector2 attackPosition)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, attackRange, targetLayer);
        Debug.Log("Найдено врагов: " + hitEnemies.Length);
        return hitEnemies;
    }

    public virtual bool SupportsCombo() => false; //interface
    public virtual void OnComboReset() { }
    public virtual void OnComboPerformed() { }
    public virtual int GetComboStep() => 0;
    public virtual void UpdateStrategy(float deltaTime) { }
}
