using System;
using UnityEngine;

public interface IInventory
{
    InventorySection PassiveSection { get; }
    InventorySection ActiveSection { get; }
    PowerUpData CurrentActivePowerUp { get; }

    event Action<PowerUpData> OnPowerUpAdded;
    event Action<PowerUpData> OnPowerUpRemoved;
    event Action<PowerUpData> OnPowerUpUsed;

    bool TryRecievePowerUp(PowerUpData powerUp);
    bool TryRemovePowerUp(PowerUpData powerUp);
    void UseActivePowerUp();
}
