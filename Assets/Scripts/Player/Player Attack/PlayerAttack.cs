using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy attackStrategy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformAttackSequence();
        }

        attackStrategy?.UpdateStrategy(Time.deltaTime);
    }

    private void PerformAttackSequence()
    {
        if (attackStrategy == null)
        {
            Debug.LogWarning("Ќе назначена стратеги€ атаки или нет состо€ни€!");
            return;
        }

        attackStrategy.PerformAttack(gameObject);
    }
}
