using System.Collections.Generic;
using UnityEngine;

public class StatusEffectApplier : MonoBehaviour
{
    [SerializeField] private List<EffectChance> effectsToApply = new List<EffectChance>();

    private IAttacker attacker;

    private void Awake()
    {
        attacker = GetComponent<IAttacker>();

        if (attacker != null && attacker is EnemyCombat enemyCombat)
        {
            enemyCombat.OnAttack += ApplyEffectsToTarget;
        }
    }

    private void OnDestroy()
    {
        if (attacker != null && attacker is EnemyCombat enemyCombat)
        {
            enemyCombat.OnAttack -= ApplyEffectsToTarget;
        }
    }

    public void ApplyEffectsToTarget(GameObject target)
    {
        if (target == null) return;

        StatusEffectHolder effectHolder = target.GetComponent<StatusEffectHolder>();
        if (effectHolder == null) return;

        foreach (EffectChance effectChance in effectsToApply)
        {
            if (effectChance.effect != null && Random.Range(0f, 100f) <= effectChance.chance)
            {
                effectHolder.ApplyEffect(effectChance.effect, gameObject);
                Debug.Log($"{gameObject.name} applied {effectChance.effect.EffectName} to {target.name}");
            }
        }
    }

    public void AddEffect(StatusEffect effect, float chance = 100f)
    {
        effectsToApply.Add(new EffectChance
        {
            effect = effect,
            chance = chance
        });
    }
}
