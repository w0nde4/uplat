using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "PowerUp/HealthBoost")]
public class HealthBoost : PowerUp
{
    [SerializeField] private int healthAmount = 25;

    private int startHealth;
    public override void Use(Player player)
    {
        var health = player.GetComponent<Health>();
        health.Heal(healthAmount);
        Debug.Log("Boosted by" +  healthAmount);
    }

    public override void RemoveEffect(Player player)
    {
        Debug.Log("have to deboost health but not");
    }
}
