using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInventoryWallet))]
public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySection passiveSection = new InventorySection();
    [SerializeField] private InventorySection activeSection = new InventorySection();

    private PowerUpData currentActivePowerUp;
    private PlayerInventoryWallet player;
    private MonoBehaviour coroutineRunner;

    public InventorySection PassiveSection => passiveSection;
    public InventorySection ActiveSection => activeSection;
    public PowerUpData CurrentActivePowerUp => currentActivePowerUp;

    public event Action<PowerUpData> OnPowerUpAdded;
    public event Action<PowerUpData> OnPowerUpRemoved;
    public event Action<PowerUpData> OnPowerUpUsed;

    private void Awake()
    {
        player = GetComponent<PlayerInventoryWallet>();
        coroutineRunner = this;
    }

    private void OnEnable()
    {
        passiveSection.OnAdded += HandlePowerUpAdded;
        passiveSection.OnRemoved += HandlePowerUpRemoved;

        activeSection.OnAdded += HandlePowerUpAdded;
        activeSection.OnRemoved += HandlePowerUpRemoved;

        activeSection.OnAdded += HandleActiveAdded;
        activeSection.OnRemoved += HandleActiveRemoved;
    }

    private void OnDisable()
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

    public bool TryRecieve(PowerUpData powerUp)
    {
        var section = powerUp is IPassivePowerUp ? passiveSection : activeSection;
        
        if (section.IsFull)
            return false;

        return section.TryAdd(powerUp, player);
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
                removed = passiveSection.TryRemove(powerUp, player);
                break;

            case false:
                removed = activeSection.TryRemove(powerUp, player);
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
