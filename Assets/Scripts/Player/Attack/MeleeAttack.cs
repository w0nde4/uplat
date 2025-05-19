using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Attack Strategies/Melee Attack")]
public class MeleeAttack : AttackStrategy, IComboable
{
    [Header("Настройки комбо")]
    [SerializeField] private int maxComboStep = 3;
    [SerializeField] private float comboResetTime = 1.5f;
    [SerializeField] private float[] comboDamageMultipliers = { 1.0f, 1.2f, 1.5f };

    private int currentComboStep = 0;
    private float timeSinceLastAttack = 0f;

    public event Action<int> ComboStepChanged;
    public event Action ComboReset;

    public int MaxComboStep => maxComboStep;
    public float ComboResetTime => comboResetTime;

    public override void Initialize(GameObject owner)
    {
        base.Initialize(owner);
        ResetCombo();
    }

    protected override void PerformAttack()
    {
        isAttacking = true;
        PlayerEvent.AttackStarted(currentComboStep);
        AdvanceComboStep();

        timeSinceLastAttack = 0f;
    }

    protected override int CalculateDamage()
    {
        float multiplier = GetCurrentDamageMultiplier();
        return Mathf.RoundToInt(baseDamage * multiplier);
    }

    public override void UpdateStrategy(float deltaTime)
    {
        if (currentComboStep > 0)
        {
            timeSinceLastAttack += deltaTime;

            if (timeSinceLastAttack >= comboResetTime && !isAttacking)
            {
                ResetCombo();
            }
        }
    }

    public void AdvanceComboStep()
    {
        int previousStep = currentComboStep;
        currentComboStep = (currentComboStep + 1) % maxComboStep;

        ComboStepChanged?.Invoke(currentComboStep);
    }

    public void ResetCombo()
    {
        if (currentComboStep != 0)
        {
            currentComboStep = 0;
            ComboReset?.Invoke();
            PlayerEvent.AttackEnded();

            OnAttackEnd();
        }
    }

    public int GetComboStep() => currentComboStep;

    private float GetCurrentDamageMultiplier()
    {
        if (comboDamageMultipliers != null && comboDamageMultipliers.Length > 0)
        {
            int index = Mathf.Min(currentComboStep, comboDamageMultipliers.Length - 1);
            return comboDamageMultipliers[index];
        }

        return 1.0f + (currentComboStep * 0.2f);
    }
}