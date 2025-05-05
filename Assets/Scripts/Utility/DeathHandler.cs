using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private MonoBehaviour[] componentsToDisable;

    public event Action OnDeath;

    private void OnEnable()
    {
        health.OnChanged += CheckDeath;
    }

    private void OnDisable()
    {
        health.OnChanged -= CheckDeath;
    }

    private void CheckDeath(int current, int max)
    {
        if (current > 0) return;

        OnDeath?.Invoke();

        foreach (var component in componentsToDisable)
        {
            component.enabled = false;
        }
    }
}

