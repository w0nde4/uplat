using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour //separate spawn and storage
{
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

        public override bool Equals(object obj)
        {
            if (obj is ShopItem other)
                return powerUp == other.powerUp && price == other.price;

            return false;
        }

        public override int GetHashCode()
        {
            return powerUp.GetHashCode() ^ price.GetHashCode();
        }
    }

    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();
    
    private int shopItemsCount;

    public event Action<ShopItem> OnItemSold;

    public IReadOnlyList<ShopItem> Items => shopItems;

    

    private void Awake()
    {
        shopItemsCount = shopItems.Count;

        SpawnItems();
    }

    private void SpawnItems()
    {
        foreach (var item in shopItems)
        {
            var prefab = item.powerUp.InstancePrefab;
            if (prefab == null)
            {
                Debug.LogError($"No prefab set for {item.powerUp.name}");
                continue;
            }

            var instanceGO = Instantiate(prefab, GetSpawnPosition(shopItems.IndexOf(item)), Quaternion.identity);
            var instance = instanceGO.GetComponent<PowerUpInstance>();

            if (instance == null)
            {
                Debug.LogError("PowerUp prefab must have PowerUpInstance component");
                continue;
            }

            instance.Init(item.powerUp, new ShopPowerUpInteraction(item, this));
        }
    }

    private Vector3 GetSpawnPosition(int index)
    {
        float spawnDistance = transform.localScale.x / (shopItemsCount + 1);

        float spawnPointX = transform.position.x - transform.localScale.x / 2 + spawnDistance * (index + 1);

        float spawnPointY = transform.position.y - transform.localScale.y / 2;

        return new Vector3(spawnPointX, spawnPointY, 0);
    }

    public void NotifyItemSold(ShopItem item)
    {
        shopItems.Remove(item);
        OnItemSold?.Invoke(item);
    }
}
