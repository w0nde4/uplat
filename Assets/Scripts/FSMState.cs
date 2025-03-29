public abstract class FSMState
{
    protected readonly FSM Fsm;

    public FSMState(FSM fsm)
    {
        Fsm = fsm; 
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
