using UnityEngine;
using UnityEngine.UI;

public class ActiveInventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnPowerUpAdded += HandlePowerUpAdded;
            inventory.OnPowerUpRemoved += HandlePowerUpRemoved;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnPowerUpAdded -= HandlePowerUpAdded;
            inventory.OnPowerUpRemoved -= HandlePowerUpRemoved;
        }
    }

    private void HandlePowerUpAdded(PowerUpData powerUpData)
    {
        if (powerUpData is IActivePowerUp)
        {
            AddPowerUpIcon(powerUpData);
        }
    }

    private void HandlePowerUpRemoved(PowerUpData powerUpData)
    {
        
    }

    private void AddPowerUpIcon(PowerUpData powerUp)
    {
        if (powerUp == null || powerUp.Icon == null) return;

        GameObject iconObj = new GameObject(powerUp.PowerUpName + "_Active_Icon");
        iconObj.transform.SetParent(transform, false);
        
        Image containerImage = iconObj.AddComponent<Image>();
        containerImage.color = new Color(0, 0, 0, 1);
        iconObj.AddComponent<Mask>().showMaskGraphic = false;

        GameObject iconImageObj = new GameObject("Icon");
        iconImageObj.transform.SetParent(iconObj.transform, false);

        RectTransform iconRect = iconImageObj.AddComponent<RectTransform>();
        iconRect.anchorMin = Vector2.zero;
        iconRect.anchorMax = Vector2.one;
        iconRect.offsetMin = Vector2.zero;
        iconRect.offsetMax = Vector2.zero;

        Image iconImage = iconImageObj.AddComponent<Image>();
        iconImage.sprite = powerUp.Icon;
    }
}
