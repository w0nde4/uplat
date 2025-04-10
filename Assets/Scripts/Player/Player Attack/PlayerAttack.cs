using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy[] attackStrategies;

    private float damageMultiplier = 1f;

    public float DamageMultiplier
    {
        get
        {
            return damageMultiplier;
        }
        set
        {
            if (value >= 1) damageMultiplier = value;
            else damageMultiplier = 1f;
        }
    }


    private int currentStrategyIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchAttackStrategy();
        }

        attackStrategies[currentStrategyIndex]?.UpdateStrategy(Time.deltaTime);
    }

    private void SwitchAttackStrategy()
    {
        currentStrategyIndex = (currentStrategyIndex + 1) % attackStrategies.Length;
    }

    private void PerformAttack()
    {
        attackStrategies[currentStrategyIndex]?.PerformAttack(gameObject);
    }
}
