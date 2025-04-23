using TMPro;
using UnityEngine;

public class MoneyUISetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextAmount;
    [SerializeField] private PlayerInventoryWallet player;

    private Wallet playerWallet;

    private void Awake()
    {
        playerWallet = player.GetComponent<Wallet>();
    }

    private void Start()
    {
        if (playerWallet != null)
        {
            UpdateCurrency(playerWallet.Currency);
        }
    }

    private void OnEnable()
    {
        playerWallet.OnMoneyChanged += UpdateCurrency;
    }

    private void OnDisable()
    {
        playerWallet.OnMoneyChanged += UpdateCurrency;
    }

    public void UpdateCurrency(int currency)
    {
        if (moneyTextAmount != null)
        {
            moneyTextAmount.text = currency.ToString();
        }
    }
}
