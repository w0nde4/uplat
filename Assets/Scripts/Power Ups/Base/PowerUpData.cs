using UnityEngine;
using UnityEngine.Events;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public abstract class PowerUpData : ScriptableObject
{
    [Header("Basic Information")]
    [SerializeField] private string powerUpName;
    [SerializeField][TextArea] private string powerUpDescription;
    [SerializeField] private Sprite icon;
    [SerializeField] private Rarity rarity;
    [SerializeField] private GameObject instancePrefab;
    [SerializeField] private int baseCost;

    [Header("Events")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    public string PowerUpName => powerUpName;
    public string PowerUpDescription => powerUpDescription;
    public GameObject InstancePrefab => instancePrefab;
    public int BaseCost => baseCost;
    public Rarity RarityType => rarity;
    public Sprite Icon => icon;

    public abstract void ApplyEffect();
    public abstract void RemoveEffect();

    public virtual void OnAcquired() { }
    public virtual void OnRemoved() { }
}
