using UnityEngine;

public class WorldPowerUp : MonoBehaviour
{
    PowerUpInstance powerUpInstance;

    private void Awake()
    {
        powerUpInstance = GetComponent<PowerUpInstance>();
        powerUpInstance.OnInteracted += HandleInteraction;
    }

    private void OnDestroy()
    {
        if(powerUpInstance != null)
        {
            powerUpInstance.OnInteracted -= HandleInteraction;
        }
    }

    private void HandleInteraction(PowerUpInstance instance)
    {
        Player player = FindObjectOfType<Player>();

        if (player != null && player.Inventory != null)
        {
            if(player.Inventory.AddPowerUp(instance.PowerUpData))
            {
                Destroy(gameObject);
            }
        }
    }
}
