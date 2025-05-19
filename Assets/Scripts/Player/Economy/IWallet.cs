using System;

public interface IWallet
{
    int Currency { get; }
    event Action<int> OnMoneyChanged;

    bool TrySpend(int amount);
    void AddMoney(int amount);
}
