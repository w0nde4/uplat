using UnityEngine;

public class HighHPState : HPState
{
    public HighHPState(FSM fsm, Health health, float lowHPThreshold)
        : base(fsm, health, lowHPThreshold) { }

    protected override void HandleHealthChanged(int current, int max)
    {
        float ratio = (float)current / max;
        if (ShouldTransition(ratio))
            Fsm.SetState<LowHPState>();

        Fsm.Update();
    }
}
