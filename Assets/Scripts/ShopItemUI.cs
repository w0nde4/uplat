using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button purchaseButton;

    private Shop.ShopItem item;
    private Shop shop;
    private Player player;

    public void Setup(Shop.ShopItem item, Shop shop, Player player)
    {
        this.item = item;
        this.shop = shop;
        this.player = player;

        if (iconImage != null)
            iconImage.sprite = item.powerUp.Icon;

        if (nameText != null)
            nameText.text = item.powerUp.PowerUpName;

        if (priceText != null)
            priceText.text = item.price + " coins";

        if (purchaseButton != null)
            purchaseButton.onClick.AddListener(TryPurchase);

        UpdateInteractability();
    }

    private void TryPurchase()
    {
        if(shop.TryPurchase(item, player))
        {
            gameObject.SetActive(false);
        }
        else
        {
            UpdateInteractability();
        }
    }

    private void UpdateInteractability()
    {
        if (purchaseButton != null && player != null && player.Wallet)
        {
            bool canAfford = player.Wallet.Currency >= item.price;
            bool hasSpace = !player.Inventory.IsFull;
            purchaseButton.interactable = canAfford && hasSpace;
        }
    }

    private void OnDestroy()
    {
        if (purchaseButton != null)
            purchaseButton.onClick.RemoveListener(TryPurchase);
    }
}
