using System;

public class PlayerEvent
{
    public static event Action<bool> OnDashChanged;
    public static event Action<bool> OnGroundedChanged;
    public static event Action<int> OnAttackStart;
    public static event Action OnAttackEnd;

    public static void AttackStarted(int comboStep) => OnAttackStart?.Invoke(comboStep);
    public static void AttackEnded() => OnAttackEnd?.Invoke();
    public static void DashChanged(bool isDashing) => OnDashChanged?.Invoke(isDashing);
    public static void GroundedChanged(bool isGrounded) => OnGroundedChanged?.Invoke(isGrounded);
}
