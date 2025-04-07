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
            Debug.LogWarning("�� ��������� ��������� ����� ��� ��� ���������!");
            return;
        }

        attackStrategy.PerformAttack(gameObject);
    }
}
