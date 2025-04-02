using UnityEngine;

[CreateAssetMenu(fileName = "BAttack", menuName = "Attack Strategies/B")]
public class BAttackStrategy : AttackStrategy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private float cooldownTime = 1.5f;
    [SerializeField] private int projectileDamageMultiplier = 1;
    [SerializeField] private bool piercing = false;
    [SerializeField] private int maxPierceCount = 3;

    private float lastAttackTime = -100f;

    public override void PerformAttack(GameObject attacker, FSMState state, int comboStep)
    { 
        lastAttackTime = Time.time;

        int damage = CalculateDamage(state, comboStep);

        Vector2 direction = GetAttackerDirection(attacker);

        Vector3 instantiatePosition = new Vector3(attacker.transform.position.x, attacker.transform.position.y + 1, 0) + (Vector3)direction * 0.5f;

        GameObject projectile = Instantiate(
            projectilePrefab,
            instantiatePosition,
            Quaternion.identity
        );

        Projectile projectileController = projectile.GetComponent<Projectile>();
        if (projectileController != null)
        {
            Debug.Log(projectile + " initialized!");
            projectileController.Initialize(
                attacker,
                direction,
                projectileSpeed,
                damage,
                projectileLifetime,
                targetLayer,
                piercing,
                maxPierceCount
            );
        }
        else
        {
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log(projectile + " initialized!");
                rb.linearVelocity = direction * projectileSpeed;
            }
            else Debug.Log("Cant initialize projectile: " + projectile);
        }
    }

    private Vector2 GetAttackerDirection(GameObject attacker)
    {
        IDirectionable directionable = attacker.GetComponent<IDirectionable>();
        if (directionable != null)
            return directionable.GetFacingDirection();

        return attacker.transform.right;
    }

    public override int CalculateDamage(FSMState state, int comboStep)
    {
        int baseDamageCalculation = base.CalculateDamage(state, comboStep);

        return baseDamageCalculation * projectileDamageMultiplier;
    }

    public override float GetAttackRange()
    {
        return attackRange * 5f; 
    }
}