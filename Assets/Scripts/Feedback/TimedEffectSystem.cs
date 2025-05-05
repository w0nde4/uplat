using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TimedEffectSystem
{
    [SerializeField] private float effectDuration;

    private bool isEffectActive = false;
    private Coroutine effectCoroutine = null;

    public float EffectDuration => effectDuration;
    public bool IsEffectActive => isEffectActive;
    public bool HasDuration => effectDuration > 0;

    public void StartEffect(MonoBehaviour runner, Action applyEffect, Action removeEffect, UnityEvent onDeactivated)
    {
        if (!HasDuration) return;

        if (isEffectActive && effectCoroutine != null)
        {
            runner.StopCoroutine(effectCoroutine);
        }

        applyEffect?.Invoke();
        isEffectActive = true;
        effectCoroutine = runner.StartCoroutine(RemoveEffectAfterDelay(runner, removeEffect, onDeactivated));
        Debug.Log($"Effect will expire in {effectDuration} seconds");
    }

    public void StopEffect(MonoBehaviour runner, Action removeEffect, UnityEvent onDeactivated)
    {
        if (effectCoroutine != null)
        {
            runner.StopCoroutine(effectCoroutine);
            effectCoroutine = null;

            if (isEffectActive)
            {
                removeEffect?.Invoke();
                isEffectActive = false;
                onDeactivated?.Invoke();
            }
        }
    }

    private IEnumerator RemoveEffectAfterDelay(MonoBehaviour runner, Action removeEffect, UnityEvent onDeactivated)
    {
        yield return new WaitForSeconds(effectDuration);
        removeEffect?.Invoke();
        isEffectActive = false;
        onDeactivated?.Invoke();
        effectCoroutine = null;
    }
}
