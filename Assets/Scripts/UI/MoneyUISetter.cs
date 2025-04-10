using TMPro;
using UnityEngine;

public class MoneyUISetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyTextAmount;
    [SerializeField] private Player player;

    private PlayerWallet playerWallet;

    private void Awake()
    {
        playerWallet = player.GetComponent<PlayerWallet>();
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
