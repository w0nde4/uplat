using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public abstract class PowerUp : ScriptableObject, IUsable
{
    [SerializeField] private string powerUpName;
    [SerializeField][TextArea] private string powerUpDescription;
    [SerializeField] private bool isPassive;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float baseCost;
    [SerializeField] private Rarity rarity;

    public string PowerUpName => powerUpName;
    public string PowerUpDescription => powerUpDescription;
    public bool IsPassive => isPassive;
    public float Cooldown => cooldownTime;
    public float BaseCost => baseCost;
    public Rarity RarityType => rarity;


    public abstract void Use(GameObject user);

    public virtual void PickUp(Player player)
    {
        Debug.Log($"{player.name} picked up {powerUpName}");
    }

    public virtual void Drop(Player player)
    {
        Debug.Log($"{player.name} dropped {powerUpName}");
    }
}
