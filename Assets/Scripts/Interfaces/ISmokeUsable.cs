using UnityEngine;

/// <summary>
/// Interface for abilities that can be used actively or passively
/// </summary>
public interface ISmokeUsable
{
    float SmokeCost { get; }
    float CooldownTime { get; }
    float RemainingCooldown { get; }
    bool CanUse(float currentSmoke);
    float Use(GameObject user);
    void ResetCooldown();
}
