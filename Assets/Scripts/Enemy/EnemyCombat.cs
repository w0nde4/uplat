using UnityEngine;

public class EnemyCombat : MonoBehaviour, IAttacker
{
    [SerializeField] EnemySettings settings;

    private float lastAttackTime = 0f;
    private Transform target;

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
        return Time.time >= lastAttackTime + settings.AttackCooldown;
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

        DamageHandler damageable = target.GetComponent<DamageHandler>();

        if (damageable != null)
        {
            damageable.TakeDamage(settings.Damage, gameObject);
            lastAttackTime = Time.time;
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
