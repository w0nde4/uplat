using System;
using UnityEngine;

public class EnemyCombat : MonoBehaviour, IAttacker
{
    [SerializeField] private EnemySettings settings;

    private float lastAttackTime = 0f;
    private Transform target;
    private GameObject damageTarget;

    public event Action<GameObject> OnAttack;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    public bool CanAttack()
    {
        return enabled && Time.time >= lastAttackTime + settings.AttackCooldown;
    }

    public bool IsTargetInRange()
    {
        if (target == null)
            return false;

        return Vector2.Distance(transform.position, target.position) <= settings.AttackRange;
    }

    public void PerformAttack(GameObject target)
    {
        if (!CanAttack())
            return;

        EnemyEvent.AttackStarted();
        damageTarget = target;
    }

    public void DeadDamage()
    {
        if (target.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(settings.Damage, gameObject);
            lastAttackTime = Time.time;

            OnAttack?.Invoke(damageTarget);
        }
    }

    public int GetDamage()
    {
        return settings.Damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.AttackRange);
    }
}
