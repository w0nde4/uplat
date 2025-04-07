using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AAttack", menuName = "Attack Strategies/A")]
public class AAttackStrategy : AttackStrategy
{
    [SerializeField] private int maxComboStep = 3;
    [SerializeField] private float comboResetTime = 1.5f;
    
    private int currentComboStep = 0;
    private float timeSinceLastAttack = 0f;

    public override bool SupportsCombo() => true;

    public override void PerformAttack(GameObject attacker)
    {
        var damage = CalculateDamage();
        var attackerPosition = attacker.transform.position;

        foreach(var enemy in GetAttackedEnemies(attackerPosition))
        {
            ApplyDamage(enemy.gameObject, attacker, damage);
        }

        OnComboPerformed();
        timeSinceLastAttack = 0f;
    }

    public override int CalculateDamage()
    {
        float comboMult = 1.0f + (currentComboStep * 0.2f);
        return Mathf.RoundToInt(baseDamage * comboMult);
    }
    public override void OnComboPerformed()
    {
        PlayerEvent.AttackStarted(currentComboStep);
        currentComboStep = (currentComboStep + 1) % maxComboStep;
    }

    public override void OnComboReset()
    {
        currentComboStep = 0;
    }

    public override int GetComboStep() => currentComboStep;

    public override void UpdateStrategy(float deltaTime)
    {
        timeSinceLastAttack += deltaTime;
        if(timeSinceLastAttack >= comboResetTime && currentComboStep != 0)
        {
            OnComboReset();
            PlayerEvent.AttackEnded();
        }
    }
}