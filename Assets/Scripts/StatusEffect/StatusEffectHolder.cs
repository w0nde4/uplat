using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHolder : MonoBehaviour
{
    private Dictionary<string, StatusEffect> activeEffects = new Dictionary<string, StatusEffect>();

    public void ApplyEffect(StatusEffect effectTemplate, GameObject effector)
    {
        if (effectTemplate == null) return;

        if (activeEffects.TryGetValue(effectTemplate.EffectName, out StatusEffect existingEffect))
        {
            if (existingEffect.CanStackWith(effectTemplate))
            {
                existingEffect.StackEffect(effectTemplate);
                Debug.Log($"Stacked effect {effectTemplate.EffectName}, current stacks: {existingEffect.CurrentStacks}");
            }
            else
            {
                existingEffect.UpdateDuration();
                Debug.Log($"Refreshed effect {effectTemplate.EffectName} duration");
            }
        }
        else
        {
            StatusEffect newEffect = Instantiate(effectTemplate);

            newEffect.Initialize(gameObject, effector);
            newEffect.ApplyEffect(gameObject, effector);
            newEffect.OnApply();

            activeEffects.Add(newEffect.EffectName, newEffect);
            Debug.Log($"Applied new effect {newEffect.EffectName}");
        }
    }

    public void RemoveEffect(string effectName)
    {
        if (activeEffects.TryGetValue(effectName, out StatusEffect effect))
        {
            effect.RemoveEffect();
            activeEffects.Remove(effectName);
            Debug.Log($"Removed effect {effectName}");
        }
    }

    public void RemoveEffectStack(string effectName)
    {
        if (activeEffects.TryGetValue(effectName, out StatusEffect effect))
        {
            effect.RemoveStack(effect);

            if (effect.CurrentStacks <= 0)
            {
                activeEffects.Remove(effectName);
                Debug.Log($"Effect {effectName} removed due to no remaining stacks");
            }
        }
    }

    private void Update()
    {
        List<string> effectsToRemove = new List<string>();

        foreach (var kvp in activeEffects)
        {
            StatusEffect effect = kvp.Value;
            effect.Tick(Time.deltaTime);

            if (effect.TimeRemaining <= 0 || effect.CurrentStacks <= 0)
            {
                effectsToRemove.Add(kvp.Key);
            }
        }

        foreach (string effectName in effectsToRemove)
        {
            if (activeEffects.TryGetValue(effectName, out StatusEffect effect))
            {
                effect.OnRemove();
                activeEffects.Remove(effectName);
                Debug.Log($"Effect {effectName} removed after expiring");
            }
        }
    }

    public bool HasEffect(string effectName)
    {
        return activeEffects.ContainsKey(effectName);
    }

    public StatusEffect GetEffect(string effectName)
    {
        if (activeEffects.TryGetValue(effectName, out StatusEffect effect))
        {
            return effect;
        }
        return null;
    }

    public int GetEffectStacks(string effectName)
    {
        if (activeEffects.TryGetValue(effectName, out StatusEffect effect))
        {
            return effect.CurrentStacks;
        }
        return 0;
    }
}
