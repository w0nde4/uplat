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
    [SerializeField] private bool isPassive;
    [SerializeField] private float cooldownTime;
    [SerializeField] private int baseCost;
    [SerializeField] private Rarity rarity;
    [SerializeField] private Sprite icon;

    public string PowerUpName => powerUpName;
    public string PowerUpDescription => powerUpDescription;
    public bool IsPassive => isPassive;
    public float Cooldown => cooldownTime;
    public int BaseCost => baseCost;
    public Rarity RarityType => rarity;
    public Sprite Icon => icon;

    public abstract void Use(Player player);

    public virtual void OnAcquired(Player player)
    {
        Debug.Log($"{player.name} acquired {powerUpName}");
    }

    public virtual void OnRemoved(Player player)
    {
        Debug.Log($"{player.name} removed {powerUpName}");
    }
}
