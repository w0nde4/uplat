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
    public bool IsActive => isActive;

    public abstract bool CheckActivationCondition(GameObject user);

    public virtual void Activate(GameObject user)
    {
        if (!isActive)
        {
            isActive = true;
            OnPassiveStateChanged?.Invoke(user, this, true);
            ActivatePassiveEffect(user);
        }
    }

    public virtual void Deactivate(GameObject user)
    {
        if (isActive)
        {
            isActive = false;
            OnPassiveStateChanged?.Invoke(user, this, false);
            DeactivatePassiveEffect(user);
        }
    }

    public virtual void UpdatePassiveEffect(GameObject user, float deltaTime)
    {
        bool conditionMet = CheckActivationCondition(user);

        if(conditionMet && !isActive) Activate(user);
        
        else if (!conditionMet && isActive) Deactivate(user);

        if (isActive) ApplyPassiveEffect(user, deltaTime);
    }

    protected abstract void ActivatePassiveEffect(GameObject user);

    protected abstract void DeactivatePassiveEffect(GameObject user);

    protected abstract void ApplyPassiveEffect(GameObject user, float deltaTime);
}
