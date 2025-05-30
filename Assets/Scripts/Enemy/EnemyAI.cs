using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemySettings settings;
    [SerializeField] private Transform[] patrolPoints;
    
    private FSM fsm;
    private Transform player;
    private Rigidbody2D rb;
    private EnemyCombat combat;

    public Transform[] PatrolPoints => patrolPoints;
    public Transform PlayerTransform => player;
    public EnemySettings Settings => settings;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        TryGetComponent(out combat);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        InitFsm();
    }

    private void InitFsm()
    {
        fsm = new FSM();

        if(settings == null)
        {
            Debug.LogError("Please set enemy settings");
            return;
        }

        fsm.AddState(new PatrolState(fsm, this, settings));
        fsm.AddState(new ChaseState(fsm, this, settings));    
        fsm.AddState(new AttackState(fsm, this, settings, combat));

        fsm.SetState<PatrolState>();
    }

    private void Update()
    {
        fsm.Update();
    }

    public void SetSpeedToZero()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void MoveTowards(Vector2 targetPosition, float speed)
    {
        if(speed <= 0)
        {
            Debug.LogWarning("Trying to set enemy movement speed via [MOVETOWARDS] to negative or zero");
            return;
        }

        if(targetPosition == null)
        {
            Debug.LogWarning("Null target position");
            return;
        }

        float direction = Mathf.Clamp((targetPosition.x - transform.position.x), -1, 1);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    public FSMState GetCurrentState()
    {
        return fsm.CurrentState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, settings.DetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.AttackRange);

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
