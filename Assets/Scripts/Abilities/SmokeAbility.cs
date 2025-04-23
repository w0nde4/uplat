using System;
using UnityEngine;

public abstract class SmokeAbility : ScriptableObject, ISmokeUsable
{
    [Header("Smoke Properties")]
    [SerializeField] protected float smokeCost = 10f;
    [SerializeField] protected float cooldownTime = 1.5f;

    [Header("Visual Effects")]
    [SerializeField] protected GameObject smokeEffectPrefab;
    [SerializeField] protected Color smokeColor = Color.gray; //remove
    [SerializeField] protected float effectDuration = 1.0f;

    protected float lastUseTime = -100;

    public float CooldownTime => cooldownTime;
    public float SmokeCost => smokeCost;
    public float RemainingCooldown => Mathf.Max(0, (lastUseTime + cooldownTime) - Time.time);

    public event Action<GameObject, SmokeAbility, float> OnAbilityUsed;

    public virtual bool CanUse(float currentSmoke)
    {
        return currentSmoke >= smokeCost && Time.time > lastUseTime + cooldownTime;
    }

    public virtual float Use(GameObject user)
    {
        if (CanUse(GetSmokeAmount(user)))
        {
            lastUseTime = Time.time;
            PerformAbility(user);
            AbilityUsed(user, this, cooldownTime);
            return smokeCost;
        }
        return 0f;
    }

    public void AbilityUsed(GameObject user, SmokeAbility ability, float remainingCooldown)
    {
        OnAbilityUsed?.Invoke(user, ability, remainingCooldown);
    }

    public void ResetCooldown()
    {
        lastUseTime = -cooldownTime;
    }

    protected virtual void PerformAbility(GameObject user) //interface
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

    protected float GetSmokeAmount(GameObject user)
    {
        Smoke smoke = user.GetComponent<Smoke>();
        return smoke != null ? smoke.CurrentSmokeAmount : 0f;
    }
}
