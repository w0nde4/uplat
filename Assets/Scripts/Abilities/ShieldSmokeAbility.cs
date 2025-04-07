using UnityEngine;

[CreateAssetMenu(fileName = "Smoke Shield", menuName = "Abilities/Smoke Shield")]
public class ShieldSmokeAbility : SmokeAbility
{
    [Header("Shield Properties")]
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private float damageReduction = 0.5f; // 50% damage reduction
    [SerializeField] private float shieldRadius = 1.5f;
    [SerializeField] private float rotationSpeed = 90f;

    protected override void PerformAbility(GameObject user)
    {
        base.PerformAbility(user);

        // Add shield effect
        ShieldSmokeEffect shieldEffect = user.GetComponent<ShieldSmokeEffect>();

        if (shieldEffect == null)
        {
            shieldEffect = user.AddComponent<ShieldSmokeEffect>();
        }

        shieldEffect.ActivateShield(
            shieldDuration,
            damageReduction,
            shieldRadius,
            rotationSpeed,
            smokeEffectPrefab,
            smokeColor
        );
    }
}
