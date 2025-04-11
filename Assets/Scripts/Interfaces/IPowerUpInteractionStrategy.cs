using UnityEngine;

public interface IPowerUpInteractionStrategy
{
    void Interact(PowerUpInstance powerUpInstance, Player player);
}
