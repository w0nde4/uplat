using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IDirectionable
{
    [SerializeField] private AttackStrategy attackStrategy;
    [SerializeField] private float comboResetTime = 1f; //?
    [SerializeField] private int maxComboSteps = 3; //?
    [SerializeField] private LayerMask enemyLayer; //?

    private HPStatesInit hpStatesInit;

    //evrth for combo ? 
    private int comboStep = 0; 
    private float lastAttackTime;

    private bool isAttacking = false;

    private void Awake()
    {
        hpStatesInit = GetComponent<HPStatesInit>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformAttackSequence(); //not sequence
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking && Time.time - lastAttackTime > comboResetTime)
        {
            ResetCombo(); // combo?
        }
    }

    private void ResetCombo()
    {
        comboStep = 0;
        isAttacking = false;
        PlayerEvent.AttackEnded();
    }

    private void PerformAttackSequence()
    {
        if (!IsAttackValid())
        {
            Debug.LogWarning("Ќе назначена стратеги€ атаки или нет состо€ни€!");
            return;
        }

        isAttacking = true;
        lastAttackTime = Time.time; //?

        attackStrategy.PerformAttack(gameObject, hpStatesInit.GetCurrentState(), comboStep);

        PlayerEvent.AttackStarted(comboStep);
        comboStep = (comboStep + 1) % maxComboSteps; //?
    }

    private bool IsAttackValid()
    {
        return attackStrategy != null && hpStatesInit.GetCurrentState() != null;
    }

    public Vector2 GetFacingDirection()
    {
        var movement = GetComponent<PlayerMovement>();
        return movement.GetFacingDirection();
    }
}
