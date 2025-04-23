using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInventoryWallet))]
public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySection passiveSection = new InventorySection();
    [SerializeField] private InventorySection activeSection = new InventorySection();

    private PowerUp currentActivePowerUp;
    private PlayerInventoryWallet player;

    public InventorySection PassiveSection => passiveSection;
    public InventorySection ActiveSection => activeSection;
    public PowerUp CurrentActivePowerUp => currentActivePowerUp;

    public event Action<PowerUp> OnPowerUpAdded;
    public event Action<PowerUp> OnPowerUpRemoved;
    public event Action<PowerUp> OnPowerUpUsed;

    private void Awake()
    {
        player = GetComponent<PlayerInventoryWallet>();
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

    public bool TryRemovePowerUp(PowerUp powerUp)
    {
        bool removed = false;

        switch (powerUp.IsPassive)
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
