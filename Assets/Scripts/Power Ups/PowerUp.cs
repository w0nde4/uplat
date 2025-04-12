using System;
using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public abstract class PowerUp : ScriptableObject
{
    [SerializeField] private string powerUpName;
    [SerializeField][TextArea] private string powerUpDescription;
    [SerializeField] private GameObject instancePrefab;
    [SerializeField] private bool isPassive;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float effectDuration;
    [SerializeField] private int baseCost;
    [SerializeField] private Rarity rarity;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool canStack;

    private bool isEffectActive = false; 

    public string PowerUpName => powerUpName;
    public string PowerUpDescription => powerUpDescription;
    public GameObject InstancePrefab => instancePrefab;
    public bool IsPassive => isPassive;
    public float Cooldown => cooldownTime;
    public float EffectDuration => effectDuration;
    public int BaseCost => baseCost;
    public Rarity RarityType => rarity;
    public Sprite Icon => icon;
    public string EffectName => powerUpName + " effect";
    public bool IsEffectActive => isEffectActive;

    public abstract void ApplyEffect(Player player);

    public abstract void RemoveEffect(Player player);
    protected T GetComponent<T>(Player player) where T : Component
    {
        if (player == null) return null;
        return player.GetComponent<T>();
    }

    protected void ModifyProperty<T, V>(T component, Func<T, V> getter, Action<T, V> setter, V newValue, ref V originalValue) where T : Component
    {
        if (component == null) return;

        originalValue = getter(component);
        setter(component, newValue);
    }

    public virtual void OnAcquired(Player player)
    {
        if (isPassive)
        {
            ApplyEffect(player);
            isEffectActive = true;
        }

        if (effectDuration > 0)
        {
            Debug.Log($"{powerUpName} will expire in {effectDuration} seconds");
        }

        Debug.Log($"{player.name} acquired {powerUpName}");
    }

    public virtual void OnRemoved(Player player)
    {
        if (isPassive && isEffectActive)
        {
            RemoveEffect(player);
            isEffectActive = false;
        }

        Debug.Log($"{player.name} removed {powerUpName}");
    }

    public virtual void Use(Player player)
    {
        if (isPassive) return;

        ApplyEffect(player);
        isEffectActive = true;

        if(effectDuration > 0)
        {
            Debug.Log($"{powerUpName} effect will expire in {effectDuration} seconds");
        }
    }
}
