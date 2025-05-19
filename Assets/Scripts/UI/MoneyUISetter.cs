using TMPro;
using UnityEngine;

public class MoneyUISetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextAmount;
    [SerializeField] private PlayerEconomy player;

    private Wallet wallet;

    private void OnEnable()
    {
        if (player != null)
        {
            wallet = player.Wallet;
            if (wallet != null)
            {
                wallet.OnMoneyChanged += UpdateCurrency;
                UpdateCurrency(wallet.Currency);
            }
            else Debug.LogError("Player's Wallet is null!");
        }
        else Debug.LogError("Player reference is null in MoneyUISetter!");
    }

    private void OnDisable()
    {
        if (wallet != null) wallet.OnMoneyChanged -= UpdateCurrency;
    }

    public void UpdateCurrency(int currency)
    {
        if (moneyTextAmount != null)
        {
            moneyTextAmount.text = currency.ToString();
        }
    }
}
