using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : IDamagable
{
    private Health health;
    private List<IDamageInterceptor> interceptors = new List<IDamageInterceptor>();
    private MonoBehaviour coroutineRunner;

    public event Action<GameObject> OnDamageRecieved;

    public DamageHandler(Health health, MonoBehaviour coroutineRunner, List<IDamageInterceptor> interceptors = null)
    {
        this.health = health;
        this.coroutineRunner = coroutineRunner;

        foreach (var interceptor in interceptors)
        {
            RegisterInterceptor(interceptor);
        }
    }

    public void RegisterInterceptor(IDamageInterceptor interceptor)
    {
        if (!interceptors.Contains(interceptor))
        {
            interceptors.Add(interceptor);
            interceptor.Initialize(coroutineRunner);
            Debug.Log("Damage interceptor registered: " + interceptor.GetType().Name);
        }
    }

    public void UnregisterInterceptor(IDamageInterceptor interceptor)
    {
        if (interceptors.Contains(interceptor))
        {
            interceptors.Remove(interceptor);
            Debug.Log("Damage interceptor unregistered: " + interceptor.GetType().Name);
        }
    }

    public void TakeDamage(int damage, GameObject damager)
    {
        foreach (var interceptor in interceptors)
        {
            if (!interceptor.CanApplyDamage(damager)) return;
        }
        
        foreach (var interceptor in interceptors)
        {
            damage = interceptor.ModifyDamage(damage, damager);
        }

        health.Decrease(damage);
        OnDamageRecieved?.Invoke(damager);
    }
}
