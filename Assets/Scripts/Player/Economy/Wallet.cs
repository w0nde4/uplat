using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int startingMoney = 100;

    private int currentMoney;
    public int Currency => currentMoney;

    public event Action<int> OnMoneyChanged; 

    private void Awake()
    {
        currentMoney = startingMoney;
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return false;

        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            OnMoneyChanged?.Invoke(currentMoney);
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        if(amount <= 0) return;

        currentMoney += amount;
        OnMoneyChanged?.Invoke(Currency);
    }
}
