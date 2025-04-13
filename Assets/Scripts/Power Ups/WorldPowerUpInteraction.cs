using UnityEngine;

public class WorldPowerUpInteraction : IPowerUpInteractionStrategy
{
    public void Interact(PowerUpInstance powerUpInstance, Player player)
    {
        PowerUp powerUp = powerUpInstance.PowerUpData;

        if(player.TryAcquirePowerUp(powerUp))
        {
            GameObject.Destroy(powerUpInstance.gameObject);
        }
    }
}
