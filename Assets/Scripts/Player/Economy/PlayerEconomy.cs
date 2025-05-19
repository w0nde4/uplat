using UnityEngine;

public class PlayerEconomy : MonoBehaviour, IPowerUpConsumer, IMoneyHandler
{
    [SerializeField] private int maxActiveSlots = 1;
    [SerializeField] private int maxPassiveSlots = 9;
    [SerializeField] private int startingMoney = 100;

    private Wallet wallet;
    private Inventory inventory;

    public Wallet Wallet => wallet ??= new Wallet(startingMoney);
    public Inventory Inventory => inventory ??= new Inventory(maxPassiveSlots, maxActiveSlots, this);

    private void Awake()
    {
        _ = Wallet;
        _ = Inventory;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseActivePowerUp();
        }
    }

    public void UseActivePowerUp()
    {
        inventory.UseActivePowerUp(); 
    }

    public bool TryPurchase(int price)
    {
        return wallet.TrySpend(price);
    }

    public void AddMoney(int value)
    {
        wallet.AddMoney(value);
    }

    public void RefundMoney(int amount)
    {
        wallet.AddMoney(amount);
    }

    public bool TryAcquirePowerUp(PowerUpData powerUp)
    {
        return inventory.TryRecievePowerUp(powerUp);
    }
}
