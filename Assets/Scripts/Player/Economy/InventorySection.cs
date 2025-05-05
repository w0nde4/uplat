using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class InventorySection
{
    [SerializeField] private int maxSlots;
    private readonly List<PowerUpData> items = new List<PowerUpData>();

    public event Action<PowerUpData> OnAdded;
    public event Action<PowerUpData> OnRemoved;

    public IReadOnlyList<PowerUpData> Items => items.AsReadOnly();
    public int MaxSlots => maxSlots;
    public bool IsFull => items.Count >= maxSlots;
    public int Count => items.Count;

    public bool Contains(PowerUpData powerUp) => items.Contains(powerUp);

    public bool TryAdd(PowerUpData powerUp, PlayerInventoryWallet player)
    {
        if (IsFull) return false;

        items.Add(powerUp);
        powerUp.OnAcquired();
        OnAdded?.Invoke(powerUp);
        return true;
    }

    public bool TryRemove(PowerUpData powerUp, PlayerInventoryWallet player)
    {
        if (items.Remove(powerUp))
        {
            powerUp.OnRemoved();
            OnRemoved?.Invoke(powerUp);
            return true;
        }
        return false;
    }
}
