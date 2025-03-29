using UnityEngine;

public class HighHPState : HPState
{
    public HighHPState(FSM fsm, PlayerHP playerHP, float healthToSwitch)
        : base(fsm, playerHP, healthToSwitch) { }

    public override void Enter() { }

    public override void Exit() { }
}
