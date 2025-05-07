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

    [Header("Abilities")]
    [SerializeField] private List<SmokeAbility> abilities = new List<SmokeAbility>();
    [SerializeField] private KeyCode[] abilityKeys = new KeyCode[3] {
        KeyCode.Q, KeyCode.E, KeyCode.R
    };

    [Header("Passive Ability")]
    [SerializeField] private PassiveSmokeAbility passiveAbility;

    private float currentSmokeAmount;
    private bool isRegenerating = false;
    private SmokeAbility currentAbility;

    public event Action<float, float> OnSmokeChanged;
    public event Action<SmokeAbility> OnAbilityUsed;
    public event Action<PassiveSmokeAbility, bool> OnPassiveAbilityStateChanged;

    public float CurrentSmokeAmount => currentSmokeAmount;
    public float MaximumSmokeAmount => maximumSmokeAmount;
    public List<SmokeAbility> Abilities => abilities;
    public PassiveSmokeAbility PassiveAbility => passiveAbility;

    private void Awake()
    {
        currentSmokeAmount = maximumSmokeAmount;
    }

    private void OnEnable()
    {
        passiveAbility.OnPassiveStateChanged += HandlePassiveStateChanged;
    }

    private void OnDisable()
    {
        passiveAbility.OnPassiveStateChanged -= HandlePassiveStateChanged;
    }

    private void HandlePassiveStateChanged(GameObject user, PassiveSmokeAbility ability, bool isActive)
    {
        OnPassiveAbilityStateChanged?.Invoke(ability, isActive);
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
                TrySpendSmoke(10f);
            }
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                RegenerateSmoke(10f);
            }
        }

        ReadAbilitiesInput();

        passiveAbility.UpdatePassiveEffect(gameObject, Time.deltaTime);

        if (currentSmokeAmount < maximumSmokeAmount && !isRegenerating)
        {
            StartCoroutine(RegenerateSmokeOverTime());
        }
    }

    public void ReadAbilitiesInput()
    {
        for (int i = 0; i < abilities.Count && i < abilityKeys.Length; i++)
        {
            if (Input.GetKeyDown(abilityKeys[i]))
            {
                Debug.Log($"Pressed {abilityKeys[i]} for ability {abilities[i]?.name}");
                UseAbility(abilities[i]);
            }
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
                TrySpendSmoke(cost);
                OnAbilityUsed?.Invoke(ability);
            }
        }
    }

    public bool TrySpendSmoke(float cost)
    {
        if (cost > currentSmokeAmount || cost < 0)
        {
            return false;
        }

        currentSmokeAmount -= cost;
        OnSmokeChanged?.Invoke(currentSmokeAmount, maximumSmokeAmount);

        return true;
    }

    public void RegenerateSmoke(float amount)
    {
        if (amount < 0) amount = 0;

        float newAmount = currentSmokeAmount + amount;
        
        currentSmokeAmount = newAmount > maximumSmokeAmount ? maximumSmokeAmount : newAmount;

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

    public void AddPassiveAbility(PassiveSmokeAbility newPassive)
    {
        if (newPassive == null || passiveAbility == newPassive) return;

        passiveAbility = newPassive;
        newPassive.OnPassiveStateChanged += HandlePassiveStateChanged;
    }

    public void RemovePassiveAbility(PassiveSmokeAbility passive)
    {
        if (passive == null || passiveAbility != passive) return;

        if (passive.IsActive)
        {
            passive.Deactivate(gameObject);
        }

        passive.OnPassiveStateChanged -= HandlePassiveStateChanged;
        passiveAbility = null;
    }
}
