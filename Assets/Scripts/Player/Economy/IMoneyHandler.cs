using UnityEngine;

public interface IMoneyHandler
{
    void AddMoney(int value);
    bool TryPurchase(int price);
    void RefundMoney(int amount);
}
