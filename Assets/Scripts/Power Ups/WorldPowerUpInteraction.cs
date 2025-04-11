using UnityEngine;

public class WorldPowerUpInteraction : IPowerUpInteractionStrategy
{
    public void Interact(PowerUpInstance powerUpInstance, Player player)
    {
        if(player.Inventory.AddPowerUp(powerUpInstance.PowerUpData))
        {
            GameObject.Destroy(powerUpInstance.gameObject);
        }
    }
}
