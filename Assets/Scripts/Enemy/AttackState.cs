using UnityEngine;

public class AttackState : EnemyState
{
    private EnemyCombat combat;
    public AttackState(FSM fsm, EnemyAI enemy, EnemySettings settings, EnemyCombat combat) : base(fsm, enemy, settings) 
    {
        this.combat = combat;
    }

    public override void Update()
    {
        base.Update();
        Attack();
    }

    private void Attack()
    {
        if (Player == null || combat == null)
            return;

        Enemy.SetSpeedToZero();

        if (combat.CanAttack())
        {
            combat.PerformAttack(Player.gameObject);
        }
    }
}
