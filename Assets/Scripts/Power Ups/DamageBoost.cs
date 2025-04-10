using UnityEngine;

[CreateAssetMenu(fileName = "DamageBoost", menuName = "PowerUp/DamageBoost")]
public class DamageBoost : PowerUp
{
    [SerializeField] private float damageMultiplier = 1.5f;

    public override void Use(Player player)
    {
        var attack = player.GetComponent<PlayerAttack>();
        if (attack == null) return;

        attack.DamageMultiplier = damageMultiplier;
    }
}
