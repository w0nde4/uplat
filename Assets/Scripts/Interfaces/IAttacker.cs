using UnityEngine;

public interface IAttacker
{
    void PerformAttack(GameObject target);
    int GetDamage();
}
