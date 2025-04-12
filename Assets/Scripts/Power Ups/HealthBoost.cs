using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "PowerUp/HealthBoost")]
public class HealthBoost : PowerUp
{
    [SerializeField] private int healthAmount = 25;

    private float startMultiplier;

    public override void ApplyEffect(Player player)
    {
        var health = GetComponent<Health>(player);
        if (health == null) return;

        startMultiplier = health.MaxHealthMultiplier;
        float multiplier = (healthAmount + health.MaxHealth) / (float)health.MaxHealth;

        ModifyProperty(health,
            h => h.MaxHealthMultiplier,
            (h, val) => h.MaxHealthMultiplier = val,
            multiplier,
            ref startMultiplier);
    }

    public override void RemoveEffect(Player player)
    {
        var health = GetComponent<Health>(player);
        if (health == null) return;

        health.MaxHealthMultiplier = startMultiplier;
    }
}
