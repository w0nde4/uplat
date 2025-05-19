using UnityEngine;

public class WorldPowerUpInteraction : IPowerUpInteractionStrategy
{
    public void Interact(PowerUpInstance powerUpInstance, IMoneyHandler moneyHandler, IPowerUpConsumer powerUpConsumer)
    {
        PowerUpData powerUp = powerUpInstance.PowerUpData;

        if(moneyHandler != null && powerUpConsumer != null)
        {
            if(powerUpConsumer.TryAcquirePowerUp(powerUp)) GameObject.Destroy(powerUpInstance.gameObject);
            else Debug.Log("Inventory full!");
        }
    }
}
