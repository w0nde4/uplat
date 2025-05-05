using UnityEngine;

public class WorldPowerUpInteraction : IPowerUpInteractionStrategy
{
    public void Interact(PowerUpInstance powerUpInstance, MonoBehaviour interactor)
    {
        PowerUpData powerUp = powerUpInstance.PowerUpData;

        if(interactor.TryGetComponent(out PlayerInventoryWallet player))
        {
            player.TryAcquirePowerUp(powerUp);
            GameObject.Destroy(powerUpInstance.gameObject);
        }
    }
}
