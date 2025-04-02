using UnityEngine;

[CreateAssetMenu(fileName = "AAttack", menuName = "Attack Strategies/A")]
public class AAttackStrategy : AttackStrategy
{
    [SerializeField] private float highHpDamageMult = 1.2f;
    [SerializeField] private float lowHpDamageMult = 0.8f;
    public override void PerformAttack(GameObject attacker, FSMState state, int comboStep)
    {
        var damage = CalculateDamage(state, comboStep);
        var attackerPosition = attacker.transform.position;
        foreach(var enemy in GetAttackedEnemies(attackerPosition))
        {
            ApplyDamage(enemy.gameObject, attacker, damage);
        }
    }

    public override int CalculateDamage(FSMState state, int comboStep)
    {
        float stateMult = 1.0f;

        switch(state)
        {
            case HighHPState:
                stateMult = highHpDamageMult;
                break;
            case LowHPState:
                stateMult = lowHpDamageMult;
                break;
        }

        float comboMult = 1.0f + (comboStep * 0.2f);
        return Mathf.RoundToInt(baseDamage * stateMult * comboMult);
    }
}