using UnityEngine;

public class ChaseState : EnemyState
{
    private float speed;
    public ChaseState(FSM fsm, EnemyAI enemy, EnemySettings settings) : base(fsm, enemy, settings) 
    {
        speed = settings.ChaseSpeed;
    }

    public override void Update()
    {
        base.Update();
        Chase();
    }

    private void Chase()
    {
        Enemy.MoveTowards(Player.position, speed);
    }
}
