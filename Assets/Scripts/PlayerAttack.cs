using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy attackStrategy;
    [SerializeField] private float comboResetTime = 1f;
    [SerializeField] private int maxComboSteps = 3;

    private SwitchHPState switchHPState;
    private int comboStep = 0;
    private float lastAttackTime;


    private void Awake()
    {
        switchHPState = GetComponent<SwitchHPState>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }
    }

    private void Attack()
    {
        if (attackStrategy == null || switchHPState.CurrentState == null)
        {
            Debug.LogWarning("Ќе назначена стратеги€ атаки или нет состо€ни€!");
            return;
        }

        attackStrategy.PerformAttack(switchHPState.CurrentState, comboStep);
        comboStep = (comboStep + 1) % maxComboSteps;
        lastAttackTime = Time.time;
    }
}
