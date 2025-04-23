using UnityEngine;

public class EnemyState : FSMState
{
    protected readonly EnemyAI Enemy;
    protected readonly EnemySettings Settings;
    protected readonly Transform Transform;
    protected readonly Transform Player;

    public EnemyState(FSM fsm, EnemyAI enemy, EnemySettings settings) : base(fsm)
    {
        Enemy = enemy;
        Settings = settings;

        Transform = Enemy.transform;
        Player = Enemy.PlayerTransform;
    }

    public override void Update()
    {
        var distanceToPlayer = GetDistanceToPlayer();

        if (distanceToPlayer <= Settings.AttackRange)
        {
            Fsm.SetState<AttackState>();
        }

        else if (distanceToPlayer <= Settings.DetectionRange)
        {
            Fsm.SetState<ChaseState>();
        }

        else
        {
            Fsm.SetState<PatrolState>();
        }
    }

    private float GetDistanceToPlayer()
    {
        return Vector2.Distance(Transform.position, Player.position);
    }
}
