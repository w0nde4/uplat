using UnityEngine;

public interface IHasLimitedEffect
{
    float EffectDuration { get; }
    bool IsEffectActive { get; }
    void StartTimedEffect(MonoBehaviour coroutineRunner);
    void StopTimedEffect(MonoBehaviour coroutineRunner);
}
