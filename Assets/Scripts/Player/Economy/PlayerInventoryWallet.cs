using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Wallet))]
public class PlayerInventoryWallet : MonoBehaviour
{
    private Inventory inventory;
    private Wallet wallet;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        wallet = GetComponent<Wallet>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.UseActivePowerUp();
        }
    }

    public bool TryPurchasePowerUp(Shop.ShopItem item)
    {
        if (wallet == null) return false;
        return wallet.TrySpend(item.price);
    }

    public void GiveMoneyBack(Shop.ShopItem item)
    {
        if (wallet == null) return;
        wallet.AddMoney(item.price);

    }

    public bool TryAcquirePowerUp(PowerUp powerUp)
    {
        if (inventory == null) return false;
        return inventory.TryRecieve(powerUp);
    }
}
