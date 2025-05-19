using System;
using UnityEngine;

public abstract class BaseDamageInterceptor : ScriptableObject, IDamageInterceptor
{
    protected Coroutine activeCoroutine;
    protected GameObject user;

    public virtual void Initialize(MonoBehaviour coroutineRunner)
    {
        user = coroutineRunner.gameObject;
    }
    public abstract bool CanApplyDamage(GameObject damager);

    public abstract int ModifyDamage(int damage, GameObject damagable);
}
