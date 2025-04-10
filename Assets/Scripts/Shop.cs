using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();

    public event Action<ShopItem> OnItemSold;

    [Serializable]
    public class ShopItem
    {
        public PowerUp powerUp;
        public int price;

        public ShopItem(PowerUp powerUp, int price)
        { 
            this.powerUp = powerUp; 
            this.price = price; 
        }
    }
    
    public IReadOnlyCollection<ShopItem> Items => shopItems;

    public bool TryPurchase(ShopItem item, Player player)
    {
        if (!shopItems.Contains(item)) return false;

        if (player.Wallet != null && player.Inventory != null)
        {
            if (player.Wallet.TrySpend(item.price))
            {
                if (player.Inventory.AddPowerUp(item.powerUp))
                {
                    shopItems.Remove(item);
                    OnItemSold?.Invoke(item);
                    return true;
                }
                else
                {
                    player.Wallet.AddMoney(item.price);
                    Debug.Log("Inventory is full!");
                    return false;
                }
            }
            else
            {
                Debug.Log("Not enough money!");
                return false;
            }
        }

        return false;
    }

    public bool TryPurchase(int itemIndex, Player player)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Count)
        {
            return TryPurchase(shopItems[itemIndex], player);
        }
        return false;
    }
}
