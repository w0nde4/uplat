using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage, GameObject damager);
    bool IsAlive();
}
