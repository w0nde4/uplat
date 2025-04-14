using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    private float damagePerSecond;
    private float remainingDuration;
    private bool isInitialized = false;
    private GameObject damager;

    public void Initialize(float damage, float duration, GameObject source = null)
    {
        damagePerSecond = damage;
        remainingDuration = duration;
        damager = source;
        isInitialized = true;
    }

    public void RefreshDuration(float duration)
    {
        remainingDuration = Mathf.Max(remainingDuration, duration);
    }

    private void Update()
    {
        if (!isInitialized) return;

        remainingDuration -= Time.deltaTime;

        if (remainingDuration <= 0)
        {
            Destroy(this);
            return;
        }

        // Apply damage over time
        if (TryGetComponent<IDamagable>(out var damageable))
        {
            int damage = Mathf.RoundToInt(damagePerSecond * Time.deltaTime);
            damageable.TakeDamage(damage, damager);
        }
    }
}
