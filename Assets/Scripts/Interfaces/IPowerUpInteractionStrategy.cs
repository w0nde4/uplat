using UnityEngine;

public interface IPowerUpInteractionStrategy
{
    void Interact(PowerUpInstance powerUpInstance, MonoBehaviour interactor);
}
