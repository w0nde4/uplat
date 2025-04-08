using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackStrategy[] attackStrategies;

    private int currentIndex = 0;

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

        attackStrategies[currentIndex]?.UpdateStrategy(Time.deltaTime);
    }

    private void SwitchAttackStrategy()
    {
        currentIndex = (currentIndex + 1) % attackStrategies.Length;
    }

    private void PerformAttack()
    {
        attackStrategies[currentIndex]?.PerformAttack(gameObject);
    }
}
