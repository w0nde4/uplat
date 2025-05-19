using System;

public interface IHealth
{
    public int Max { get; }
    public int Current { get; }

    public event Action<int, int> OnChanged;

    public void Increase(int value);
    public void Decrease(int value);
}
