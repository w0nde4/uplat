using UnityEngine;

public class HPState : FSMState
{
    protected PlayerHP playerHP;
    protected readonly float healthToSwitch;

    protected HPState(FSM fsm, PlayerHP playerHP, float healthToSwitch) 
        : base(fsm)
    {
        this.playerHP = playerHP;
        this.healthToSwitch = healthToSwitch;
    }

    public override void Update()
    {
        CheckStateTransition();
    }

    protected void CheckStateTransition()
    {
        float health = playerHP.CurrentHealth;

        if (health > healthToSwitch)
        {
            Fsm.SetState<HighHPState>();
        }

        else if (health <= healthToSwitch)
        {
            Fsm.SetState<LowHPState>();
        }
    }
}
