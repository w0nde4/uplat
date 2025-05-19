using System;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    private readonly InventorySection passiveSection;
    private readonly InventorySection activeSection;
    private readonly MonoBehaviour coroutineRunner;

    private PowerUpData currentActivePowerUp;

    public InventorySection PassiveSection => passiveSection;
    public InventorySection ActiveSection => activeSection;
    public PowerUpData CurrentActivePowerUp => currentActivePowerUp;

    public event Action<PowerUpData> OnPowerUpAdded;
    public event Action<PowerUpData> OnPowerUpRemoved;
    public event Action<PowerUpData> OnPowerUpUsed;

    public Inventory(int maxPassiveSlots, int maxActiveSlots, MonoBehaviour coroutineRunner)
    {
        passiveSection = new InventorySection(maxActiveSlots);
        activeSection = new InventorySection(maxActiveSlots);
        this.coroutineRunner = coroutineRunner;

        Subscribe();
    }

    ~Inventory()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        passiveSection.OnAdded += HandlePowerUpAdded;
        passiveSection.OnRemoved += HandlePowerUpRemoved;

        activeSection.OnAdded += HandlePowerUpAdded;
        activeSection.OnRemoved += HandlePowerUpRemoved;

        activeSection.OnAdded += HandleActiveAdded;
        activeSection.OnRemoved += HandleActiveRemoved;
    }

    private void Unsubscribe()
    {
        passiveSection.OnAdded -= HandlePowerUpAdded;
        passiveSection.OnRemoved -= HandlePowerUpRemoved;

        activeSection.OnAdded -= HandlePowerUpAdded;
        activeSection.OnRemoved -= HandlePowerUpRemoved;

        activeSection.OnAdded -= HandleActiveAdded;
        activeSection.OnRemoved -= HandleActiveRemoved;
    }

    private void HandlePowerUpAdded(PowerUpData powerUp)
    {
        OnPowerUpAdded?.Invoke(powerUp);
    }

    private void HandlePowerUpRemoved(PowerUpData powerUp)
    {
        OnPowerUpRemoved?.Invoke(powerUp);
    }

    private void HandleActiveAdded(PowerUpData powerUp)
    {
        if (currentActivePowerUp == null)
            currentActivePowerUp = powerUp;
    }

    private void HandleActiveRemoved(PowerUpData powerUp)
    {
        if (currentActivePowerUp == powerUp)
            currentActivePowerUp = activeSection.Items.Count > 0 ? activeSection.Items[0] : null;
    }

    public bool TryRecievePowerUp(PowerUpData powerUp)
    {
        var section = powerUp is IPassivePowerUp ? passiveSection : activeSection;
        
        if (section.IsFull)
            return false;

        return section.TryAdd(powerUp);
    }

    public void UseActivePowerUp()
    {
        if (currentActivePowerUp == null) return;

        if(currentActivePowerUp is IActivePowerUp activePowerUp)
        {
            if(activePowerUp.TryUse(coroutineRunner))
            {
                OnPowerUpUsed?.Invoke(currentActivePowerUp);
            }
        }
    }

    public bool TryRemovePowerUp(PowerUpData powerUp)
    {
        bool removed = false;

        switch (powerUp is IPassivePowerUp)
        {
            case true:
                removed = passiveSection.TryRemove(powerUp);
                break;

            case false:
                removed = activeSection.TryRemove(powerUp);
                break;
        }

        if (removed)
        {
            if (currentActivePowerUp == powerUp) currentActivePowerUp = null;
            OnPowerUpRemoved?.Invoke(powerUp);
        }

        return removed;
    }
}
