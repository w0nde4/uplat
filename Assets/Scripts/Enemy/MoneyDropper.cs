using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(DeathHandler))]
public class MoneyDropper : MonoBehaviour
{
    [SerializeField] private MoneyData moneyData;
    [SerializeField] private Vector2 baseDropForce = new Vector2(0, 2f);

    [Header("Coins")]
    [SerializeField] private int coinAmount = 3;
    [SerializeField] private bool isRandomCoinAmount = false;
    [SerializeField] private int minCoinRange = 2;
    [SerializeField] private int maxCoinRange = 3;
    
    [Header("Money")]
    [SerializeField] private bool isRandomMoneyRange = false;
    [SerializeField] private int minMoneyRange = 10;
    [SerializeField] private int maxMoneyRange = 20;

    private DeathHandler deathHandler;

    private void Awake()
    {
        deathHandler = GetComponent<DeathHandler>();
    }

    private void OnEnable()
    {
        deathHandler.OnDeath += DropMoney;
    }

    private void OnDisable()
    {
        deathHandler.OnDeath -= DropMoney;
    }

    public void DropMoney()
    {
        if (isRandomCoinAmount) coinAmount = Random.Range(minCoinRange, maxCoinRange);

        for (int i = 0; i < coinAmount; i++)
        {
            var newMoneyData = GetNewMoneyData();
            var dropForce = GetDropForce();
            var moneyInstance = Instantiate(moneyData.Prefab, transform.position, Quaternion.identity);
            var moneyInstanceComponent = moneyInstance.GetComponent<MoneyInstance>();

            if(moneyInstanceComponent != null) moneyInstanceComponent.Initialize(newMoneyData, dropForce);
        }
    }

    private Vector2 GetDropForce()
    {
        var randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(1f, 4f));
        var dropForce = randomForce + baseDropForce;

        return dropForce;
    }

    private MoneyData GetNewMoneyData()
    {
        MoneyData newMoneyData = ScriptableObject.CreateInstance<MoneyData>();

        int newAmount = 1;
        if(isRandomMoneyRange) newAmount = Random.Range(minMoneyRange, maxMoneyRange);

        else newAmount = moneyData.Amount;

        newMoneyData.InitializeCurrency(newAmount);

        return newMoneyData;
    }
}
