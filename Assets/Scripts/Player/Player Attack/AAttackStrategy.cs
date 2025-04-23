using UnityEngine;

[CreateAssetMenu(fileName = "AAttack", menuName = "Attack Strategies/A")]
public class AAttackStrategy : AttackStrategy //combo interface
{
    [SerializeField] private int maxComboStep = 3;
    [SerializeField] private float comboResetTime = 1.5f;
    
    private int currentComboStep = 0;
    private float timeSinceLastAttack = 0f;

    public override bool SupportsCombo() => true;

    public override void PerformAttack(GameObject attacker)
    {
        var playerAttack = attacker.GetComponent<PlayerAttack>();
        var damageMultiplier = playerAttack != null ? playerAttack.DamageMultiplier : 1f;
        var damage = CalculateDamage(damageMultiplier);

        var attackerPosition = attacker.transform.position;

        foreach(var enemy in GetAttackedEnemies(attackerPosition))
        {
            ApplyDamage(enemy.gameObject, attacker, damage);
        }

        OnComboPerformed();
        timeSinceLastAttack = 0f;
    }

    public override int CalculateDamage(float damageMultiplier)
    {
        float comboMult = 1.0f + (currentComboStep * 0.2f);
        return Mathf.RoundToInt(baseDamage * comboMult * damageMultiplier);
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

    public override void UpdateStrategy(float deltaTime) //interface
    {
        timeSinceLastAttack += deltaTime;
        if(timeSinceLastAttack >= comboResetTime && currentComboStep != 0)
        {
            OnComboReset();
            PlayerEvent.AttackEnded();
        }
    }
}