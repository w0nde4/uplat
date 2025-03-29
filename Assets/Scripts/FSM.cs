using System;
using System.Collections.Generic;

public class FSM
{
    private FSMState currentState;
    private Dictionary<Type, FSMState> states = new Dictionary<Type, FSMState>();   

    public FSMState CurrentState => currentState;

    public void AddState(FSMState state)
    {
        states.Add(state.GetType(), state);
    }

    public void SetState<T>() where T : FSMState
    {
        var type = typeof(T);

        if (currentState?.GetType() == type)
        {
            return;
        }

        if (states.TryGetValue(type, out var newState))
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    public void Update()
    {
        currentState?.Update();
    }
}
