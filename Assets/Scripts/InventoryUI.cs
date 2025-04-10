using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GameObject slotPrefab;

    private Player player;
    private Inventory inventory;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();

            if (inventory != null)
            {
                // Subscribe to inventory events
                // Note: These events need to be added to your Inventory class if they don't exist
                if (inventory is INotifyInventoryChanged notifier)
                {
                    notifier.OnPowerUpAdded += RefreshUI;
                    notifier.OnPowerUpRemoved += RefreshUI;
                }

                // Initial setup
                SetupSlots();
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events if they exist
        if (inventory is INotifyInventoryChanged notifier)
        {
            notifier.OnPowerUpAdded -= RefreshUI;
            notifier.OnPowerUpRemoved -= RefreshUI;
        }
    }

    private void SetupSlots()
    {
        // Clear existing slots
        foreach (Transform child in slotsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create slots based on the current inventory state
        var powerUpInstances = inventory.GetAllPowerUps();
        int maxSlots = inventory.MaxSlots; // Use the field directly if property doesn't exist

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();

            // Setup slot
            if (i < powerUpInstances.Count)
            {
                slot.SetPowerUp(powerUpInstances[i].PowerUpData, powerUpInstances[i], player);
            }
            else
            {
                slot.Clear();
            }
        }
    }

    private void RefreshUI(PowerUpInstance _)
    {
        SetupSlots();
    }
}
