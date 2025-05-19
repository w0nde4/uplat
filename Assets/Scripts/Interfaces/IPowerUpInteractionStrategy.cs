using UnityEngine;

public interface IPowerUpInteractionStrategy
{
    void Interact(PowerUpInstance powerUpInstance, IMoneyHandler moneyHandler, IPowerUpConsumer powerUpConsumer);
}
