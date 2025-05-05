using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int max = 100;

    private int current;

    public int Max => max;
    public int Current => current;

    public event Action<int, int> OnChanged;

    private void Awake()
    {
        current = max;
    }

    private void Update()
    {
        if (Debug.isDebugBuild && CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                Decrease(10);
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Increase(10);
            }
        }
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
