using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "PowerUp/HealthBoost")]
public class HealthBoost : PowerUp
{
    [SerializeField] private int healthAmount = 25;

    public override void Use(Player player)
    {
        var health = player.GetComponent<Health>(); //player stats
        health.Heal(healthAmount);
        Debug.Log("Boosted by" +  healthAmount);
    }
}
