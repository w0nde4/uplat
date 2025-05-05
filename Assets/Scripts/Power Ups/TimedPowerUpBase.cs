using UnityEngine;

public abstract class TimedPowerUpBase : PowerUpData, IHasLimitedEffect
{
    [Header("Timed Effect Settings")]
    [SerializeField] protected TimedEffectSystem timedEffect = new TimedEffectSystem();

    public float EffectDuration => timedEffect.EffectDuration;
    public bool IsEffectActive => timedEffect.IsEffectActive;

    public void StartTimedEffect(MonoBehaviour coroutineRunner)
    {
        timedEffect.StartEffect(coroutineRunner, ApplyEffect, RemoveEffect, OnDeactivated);
    }

    public void StopTimedEffect(MonoBehaviour coroutineRunner)
    {
        timedEffect.StopEffect(coroutineRunner, RemoveEffect, OnDeactivated);
    }
}
