using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Base class for all smoke-powered abilities
/// Inherits from the abstract Ability class
/// </summary>
public abstract class SmokeAbility : ScriptableObject
{
    [Header("Smoke Properties")]
    [SerializeField] protected float smokeCost = 10f;
    [SerializeField] protected float cooldownTime = 1.5f;

    [Header("Visual Effects")]
    [SerializeField] protected GameObject smokeEffectPrefab;
    [SerializeField] protected Color smokeColor = Color.gray;
    [SerializeField] protected float effectDuration = 1.0f;

    protected float lastUseTime = -100;

    /// <summary>
    /// Checks if the ability can be used based on available smoke and cooldown
    /// </summary>
    /// <param name="currentSmoke">Current amount of smoke available</param>
    /// <returns>True if the ability can be used</returns>
    public virtual bool CanUse(float currentSmoke)
    {
        return currentSmoke >= smokeCost && Time.time > lastUseTime + cooldownTime;
    }

    /// <summary>
    /// Uses the ability and returns the amount of smoke consumed
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    /// <returns>Amount of smoke consumed</returns>
    public virtual float Use(GameObject user)
    {
        if (CanUse(GetSmokeAmount(user)))
        {
            lastUseTime = Time.time;
            PerformAbility(user);
            return smokeCost;
        }
        return 0f;
    }

    public void ResetCooldown()
    {
        lastUseTime = -cooldownTime;
    }


    /// <summary>
    /// Implemented by derived classes to perform the specific ability effect
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    protected virtual void PerformAbility(GameObject user)
    {
        if (smokeEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                smokeEffectPrefab,
                user.transform.position,
                Quaternion.identity
            );

            if (effect.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = smokeColor;
            }

            Destroy(effect, effectDuration);
        }
    }
    /// <summary>
    /// Gets the current smoke amount from the user
    /// </summary>
    /// <param name="user">GameObject with Smoke component</param>
    /// <returns>Current smoke amount or 0 if Smoke component not found</returns>
    protected float GetSmokeAmount(GameObject user)
    {
        Smoke smoke = user.GetComponent<Smoke>();
        return smoke != null ? smoke.CurrentSmokeAmount : 0f;
    }

    public float CooldownTime => cooldownTime;

    public float SmokeCost => smokeCost;

    public float RemainingCooldown => Mathf.Max(0, (lastUseTime + cooldownTime) - Time.time);
}
