using System;
using System.Collections;
using UnityEngine;

public class Health
{
    private int max;
    private int current;

    public int Max => max;
    public int Current => current;

    public event Action<int, int> OnChanged;

    public Health(int max)
    {
        this.max = Mathf.Max(max, 1);
        current = this.max;
    }

    public void Increase(int value)
    {
        current = Mathf.Min(current + value, max);
        OnChanged?.Invoke(current, max);
    }

    public void Decrease(int value)
    {
        current = Mathf.Clamp(current - value, 0, max);
        OnChanged?.Invoke(current, max);
    }
}
