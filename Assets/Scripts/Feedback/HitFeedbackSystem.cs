using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(DamageHandler))]
public class HitFeedbackSystem : MonoBehaviour
{
    [SerializeField] private List<HitFeedbackEffectData> feedbackEffects = new List<HitFeedbackEffectData>();

    [Serializable]
    public class HitFeedbackEffectData
    {
        public bool enabled = true;
        public MonoBehaviour effectComponent;
    }

    private DamageHandler damageHandler;

    private void Awake()
    {
        damageHandler = GetComponent<DamageHandler>();
    }

    private void OnEnable()
    {
        damageHandler.OnDamageRecieved += HandleDamageReceived;
    }

    private void OnDisable()
    {
        damageHandler.OnDamageRecieved -= HandleDamageReceived;
    }

    private void HandleDamageReceived(GameObject damager)
    {
        Vector3 hitDirection = Vector3.zero;
        if (damager != null)
        {
            hitDirection = (transform.position - damager.transform.position).normalized;
            hitDirection.y = 0;
        }

        HitFeedbackContext context = new HitFeedbackContext
        {
            HitDirection = hitDirection,
            HitPoint = transform.position,
            DamageAmount = 0,
            IsCritical = false
        };

        foreach (var effectData in feedbackEffects)
        {
            if (effectData.enabled && effectData.effectComponent != null)
            {
                IHitFeedback effect = effectData.effectComponent as IHitFeedback;
                if (effect != null)
                {
                    effect.ApplyEffect(gameObject, damager, context);
                }
            }
        }
    }

    public void AddEffect(MonoBehaviour effectComponent)
    {
        if (effectComponent is IHitFeedback)
        {
            var effectData = new HitFeedbackEffectData
            {
                enabled = true,
                effectComponent = effectComponent
            };
            feedbackEffects.Add(effectData);
        }
        else
        {
            Debug.LogError($"Component {effectComponent.GetType().Name} does not implement IHitFeedbackEffect");
        }
    }
}
