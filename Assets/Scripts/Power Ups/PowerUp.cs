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
    [SerializeField] private int baseCost;
    [SerializeField] private Rarity rarity;
    [SerializeField] private Sprite icon;

    private bool isEffectActive = false; 

    public string PowerUpName => powerUpName;
    public string PowerUpDescription => powerUpDescription;
    public GameObject InstancePrefab => instancePrefab;
    public bool IsPassive => isPassive;
    public float Cooldown => cooldownTime;
    public int BaseCost => baseCost;
    public Rarity RarityType => rarity;
    public Sprite Icon => icon;
    public string EffectName => powerUpName + " effect";

    public abstract void Use(Player player);

    public abstract void RemoveEffect(Player player);

    public virtual void OnAcquired(Player player)
    {
        if (isPassive)
        {
            Use(player);
            isEffectActive = true;
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
}
