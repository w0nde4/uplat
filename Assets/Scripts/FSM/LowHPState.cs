using UnityEngine;

public class LowHPState : HPState
{
    public LowHPState(FSM fsm, Health health, float lowHPThreshold)
        : base(fsm, health, lowHPThreshold) { }

    protected override void HandleHealthChanged(int current, int max)
    {
        float ratio = (float)current / max;
        if (ShouldTransition(ratio))
            Fsm.SetState<HighHPState>();

        Fsm.Update();
    }
}
