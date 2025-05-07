using UnityEngine;

public class WorldPowerUpInteraction : IPowerUpInteractionStrategy
{
    public void Interact(PowerUpInstance powerUpInstance, MonoBehaviour interactor)
    {
        PowerUpData powerUp = powerUpInstance.PowerUpData;

        if(interactor.TryGetComponent(out PlayerInventoryWallet player))
        {
            if(player.TryAcquirePowerUp(powerUp)) GameObject.Destroy(powerUpInstance.gameObject);
            else Debug.Log("Inventory full!");
        }
    }
}
