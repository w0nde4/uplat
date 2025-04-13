using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySection passiveSection = new InventorySection();
    [SerializeField] private InventorySection activeSection = new InventorySection();

    private PowerUp currentActivePowerUp;
    private Player player;

    public event Action<PowerUp> OnPowerUpAdded;
    public event Action<PowerUp> OnPowerUpRemoved;
    public event Action<PowerUp> OnPowerUpUsed;

    public InventorySection PassiveSection => passiveSection;
    public InventorySection ActiveSection => activeSection;
    public PowerUp CurrentActivePowerUp => currentActivePowerUp;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        passiveSection.OnAdded += OnPowerUpAdded;
        passiveSection.OnRemoved += OnPowerUpRemoved;

        activeSection.OnAdded += OnPowerUpAdded;
        activeSection.OnRemoved += OnPowerUpRemoved;

        activeSection.OnAdded += HandleActiveAdded;
        activeSection.OnRemoved += HandleActiveRemoved;
    }

    private void OnDisable()
    {
        passiveSection.OnAdded -= OnPowerUpAdded;
        passiveSection.OnRemoved -= OnPowerUpRemoved;

        activeSection.OnAdded -= OnPowerUpAdded;
        activeSection.OnRemoved -= OnPowerUpRemoved;

        activeSection.OnAdded -= HandleActiveAdded;
        activeSection.OnRemoved -= HandleActiveRemoved;
    }

    private void HandleActiveAdded(PowerUp powerUp)
    {
        if (currentActivePowerUp == null)
            currentActivePowerUp = powerUp;
    }

    private void HandleActiveRemoved(PowerUp powerUp)
    {
        if (currentActivePowerUp == powerUp)
            currentActivePowerUp = activeSection.Items.Count > 0 ? activeSection.Items[0] : null;
    }

    public bool TryRecieve(PowerUp powerUp)
    {
        var section = powerUp.IsPassive? passiveSection : activeSection;
        
        if (section.IsFull)
            return false;

        return section.TryAdd(powerUp, player);
    }

    public void UseActivePowerUp()
    {
        if (currentActivePowerUp == null) return;

        currentActivePowerUp.TryUse(player);
        OnPowerUpUsed?.Invoke(currentActivePowerUp);
    }

    public bool RemovePowerUp(PowerUp powerUp)
    {
        bool removed = false;

        if (powerUp.IsPassive)
        {
            removed = passiveSection.Remove(powerUp, player);
        }
        else
        {
            removed = activeSection.Remove(powerUp, player);

            if (removed && currentActivePowerUp == powerUp)
            {
                currentActivePowerUp = null;
            }
        }

        if (removed)
        {
            OnPowerUpRemoved?.Invoke(powerUp);
        }

        return removed;
    }

    public IReadOnlyList<PowerUp> GetAllPowerUps()
    {
        List<PowerUp> all = new List<PowerUp>();
        all.AddRange(passiveSection.Items);
        all.AddRange(activeSection.Items);
        return all.AsReadOnly();
    }

    public bool ContainsPowerUp(PowerUp powerUp)
    {
        return passiveSection.Contains(powerUp) || activeSection.Contains(powerUp);
    }
}
