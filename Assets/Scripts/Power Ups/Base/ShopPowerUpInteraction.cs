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

    public void Interact(PowerUpInstance powerUpInstance, IMoneyHandler moneyHandler, IPowerUpConsumer powerUpConsumer)
    {
        if(moneyHandler != null && powerUpConsumer != null)
        {
            if (moneyHandler.TryPurchase(item.price))
            {
                if (powerUpConsumer.TryAcquirePowerUp(item.powerUp))
                {
                    shop.NotifyItemSold(item);
                    GameObject.Destroy(powerUpInstance.gameObject);
                }
                else
                {
                    moneyHandler.RefundMoney(item.price);
                    Debug.Log("Inventory full!");
                }
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
    }
}
