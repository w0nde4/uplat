using System.Collections.Generic;
using System;
using UnityEngine;

public class InventorySection
{
    private int maxSlots;
    private readonly List<PowerUpData> items = new List<PowerUpData>();

    public event Action<PowerUpData> OnAdded;
    public event Action<PowerUpData> OnRemoved;

    public IReadOnlyList<PowerUpData> Items => items.AsReadOnly();
    public int MaxSlots => maxSlots;
    public bool IsFull => items.Count >= maxSlots;
    public int Count => items.Count;

    public InventorySection(int maxSlots)
    {
        this.maxSlots = Mathf.Max(maxSlots, 1);
    }

    public bool Contains(PowerUpData powerUp) => items.Contains(powerUp);

    public bool TryAdd(PowerUpData powerUp)
    {
        if (IsFull) return false;

        items.Add(powerUp);
        powerUp.OnAcquired();
        OnAdded?.Invoke(powerUp);
        return true;
    }

    public bool TryRemove(PowerUpData powerUp)
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
