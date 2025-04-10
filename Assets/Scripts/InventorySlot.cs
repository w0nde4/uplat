using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private UnityEngine.UI.Text nameText;
    [SerializeField] private Button useButton;

    private PowerUp powerUpData;
    private PowerUpInstance powerUpInstance;
    private Player player;

    public void SetPowerUp(PowerUp powerUp, PowerUpInstance instance, Player player)
    {
        this.powerUpData = powerUp;
        this.powerUpInstance = instance;
        this.player = player;

        // Update UI
        if (iconImage != null)
        {
            // Try to use a sprite from the powerUpData if available
            // This assumes your PowerUp has a sprite or icon field
            Sprite icon = null;

            // Try to get sprite from instance if it exists
            if (instance != null)
            {
                SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    icon = renderer.sprite;
                }
            }

            iconImage.sprite = icon;
            iconImage.gameObject.SetActive(icon != null);
        }

        if (nameText != null)
            nameText.text = powerUp.PowerUpName;

        if (useButton != null)
        {
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(UsePowerUp);
            useButton.gameObject.SetActive(true);
        }
    }

    public void Clear()
    {
        powerUpData = null;
        powerUpInstance = null;
        player = null;

        // Update UI
        if (iconImage != null)
            iconImage.gameObject.SetActive(false);

        if (nameText != null)
            nameText.text = "Empty";

        if (useButton != null)
            useButton.gameObject.SetActive(false);
    }

    private void UsePowerUp()
    {
        if (powerUpData != null && player != null)
        {
            // Use the power up via the PowerUpData.Use() method
            powerUpData.Use(player);

            // For non-passive power-ups, remove after use
            if (!powerUpData.IsPassive)
            {
                if (powerUpInstance != null)
                {
                    player.GetComponent<Inventory>().RemovePowerUp(powerUpInstance.PowerUpData);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (useButton != null)
            useButton.onClick.RemoveAllListeners();
    }
}
