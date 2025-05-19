using System;
using UnityEngine;

public class Wallet
{
    private int currentMoney;
    public int Currency => currentMoney;

    public event Action<int> OnMoneyChanged; 

    public Wallet(int startingMoney)
    {
        currentMoney = Mathf.Max(startingMoney, 0);
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
