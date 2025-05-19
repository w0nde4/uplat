using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy[] attackStrategies;
    [SerializeField] private KeyboardInput input;

    private int currentStrategyIndex = 0;
    private AttackStrategy currentStrategy;

    private void Start()
    {
        if (attackStrategies.Length == 0)
        {
            Debug.LogError("Не назначены стратегии атаки в PlayerAttack!");
            return;
        }

        InitializeStrategies();
    }

    private void InitializeStrategies()
    {
        foreach (var strategy in attackStrategies)
        {
            strategy.Initialize(gameObject);
        }

        UpdateCurrentStrategy();
    }

    private void Update()
    {
        HandleInput();
        currentStrategy?.UpdateStrategy(Time.deltaTime);
    }

    private void HandleInput()
    {
        if (input.AttackPressed && currentStrategy != null)
        {
            currentStrategy.TryPerformAttack();
        }

        if (input.SwitchAttackPressed)
        {
            SwitchAttackStrategy();
        }
    }

    public void DealDamage()
    {
        if (currentStrategy != null)
        {
            currentStrategy.ApplyDamageToTargets();
        }
    }

    public void EndAttack()
    {
        if (currentStrategy != null)
        {
            currentStrategy.OnAttackEnd();
        }
    }

    private void SwitchAttackStrategy()
    {
        currentStrategyIndex = (currentStrategyIndex + 1) % attackStrategies.Length;
        UpdateCurrentStrategy();
    }

    private void UpdateCurrentStrategy()
    {
        if (attackStrategies.Length == 0) return;

        currentStrategy = attackStrategies[currentStrategyIndex];
    }

    public AttackStrategy GetCurrentStrategy()
    {
        return currentStrategy;
    }
}
