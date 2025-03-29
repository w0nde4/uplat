using UnityEngine;

public class LowHPState : HPState
{
    public LowHPState(FSM fsm, PlayerHP playerHP, float healthToSwitch)
       : base(fsm, playerHP, healthToSwitch) { }

    public override void Enter() { }

    public override void Exit() { }
}
