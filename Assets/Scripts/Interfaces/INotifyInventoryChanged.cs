using System;
using UnityEngine;

public interface INotifyInventoryChanged
{
    event Action<PowerUpInstance> OnPowerUpAdded;
    event Action<PowerUpInstance> OnPowerUpRemoved;
}
