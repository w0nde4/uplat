using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class ComponentDisablerEffect : MonoBehaviour, IHitFeedback
{
    [Serializable]
    public class ComponentToDisable
    {
        public Behaviour component;
        public float disableDuration = 0.5f;
        public bool affectOnlyIfEnabled = true;
    }

    [SerializeField] private List<ComponentToDisable> componentsToDisable = new List<ComponentToDisable>();
    [SerializeField] private bool applyOnlyForDamageAboveThreshold = false;
    [SerializeField] private int damageThreshold = 5;

    private Dictionary<Behaviour, Coroutine> activeDisables = new Dictionary<Behaviour, Coroutine>();

    public void ApplyEffect(GameObject target, GameObject damager, HitFeedbackContext context)
    {
        if (applyOnlyForDamageAboveThreshold && context.DamageAmount < damageThreshold)
            return;

        foreach (var componentData in componentsToDisable)
        {
            if (componentData.component == null)
                continue;

            if (componentData.affectOnlyIfEnabled && !componentData.component.enabled)
                continue;

            if (activeDisables.TryGetValue(componentData.component, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
            }

            activeDisables[componentData.component] = StartCoroutine(
                DisableComponentTemporarily(componentData.component, componentData.disableDuration));
        }
    }

    private IEnumerator DisableComponentTemporarily(Behaviour component, float duration)
    {
        bool wasEnabled = component.enabled;

        component.enabled = false;

        yield return new WaitForSeconds(duration);

        if (wasEnabled)
        {
            component.enabled = true;
        }

        activeDisables.Remove(component);
    }

    private void OnDisable()
    {
        foreach (var kvp in activeDisables)
        {
            if (kvp.Value != null)
            {
                StopCoroutine(kvp.Value);
            }

            if (kvp.Key != null)
            {
                kvp.Key.enabled = true;
            }
        }

        activeDisables.Clear();
    }
}
