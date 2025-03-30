using UnityEngine;

//smth like fsm 
//states:
//patrol
//chase
//attack
public class EnemyMovement : MonoBehaviour
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

    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime;

    private enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol;

    private int currentPatrolIndex = 0;
    private float waypointThreshold = 0.1f;

    private Transform player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform; //DI here
    }

    private void Update()
    {
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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Switching to ATTACK");
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            Debug.Log("Switching to CHASE");
            currentState = EnemyState.Chase;
        }
        else
        {
            Debug.Log("Switching to PATROL");
            currentState = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Debug.Log($"Moving to {targetPoint.position}");
        MoveTowards(targetPoint.position, moveSpeed);

        if (Vector2.Distance(transform.position, targetPoint.position) < waypointThreshold)
        {
            Debug.Log("Reached waypoint, switching to next");
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Chase()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHP playerHP = player.GetComponent<PlayerHP>();
            
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }

    private void MoveTowards(Vector2 targetPosition, float speed)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * speed;
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
