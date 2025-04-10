using System.Collections.Generic;
using UnityEngine;

public class PowerUpInstanceRegistry : MonoBehaviour
{
    private static PowerUpInstanceRegistry instance;

    [SerializeField] private List<PowerUp> allPowerUps = new List<PowerUp>();

    public static PowerUpInstanceRegistry Instance => instance;
    public IReadOnlyList<PowerUp> AllPowerUps => allPowerUps;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public PowerUp GetRandomPowerUp(Rarity minRarity = Rarity.Common)
    {
        List<PowerUp> validPowerUps = new List<PowerUp>();

        foreach (var powerUp in allPowerUps)
        {
            if (powerUp.RarityType >= minRarity)
            {
                validPowerUps.Add(powerUp);
            }
        }

        if (validPowerUps.Count == 0)
            return null;

        return validPowerUps[Random.Range(0, validPowerUps.Count)];
    }
}
