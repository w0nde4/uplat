using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegen", menuName = "PowerUps/Passive/HealthRegen")]
public class HealthRegen : PowerUp
{
    [Header("Health Regeneration Settings")]
    [SerializeField] private int healthPerSecond = 1;

    private Coroutine regenCoroutine;
    private Health health;

    public override void OnInitialize(Player player)
    {
        health = player.GetComponent<Health>();
    }

    public override void ApplyEffect(Player player)
    {
        if (health == null) health = player.GetComponent<Health>();
        if (health == null)
        {
            return;
        }

        if (regenCoroutine == null)
        {
            regenCoroutine = player.StartCoroutine(RegenerateHealth(player));
        }
    }

    public override void RemoveEffect(Player player)
    {
        if (regenCoroutine != null)
        {
            player.StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }
    }

    private IEnumerator RegenerateHealth(Player player)
    {
        if (health != null) health.Heal(healthPerSecond);
        yield return new WaitForSeconds(1f);
    }
}
