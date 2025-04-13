using UnityEngine;

public class Player : MonoBehaviour //rename
{
    private Inventory inventory;
    private PlayerWallet wallet;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        wallet = GetComponent<PlayerWallet>();
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
