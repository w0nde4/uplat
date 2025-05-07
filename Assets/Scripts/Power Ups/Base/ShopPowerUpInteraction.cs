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

    public void Interact(PowerUpInstance powerUpInstance, MonoBehaviour interactor)
    {
        if(interactor.TryGetComponent(out PlayerInventoryWallet player))
        {
            if (player.TryPurchasePowerUp(item))
            {
                if (player.TryAcquirePowerUp(item.powerUp))
                {
                    shop.NotifyItemSold(item);
                    GameObject.Destroy(powerUpInstance.gameObject);
                }
                else
                {
                    player.GiveMoneyBack(item);
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
