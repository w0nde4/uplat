using UnityEngine;

public interface IDamageInterceptor
{
    public void Initialize(MonoBehaviour coroutineRunner);
    bool CanApplyDamage(GameObject damager);
    int ModifyDamage(int damage, GameObject damagable);
}
