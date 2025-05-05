using System;
using UnityEngine;

public class EnemyEvent
{
    public static event Action OnAttackStart;

    public static void AttackStarted() => OnAttackStart?.Invoke();
}
