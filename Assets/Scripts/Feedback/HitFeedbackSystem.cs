using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Lifecycle))]
public class HitFeedbackSystem : MonoBehaviour
{
    [SerializeField] private List<HitFeedbackEffectData> feedbackEffects = new List<HitFeedbackEffectData>();

    [Serializable]
    public class HitFeedbackEffectData
    {
        public bool enabled = true;
        public MonoBehaviour effectComponent;
    }

    private Lifecycle lifecycle;

    private void Awake()
    {
        lifecycle = GetComponent<Lifecycle>();
    }

    private void OnEnable()
    {
        lifecycle.DamageHandler.OnDamageRecieved += HandleDamageReceived;
    }

    private void OnDisable()
    {
        lifecycle.DamageHandler.OnDamageRecieved -= HandleDamageReceived;
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
