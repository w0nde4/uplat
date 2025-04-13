using UnityEngine;

[CreateAssetMenu(fileName = "DamageBoost", menuName = "PowerUps/Passive/DamageBoost")]
public class DamageBoost : PowerUp
{
    [SerializeField] private float damageMultiplier = 1.5f;

    private float startDamageMultiplier = 1f;

    public override void ApplyEffect(Player player)
    {
        var attack = GetComponent<PlayerAttack>(player);
        if (attack == null) return;

        ModifyProperty(attack,
            a => a.DamageMultiplier,
            (a, val) => a.DamageMultiplier = val,
            damageMultiplier,
            ref startDamageMultiplier);
    }

    public override void RemoveEffect(Player player)
    {
        var attack = GetComponent<PlayerAttack>(player);
        if (attack == null) return;

        attack.DamageMultiplier = startDamageMultiplier;
    }
}
