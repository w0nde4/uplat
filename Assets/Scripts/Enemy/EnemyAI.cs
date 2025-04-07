using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;

    private enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol;

    private int currentPatrolIndex = 0;
    private float waypointThreshold = 0.1f;
    private Transform player;
    private EnemyCombat combat;
    private Health health;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        combat = GetComponent<EnemyCombat>();
        health = GetComponent<Health>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (health != null)
        {
            health.OnDeath += OnDeath;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath -= OnDeath;
        }
    }

    private void Update()
    {
        if (health != null && !health.IsAlive())
            return;

        UpdateState();

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void UpdateState()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPoint.position, moveSpeed);

        if (Vector2.Distance(transform.position, targetPoint.position) < waypointThreshold)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Chase()
    {
        if (player == null)
            return;

        MoveTowards(player.position, chaseSpeed);
    }

    private void Attack()
    {
        if (player == null || combat == null)
            return;

        rb.linearVelocity = Vector2.zero;

        if (combat.CanAttack())
        {
            combat.PerformAttack(player.gameObject);
        }
    }

    private void MoveTowards(Vector2 targetPosition, float speed)
    {
        float direction = Mathf.Clamp((targetPosition.x - transform.position.x), -1, 1);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    private void OnDeath()
    {
        rb.linearVelocity = Vector2.zero;
        sr.color = Color.red;

        enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        if (patrolPoints != null)
        {
            foreach (var point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}
