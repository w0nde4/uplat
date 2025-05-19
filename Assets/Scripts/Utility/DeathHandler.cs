using System;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler
{
    private Health health;
    private List<MonoBehaviour> componentsToDisable = new List<MonoBehaviour>();

    public event Action OnDeath;

    public DeathHandler(Health health, List<MonoBehaviour> componentsToDisable = null)
    {
        this.health = health;
        if(componentsToDisable != null) this.componentsToDisable = componentsToDisable;

        Subscribe();
    }

    ~DeathHandler()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        health.OnChanged += CheckDeath;
    }

    private void Unsubscribe()
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

