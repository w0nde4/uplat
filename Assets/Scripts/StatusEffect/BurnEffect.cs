using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "Effects/Burn")]
public class BurnEffect : StatusEffect
{
    [SerializeField] private float damagePerTick = 2f;
    [SerializeField] private float tickInterval = 0.5f;
    
    private IDamagable damagableTarget;
    private GameObject attacker;
    private Coroutine tickCoroutine;
    private MonoBehaviour coroutineHost;

    public void SetProperties(float duration, float dps, float tickInterval = 0.5f)
    {
        this.duration = duration;
        this.damagePerTick = dps;
        this.tickInterval = tickInterval;
        UpdateDuration();
    }

    public override void Initialize(GameObject target, GameObject effector)
    {
        base.Initialize(target, effector);

        if (target != null)
        {
            target.TryGetComponent(out damagableTarget);
            coroutineHost = target.GetComponent<MonoBehaviour>();
            if (coroutineHost == null)
            {
                Debug.LogWarning("No MonoBehaviour found on target to host the burn effect coroutine");
            }
        }

        if(effector != null) attacker = effector;
    }

    public override void ApplyEffect(GameObject target, GameObject effector)
    {
        base.ApplyEffect(target, effector);

        if(effector != null) attacker = effector;

        if (damagableTarget == null && target.TryGetComponent(out IDamagable damagable))
        {
            damagableTarget = damagable;
        }

        if(coroutineHost == null)
        {
            coroutineHost = target.GetComponent<MonoBehaviour>();
        }

        StartBurnCoroutine();
    }

    public override void StackEffect(StatusEffect newEffect)
    {
        base.StackEffect(newEffect);

        Debug.Log($"Burn effect stacked. Current stacks: {currentStacks}");
    }

    private void StartBurnCoroutine()
    {
        if (coroutineHost != null)
        {
            if (tickCoroutine != null)
            {
                coroutineHost.StopCoroutine(tickCoroutine);
                tickCoroutine = null;
            }

            tickCoroutine = coroutineHost.StartCoroutine(TickCoroutine());
        }
        else
        {
            Debug.LogError("Failed to apply burn effect: No MonoBehaviour found to host coroutine");
        }
    }

    private IEnumerator TickCoroutine()
    {
        while (timeRemaining > 0)
        {
            if (damagableTarget != null && attacker != null)
            {
                int damage = Mathf.RoundToInt(damagePerTick * currentStacks);
                damagableTarget.TakeDamage(damage, attacker);
                Debug.Log($"Burn tick: {damage} damage applied. Stacks: {currentStacks}, Time remaining: {timeRemaining:F1}s");
            }

            yield return new WaitForSeconds(tickInterval);
            timeRemaining -= tickInterval;
        }

        RemoveEffect();
    }

    public override void RemoveEffect()
    {
        if (coroutineHost != null && tickCoroutine != null)
        {
            coroutineHost.StopCoroutine(tickCoroutine);
            tickCoroutine = null;
        }

        Debug.Log($"Removed Burn effect. Final stacks: {currentStacks}");
        OnRemove();
    }

    public override void RemoveStack(StatusEffect effect)
    {
        base.RemoveStack(effect);
        Debug.Log($"Removed a stack of Burn. Remaining stacks: {currentStacks}");
    }

    public override void UpdateDuration()
    {
        base.UpdateDuration();
        Debug.Log($"Updated burn duration. Time remaining: {timeRemaining:F1}s");
    }

    public override void OnApply()
    {
        Debug.Log($"Burn effect applied with {currentStacks} stacks");
    }

    public override void OnRemove()
    {
        Debug.Log("Burn effect fully removed");
    }
}
