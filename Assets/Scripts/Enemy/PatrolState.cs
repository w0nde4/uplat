using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState
{
    private List<Transform> patrolPoints = new List<Transform>();
    
    private int currentPatrolIndex = 0;
    private float speed;

    public PatrolState(FSM fsm, EnemyAI enemy, EnemySettings settings) : base(fsm, enemy, settings) 
    {
        foreach (var point in Enemy.PatrolPoints) 
            patrolPoints.Add(point);

        speed = settings.MoveSpeed;
    }

    public override void Update()
    {
        base.Update();
        Patrol();
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Enemy.MoveTowards(targetPoint.position, speed);

        if (Vector2.Distance(Transform.position, targetPoint.position) < 1)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }
}
