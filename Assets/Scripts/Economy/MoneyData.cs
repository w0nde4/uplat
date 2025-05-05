using UnityEngine;

[CreateAssetMenu(fileName = "MoneyData")]
public class MoneyData : ScriptableObject
{
    [SerializeField] private int amount = 10;
    [SerializeField] private int lifetimeSeconds = 15;
    [SerializeField] private GameObject prefab;

    public int Amount => amount;
    public int LifetimeSeconds => lifetimeSeconds;
    public GameObject Prefab => prefab;

    public void InitializeCurrency(int newAmount)
    {
        amount = Mathf.Max(newAmount, 1);
    }
}
