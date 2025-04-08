using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoost", menuName = "PowerUp/HealthBoost")]
public class HealthBoost : PowerUp
{
    [SerializeField] private int bonusHealth = 50;

    public override void Use(GameObject user)
    {
        var health = user.GetComponent<Health>();
        if (health == null) return;

        health.Heal(bonusHealth);
        Debug.Log($"{user.name} received {bonusHealth} bonus health.");
    }

    public override void PickUp(Player player)
    {
        base.PickUp(player);
        Use(player.gameObject);
    }
}
