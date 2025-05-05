using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageHandler : MonoBehaviour, IDamagable
{
    [SerializeField] private Health health;
    
    private List<IDamageInterceptor> interceptors = new List<IDamageInterceptor>();

    public event Action<GameObject> OnDamageRecieved;

    private void Awake()
    {
        var componentInterceptors = GetComponents<IDamageInterceptor>();
        foreach (var interceptor in componentInterceptors)
        {
            interceptors.Add(interceptor);
        }

        health = GetComponent<Health>();
    }

    public void RegisterInterceptor(IDamageInterceptor interceptor)
    {
        if (!interceptors.Contains(interceptor))
        {
            interceptors.Add(interceptor);
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
