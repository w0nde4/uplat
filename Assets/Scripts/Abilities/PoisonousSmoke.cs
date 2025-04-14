using UnityEngine;

[CreateAssetMenu(fileName = "PoisonousSmokeAbility", menuName = "Abilities/Passive/PoisonousSmoke")]
public class PoisonousSmoke : PassiveSmokeAbility
{
    [Header("Poison Properties")]
    [SerializeField] private float smokeThreshold = 0.8f; // 80% smoke threshold
    [SerializeField] private float damagePerSecond = 5f;
    [SerializeField] private float effectRadius = 3f;
    [SerializeField] private float poisonDuration = 2.5f; // Duration of lingering poison effect
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float poisonApplicationInterval = 0.5f;

    private float timeSinceLastPoison = 0f;

    public override bool CheckActivationCondition(GameObject user)
    {
        // Check if smoke level is at or above threshold
        if (user.TryGetComponent<Smoke>(out var smoke))
        {
            return smoke.CurrentSmokeAmount >= smoke.MaximumSmokeAmount * smokeThreshold;
        }
        return false;
    }

    protected override void ApplyPassiveEffect(GameObject user, float deltaTime)
    {
        timeSinceLastPoison += deltaTime;

        if(timeSinceLastPoison >= poisonApplicationInterval)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(user.transform.position, effectRadius, enemyLayer);
            if (hitColliders.Length > 0)
            {
                Debug.Log($"Found {hitColliders.Length} enemies in poisonous smoke radius");

                foreach (var hitCollider in hitColliders)
                {
                    GameObject enemy = hitCollider.gameObject;
                    Debug.Log($"Applying poison effect to: {enemy.name}");

                    if (hitCollider.TryGetComponent<IDamagable>(out var damageable))
                    {
                        int directDamage = Mathf.RoundToInt(damagePerSecond * poisonApplicationInterval);
                        Debug.Log($"Direct damage to {enemy.name}: {directDamage}");
                        damageable.TakeDamage(directDamage, user);
                    }

                    // Add or refresh the poison effect component for persistent damage
                    if (!enemy.TryGetComponent<PoisonEffect>(out var existingPoison))
                    {
                        PoisonEffect poisonEffect = enemy.AddComponent<PoisonEffect>();
                        poisonEffect.Initialize(damagePerSecond, poisonDuration, user);
                        Debug.Log($"Added new poison effect to {enemy.name} for {poisonDuration}s");
                    }
                    else
                    {
                        // Only refresh the duration, keeping the damage consistent
                        existingPoison.RefreshDuration(poisonDuration);
                        Debug.Log($"Refreshed poison duration on {enemy.name}");
                    }
                }
            }

            timeSinceLastPoison = 0;
        }
    }

    protected override void ActivatePassiveEffect(GameObject user)
    {
        base.ActivatePassiveEffect(user);
        Debug.Log("Poisonous Smoke activated - enemies will take poison damage!");
        timeSinceLastPoison = poisonApplicationInterval;
    }

    protected override void DeactivatePassiveEffect(GameObject user)
    {
        base.DeactivatePassiveEffect(user);
        Debug.Log("Poisonous Smoke deactivated");
    }
}
