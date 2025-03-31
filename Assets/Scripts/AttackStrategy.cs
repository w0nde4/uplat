using UnityEngine;

public abstract class AttackStrategy : ScriptableObject
{
    [SerializeField] protected int baseDamage = 10;
    [SerializeField] protected float attackRange = 1.5f;

    public abstract void PerformAttack(FSMState state, int comboStep);

    public virtual int CalculateDamage(FSMState state, int comboStep)
    {
        float comboMultiplier = 1.0f + (comboStep * 0.2f);
        return Mathf.RoundToInt(baseDamage * comboMultiplier);
    }

    public virtual float GetAttackRange()
    {
        return attackRange;
    }
}
