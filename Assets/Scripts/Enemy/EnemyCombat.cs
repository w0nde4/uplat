using UnityEngine;

public class EnemyCombat : MonoBehaviour, IAttacker
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask playerLayer;

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
        return Time.time >= lastAttackTime + attackCooldown;
    }

    public bool IsTargetInRange()
    {
        if (target == null)
            return false;

        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }

    

    public void PerformAttack(GameObject target)
    {
        if (!CanAttack())
            return;

        IDamagable damageable = target.GetComponent<IDamagable>();

        if (damageable != null && damageable.IsAlive())
        {
            damageable.TakeDamage(damage, gameObject);
            lastAttackTime = Time.time;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
