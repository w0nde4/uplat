using System;
using UnityEngine;

public abstract class PassiveSmokeAbility : SmokeAbility, IPassiveSmokeAbility
{
    [Header("Passive Properties")]
    [SerializeField] protected GameObject passiveEffectPrefab;
    [SerializeField] protected Color effectColor = Color.gray;

    protected bool isActive = false;
    protected GameObject activeEffect;

    public event Action<GameObject, PassiveSmokeAbility, bool> OnPassiveStateChanged;

    /// <summary>
    /// Current active state of the passive ability
    /// </summary>
    public bool IsActive => isActive;

    /// <summary>
    /// Checks if the activation condition is met
    /// </summary>
    /// <param name="user">GameObject to check conditions for</param>
    /// <returns>True if activation conditions are met</returns>
    public abstract bool CheckActivationCondition(GameObject user);

    /// <summary>
    /// Activates the passive ability
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    public virtual void Activate(GameObject user)
    {
        if (!isActive)
        {
            isActive = true;
            OnPassiveStateChanged?.Invoke(user, this, true);
            ActivatePassiveEffect(user);
        }
    }

    /// <summary>
    /// Deactivates the passive ability
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    public virtual void Deactivate(GameObject user)
    {
        if (isActive)
        {
            isActive = false;
            OnPassiveStateChanged?.Invoke(user, this, false);
            DeactivatePassiveEffect(user);
        }
    }

    /// <summary>
    /// Updates the passive effect and returns smoke cost for this frame
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    /// <param name="deltaTime">Time since last frame</param>
    public virtual void UpdatePassiveEffect(GameObject user, float deltaTime)
    {
        bool conditionMet = CheckActivationCondition(user);

        if(conditionMet && !isActive) Activate(user);
        
        else if (!conditionMet && isActive) Deactivate(user);

        if (isActive) ApplyPassiveEffect(user, deltaTime);
    }

    /// <summary>
    /// Implemented by derived classes to activate the specific passive effect
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    protected virtual void ActivatePassiveEffect(GameObject user)
    {
        if (passiveEffectPrefab != null)
        {
            activeEffect = Instantiate(
                passiveEffectPrefab,
                user.transform.position,
                Quaternion.identity,
                user.transform
            );

            if (activeEffect.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = smokeColor;
            }
        }
    }

    /// <summary>
    /// Implemented by derived classes to deactivate the specific passive effect
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    protected virtual void DeactivatePassiveEffect(GameObject user)
    {
        if (activeEffect != null)
        {
            Destroy(activeEffect);
            activeEffect = null;
        }
    }

    /// <summary>
    /// Implemented by derived classes to apply the ongoing passive effect
    /// </summary>
    /// <param name="user">GameObject that is using the ability</param>
    /// <param name="deltaTime">Time since last frame</param>
    protected abstract void ApplyPassiveEffect(GameObject user, float deltaTime);
}
