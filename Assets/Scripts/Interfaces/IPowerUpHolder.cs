using UnityEngine;

public interface IPowerUpHolder
{
    void AddPowerUp(PowerUpInstance instance);
    void RemovePowerUp(PowerUpInstance instance);
    bool ContainsPowerUp(PowerUp powerUp);
}
