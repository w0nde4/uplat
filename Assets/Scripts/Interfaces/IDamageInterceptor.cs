using UnityEngine;
using UnityEngine.Events;

public interface IDamageInterceptor
{
    bool CanApplyDamage(GameObject damager);
    int ModifyDamage(int damage, GameObject damagable);
}
