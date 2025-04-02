using UnityEngine;

public class HPStatesInit : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float lowHPThreshold = 0.3f;

    private FSM fsm;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError("No Health component found for HPStatesInit");
            enabled = false;
            return;
        }

        InitializeFSM();
    }

    public FSMState GetCurrentState()
    {
        return fsm.CurrentState;
    }

    private void InitializeFSM()
    {
        fsm = new FSM();

        fsm.AddState(new HighHPState(fsm, health, lowHPThreshold));
        fsm.AddState(new LowHPState(fsm, health, lowHPThreshold));

        float healthRatio = health.GetHealthPercentage();
        if (healthRatio > lowHPThreshold)
            fsm.SetState<HighHPState>();
        else
            fsm.SetState<LowHPState>();
    }
}
