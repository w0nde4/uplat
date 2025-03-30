using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy attackStrategy;
    [SerializeField] private float comboResetTime = 1f;
    [SerializeField] private int maxComboSteps = 3;

    private SwitchHPState switchHPState;
    private int comboStep = 0;
    private float lastAttackTime;
    private bool isAttacking = false;

    //attack event

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
        if (Time.time - lastAttackTime > comboResetTime && isAttacking)
        {
            comboStep = 0;
            isAttacking = false;
            PlayerEvent.AttackEnded();
        }
    }

    private void Attack()
    {
        isAttacking = true;

        if (attackStrategy == null || switchHPState.CurrentState == null)
        {
            Debug.LogWarning("Ќе назначена стратеги€ атаки или нет состо€ни€!");
            return;
        }

        attackStrategy.PerformAttack(switchHPState.CurrentState, comboStep);
        PlayerEvent.AttackStarted(comboStep);
        comboStep = (comboStep + 1) % maxComboSteps;
        lastAttackTime = Time.time;
    }
}
