using UnityEngine;

public interface IDamageInterceptor
{
    bool CanApplyDamage(GameObject damager);
    int ModifyDamage(int damage, GameObject damagable);
}
