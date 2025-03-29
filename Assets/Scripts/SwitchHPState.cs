using UnityEngine;

public class SwitchHPState : MonoBehaviour
{
    [SerializeField] private float switchPercent;

    private FSM fsm;
    private PlayerHP playerHP;

    public FSMState CurrentState => fsm.CurrentState;

    private void Awake()
    {
        playerHP = GetComponent<PlayerHP>();
        fsm = new FSM();

        var maxHP = playerHP.MaxHealth;

        var healthToSwitch = maxHP * switchPercent / 100f;

        fsm.AddState(new HighHPState(fsm, playerHP, healthToSwitch));
        fsm.AddState(new LowHPState(fsm, playerHP, healthToSwitch));

        fsm.SetState<HighHPState>();
    }
    private void OnEnable() => PlayerHP.OnHealthChange += _ => fsm.Update();
    private void OnDisable() => PlayerHP.OnHealthChange -= _ => fsm.Update();
}
