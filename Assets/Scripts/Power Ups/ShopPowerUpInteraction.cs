using UnityEngine;

public class ShopPowerUpInteraction : IPowerUpInteractionStrategy
{
    private Shop.ShopItem item;
    private Shop shop;

    public ShopPowerUpInteraction(Shop.ShopItem item, Shop shop)
    {
        this.item = item;
        this.shop = shop;
    }

    public void Interact(PowerUpInstance powerUpInstance, Player player)
    {
        if (player.Wallet.TrySpend(item.price))
        {
            if (player.TryAcquirePowerUp(item.powerUp))
            {
                shop.NotifyItemSold(item);
                GameObject.Destroy(powerUpInstance.gameObject);
            }
            else
            {
                player.Wallet.AddMoney(item.price);
                Debug.Log("Inventory full!");
            }
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
