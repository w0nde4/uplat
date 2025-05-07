using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolState : EnemyState
{
    private List<Transform> patrolPoints = new List<Transform>();
    
    private int currentPatrolIndex = 0;
    private float speed;

    private bool isWaiting = false;
    private float waitEndTime;

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

        if (isWaiting)
        {
            if (Time.time >= waitEndTime)
            {
                isWaiting = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            }
            return;
        }

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Enemy.MoveTowards(targetPoint.position, speed);

        if (Vector2.Distance(Transform.position, targetPoint.position) < 1)
        {
            StartWaiting();
        }
    }

    private void StartWaiting()
    {
        isWaiting = true;
        waitEndTime = Time.time + Settings.PatrolWaitInSeconds;
        Enemy.SetSpeedToZero();
    }
}
