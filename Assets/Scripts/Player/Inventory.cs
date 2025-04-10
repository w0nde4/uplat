using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 2;

    private List<PowerUp> powerUps = new List<PowerUp>();

    public event Action<PowerUp> OnPowerUpAdded;
    public event Action<PowerUp> OnPowerUpRemoved;
    public event Action<PowerUp> OnPowerUpUsed;

    public int MaxSlots => maxSlots;
    public int CurrentCount => powerUps.Count;
    public bool IsFull => powerUps.Count >= maxSlots;

    public IReadOnlyList<PowerUpInstance> GetAllPowerUps() => (IReadOnlyList<PowerUpInstance>)powerUps.AsReadOnly();

    public bool AddPowerUp(PowerUp powerUp)
    {
        if (IsFull)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        powerUps.Add(powerUp);
        powerUp.OnAcquired(GetComponent<Player>());
        OnPowerUpAdded?.Invoke(powerUp);
        return true;
    }

    public bool RemovePowerUp(PowerUp powerUp)
    {
        if (powerUps.Remove(powerUp))
        {
            powerUp.OnRemoved(GetComponent<Player>());
            OnPowerUpRemoved?.Invoke(powerUp);
            return true;
        }
        return false;
    }

    public void UsePowerUp(PowerUp powerUp)
    {
        if (powerUps.Contains(powerUp))
        {
            powerUp.Use(GetComponent<Player>());
            OnPowerUpUsed?.Invoke(powerUp);

            if (!powerUp.IsPassive)
            {
                RemovePowerUp(powerUp);
            }
        }
    }

    public bool ContainsPowerUp(PowerUp powerUp)
    {
        return powerUps.Contains(powerUp);
    }
}
