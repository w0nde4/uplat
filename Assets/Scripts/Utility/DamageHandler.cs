using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    
    private IDamageInterceptor[] interceptors;

    public event Action<GameObject> OnDamageRecieved;

    private void Awake()
    {
        interceptors = GetComponents<IDamageInterceptor>();
        health = GetComponent<Health>();
    }

    public void TakeDamage(int damage, GameObject damager)
    {
        foreach (var interceptor in interceptors)
        {
            if (!interceptor.CanApplyDamage(damager)) return;
        }
        
        foreach (var interceptor in interceptors)
            damage = interceptor.ModifyDamage(damage, damager);

        health.Decrease(damage);
        OnDamageRecieved?.Invoke(damager);
    }
}
