using UnityEngine;

public class SwitchHPState : MonoBehaviour
{
    [SerializeField] private float lowHpThreshold = 0.3f;

    private FSMState currentState;
    private HighHPState highHpState;
    private LowHPState lowHpState;
    private Health health;

    public FSMState CurrentState => currentState;

    private void Awake()
    {
        health = GetComponent<Health>();
        Player player = GetComponent<Player>();

        highHpState = new HighHPState(player);
        lowHpState = new LowHPState(player);
    }

    private void Start()
    {
        ChangeState(highHpState);

        if (health != null)
        {
            health.OnHealthChanged += CheckHealthState;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChanged -= CheckHealthState;
        }
    }

    private void CheckHealthState(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;

        if (healthPercentage <= lowHpThreshold && !(currentState is LowHPState))
        {
            ChangeState(lowHpState);
        }
        else if (healthPercentage > lowHpThreshold && !(currentState is HighHPState))
        {
            ChangeState(highHpState);
        }
    }

    private void ChangeState(FSMState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}
