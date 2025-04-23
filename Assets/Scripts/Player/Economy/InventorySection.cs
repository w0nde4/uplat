using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class InventorySection
{
    [SerializeField] private int maxSlots;
    private readonly List<PowerUp> items = new List<PowerUp>();

    public event Action<PowerUp> OnAdded;
    public event Action<PowerUp> OnRemoved;

    public IReadOnlyList<PowerUp> Items => items.AsReadOnly();
    public int MaxSlots => maxSlots;
    public bool IsFull => items.Count >= maxSlots;
    public int Count => items.Count;

    public bool Contains(PowerUp powerUp) => items.Contains(powerUp);

    public bool TryAdd(PowerUp powerUp, PlayerInventoryWallet player)
    {
        if (IsFull) return false;

        items.Add(powerUp);
        powerUp.OnAcquired(player);
        OnAdded?.Invoke(powerUp);
        return true;
    }

    public bool TryRemove(PowerUp powerUp, PlayerInventoryWallet player)
    {
        if (items.Remove(powerUp))
        {
            powerUp.OnRemoved(player);
            OnRemoved?.Invoke(powerUp);
            return true;
        }
        return false;
    }
}
