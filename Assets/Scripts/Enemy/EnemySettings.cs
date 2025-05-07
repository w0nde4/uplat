using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "Settings/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [SerializeField] private LayerMask playerLayer;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float patrolWaitInSeconds = 1f;
    [Header("Attack Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    public LayerMask PlayerLayer => playerLayer;
    public float MoveSpeed => moveSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float DetectionRange => detectionRange;
    public int Damage => damage;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public float PatrolWaitInSeconds => patrolWaitInSeconds;
}
