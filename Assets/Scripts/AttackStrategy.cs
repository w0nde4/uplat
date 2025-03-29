using UnityEngine;

public abstract class AttackStrategy : ScriptableObject
{
    public abstract void PerformAttack(FSMState state, int comboStep);
}
