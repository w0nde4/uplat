using System.Collections;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    [SerializeField] private float maximumSmokeAmount = 100f;
    [SerializeField] private float regenerationTime = 1f;
    [SerializeField] private float regenerationAmount = 10f;

    private float currentSmokeAmount;
    private bool isRegenerating = false;

    private void Awake()
    {
        currentSmokeAmount = maximumSmokeAmount;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            SpendSmoke(10f);
        }
        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            RegenerateSmoke(10f);
        }

        if (currentSmokeAmount < maximumSmokeAmount)
        {
            StartCoroutine(RegenerateSmokeByTime()); //event
        }
    }

    private void SpendSmoke(float cost)
    {
        if (cost > currentSmokeAmount)
        {
            //method
            return;
        }
        
        if (cost < 0) cost = 0;

        currentSmokeAmount -= cost;
    }

    private void RegenerateSmoke(float amount)
    {
        if (currentSmokeAmount + amount >= maximumSmokeAmount)
        {
            currentSmokeAmount = maximumSmokeAmount;
        }
        
        if(amount < 0) amount = 0;
        
        currentSmokeAmount += amount;
    }

    private IEnumerator RegenerateSmokeByTime()
    {
        yield return new WaitForSeconds(regenerationTime);
        
        if(currentSmokeAmount >= maximumSmokeAmount)
        {
            StopCoroutine(RegenerateSmokeByTime());
        }

        if (currentSmokeAmount + regenerationAmount >= maximumSmokeAmount)
        {
            currentSmokeAmount = maximumSmokeAmount;
            StopCoroutine(RegenerateSmokeByTime());
        }
        currentSmokeAmount += regenerationAmount;
    }
}
