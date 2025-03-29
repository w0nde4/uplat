using System;
using System.Numerics;

public class PlayerEvent
{
    public static event Action<bool> OnDashChanged;
    public static event Action<bool> OnGroundedChanged;
    public static event Action<bool> OnFlipChanged;


    public static void DashChanged(bool isDashing) => OnDashChanged?.Invoke(isDashing);
    public static void GroundedChanged(bool isGrounded) => OnGroundedChanged?.Invoke(isGrounded);
    public static void FlipChanged(bool isFlipped) => OnFlipChanged?.Invoke(isFlipped);
}
