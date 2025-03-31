using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy attackStrategy;
    [SerializeField] private float comboResetTime = 1f;
    [SerializeField] private int maxComboSteps = 3;
    [SerializeField] private LayerMask enemyLayer;

    private SwitchHPState switchHPState;
    private int comboStep = 0;
    private float lastAttackTime;
    private bool isAttacking = false;

    //attack event

    private void Awake()
    {
        switchHPState = GetComponent<SwitchHPState>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - lastAttackTime > comboResetTime && isAttacking)
        {
            comboStep = 0;
            isAttacking = false;
            PlayerEvent.AttackEnded();
        }
    }

    private void Attack()
    {
        if (attackStrategy == null || switchHPState.CurrentState == null)
        {
            Debug.LogWarning("Ќе назначена стратеги€ атаки или нет состо€ни€!");
            return;
        }
        isAttacking = true;
        lastAttackTime = Time.time;

        attackStrategy.PerformAttack(switchHPState.CurrentState, comboStep);

        float attackRange = attackStrategy.GetAttackRange();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            PerformAttack(enemy.gameObject);
        }

        PlayerEvent.AttackStarted(comboStep);
        comboStep = (comboStep + 1) % maxComboSteps;
    }

    public void PerformAttack(GameObject target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();

        if(damagable != null && damagable.IsAlive())
        {
            int damage = GetDamage();
            damagable.TakeDamage(damage, gameObject);
        }
    }

    public int GetDamage()
    {
        return attackStrategy.CalculateDamage(switchHPState.CurrentState, comboStep);
    }
}
