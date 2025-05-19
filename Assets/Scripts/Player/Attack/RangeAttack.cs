using UnityEngine;

[CreateAssetMenu(fileName = "BAttack", menuName = "Attack Strategies/B")]
public class RangeAttack : AttackStrategy //interface (projectile, ranged, instanciated, initialize)
{
    /*[SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private float projectileDamageMultiplier = 1;
    [SerializeField] private bool piercing = false;
    [SerializeField] private int maxPierceCount = 3;
    [SerializeField] private Vector3 offset;

    private float lastAttackTime = -100f;

    public override void TryPerformAttack(GameObject attacker)
    { 
        lastAttackTime = Time.time;
        
        var playerAttack = attacker.GetComponent<PlayerAttack>();
        var damage = CalculateDamage(1f);

        Vector2 direction = GetAttackerDirection(attacker);

        var attackerPosition = attacker.transform.position;
        Vector3 instantiatePosition = new Vector3(attackerPosition.x, attackerPosition.y) + offset;

        GameObject projectile = Instantiate(
            projectilePrefab,
            instantiatePosition,
            Quaternion.identity
        );

        if (projectile == null) return;

        StandartProjectile projectileController = projectile.GetComponent<StandartProjectile>();
        if (projectileController != null)
        {
            projectileController.Initialize(
                attacker,
                direction,
                projectileSpeed,
                projectileLifetime,
                targetLayer
            );
            projectileController.SetupExtra(
                attackRange,
                damage,
                piercing,
                maxPierceCount
            );
        }
        else
        {
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
        }
    }

    private Vector2 GetAttackerDirection(GameObject attacker)
    {
        IDirectionable directionable = attacker.GetComponent<IDirectionable>();
        if (directionable != null)
        {
            return directionable.GetFacingDirection();
        }

        return attacker.transform.right;
    }

    public override int CalculateDamage(float damageMultiplier)
    {
        return Mathf.RoundToInt(baseDamage * projectileDamageMultiplier * damageMultiplier);
    }*/
}