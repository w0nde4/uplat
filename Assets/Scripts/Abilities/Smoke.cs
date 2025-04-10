using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    [Header("Smoke Resource")]
    [SerializeField] private float maximumSmokeAmount = 100f;
    [SerializeField] private float regenerationTime = 0.5f;
    [SerializeField] private float regenerationAmount = 1f;
    [SerializeField] private float lowSmokeThreshold = 20f;

    [Header("Abilities")]
    [SerializeField] private List<SmokeAbility> abilities = new List<SmokeAbility>();
    [SerializeField] private KeyCode[] abilityKeys = new KeyCode[4] {
        KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.F
    };

    [Header("UI References")]
    [SerializeField] private GameObject smokeBarUI;

    private float currentSmokeAmount;
    private bool isRegenerating = false;
    private SmokeAbility currentAbility;

    public event Action<float, float> OnSmokeChanged;
    public event Action<SmokeAbility> OnAbilityUsed;
    public event Action OnLowSmoke;

    public float CurrentSmokeAmount => currentSmokeAmount;
    public float MaximumSmokeAmount => maximumSmokeAmount;
    public List<SmokeAbility> Abilities => abilities;

    private void Awake()
    {
        currentSmokeAmount = maximumSmokeAmount;
    }

    private void Start()
    {
        OnSmokeChanged?.Invoke(currentSmokeAmount, maximumSmokeAmount);

        foreach (var ability in abilities)
        {
            ability.ResetCooldown();
        }
    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                SpendSmoke(10f);
            }
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                RegenerateSmoke(10f);
            }
        }

        for (int i = 0; i < abilities.Count && i < abilityKeys.Length; i++)
        {
            if (Input.GetKeyDown(abilityKeys[i]))
            {
                Debug.Log($"Pressed {abilityKeys[i]} for ability {abilities[i]?.name}");
                UseAbility(abilities[i]);
            }
        }

        if (currentSmokeAmount < maximumSmokeAmount && !isRegenerating)
        {
            StartCoroutine(RegenerateSmokeOverTime());
        }
    }

    public void UseAbility(SmokeAbility ability)
    {
        if (ability == null) return;

        if (ability.CanUse(currentSmokeAmount))
        {
            float cost = ability.Use(gameObject);
            if (cost > 0)
            {
                SpendSmoke(cost);
                OnAbilityUsed?.Invoke(ability);
            }
        }
        else
        {
            if (currentSmokeAmount < ability.SmokeCost)
            {
                OnLowSmoke?.Invoke();
            }
        }
    }

    public bool SpendSmoke(float cost)
    {
        if (cost > currentSmokeAmount)
        {
            return false;
        }

        if (cost < 0) cost = 0;

        currentSmokeAmount -= cost;
        OnSmokeChanged?.Invoke(currentSmokeAmount, maximumSmokeAmount);

        if (currentSmokeAmount <= lowSmokeThreshold)
        {
            OnLowSmoke?.Invoke();
        }

        return true;
    }

    public void RegenerateSmoke(float amount)
    {
        if (amount < 0) amount = 0;

        float newAmount = currentSmokeAmount + amount;
        if (newAmount > maximumSmokeAmount)
        {
            currentSmokeAmount = maximumSmokeAmount;
        }
        else
        {
            currentSmokeAmount = newAmount;
        }

        OnSmokeChanged?.Invoke(currentSmokeAmount, maximumSmokeAmount);
    }

    private IEnumerator RegenerateSmokeOverTime()
    {
        isRegenerating = true;

        while (currentSmokeAmount < maximumSmokeAmount)
        {
            yield return new WaitForSeconds(regenerationTime);

            float amount = regenerationAmount;
            if (currentSmokeAmount + amount > maximumSmokeAmount)
            {
                amount = maximumSmokeAmount - currentSmokeAmount;
            }

            RegenerateSmoke(amount);
        }

        isRegenerating = false;
    }

    public float GetAbilityCooldownProgress(SmokeAbility ability)
    {
        if (ability == null) return 1f;

        return 1f - (ability.RemainingCooldown / ability.CooldownTime);
    }
}
