public abstract class HPState : FSMState
{
    protected Health Health { get; private set; }
    protected float LowHPThreshold { get; private set; }

    protected HPState(FSM fsm, Health health, float lowHPThreshold) : base(fsm)
    {
        Health = health;
        LowHPThreshold = lowHPThreshold;
    }

    public override void Enter()
    {
        Health.OnHealthChanged += HandleHealthChanged;
    }

    public override void Exit()
    {
        Health.OnHealthChanged -= HandleHealthChanged;
    }

    protected abstract void HandleHealthChanged(int current, int max);

    protected bool ShouldTransition(float currentHPRatio)
    {
        if (this is HighHPState && currentHPRatio <= LowHPThreshold)
        {
            return true;
        }
        if (this is LowHPState && currentHPRatio > LowHPThreshold)
        {
            return true;
        }
        return false;
    }
}
