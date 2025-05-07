using UnityEngine;

[CreateAssetMenu(fileName = "ActivePowerUp", menuName = "Power Ups/Active")]
public abstract class ActivePowerUp : PowerUpData, IActivePowerUp
{
    [Header("Cooldown Settings")]
    [SerializeField] protected CooldownSystem cooldown = new CooldownSystem();

    public bool IsOnCooldown => cooldown.IsOnCooldown;
    public float CooldownTimeRemaining => cooldown.CooldownTimeRemaining;

    public bool TryUse(MonoBehaviour coroutineRunner = null)
    {
        if (IsOnCooldown) return false;

        cooldown.StartCooldown();
        ApplyEffect();
        OnActivated?.Invoke();

        return true;
    }

    public override void OnRemoved()
    {
        RemoveEffect();
        OnDeactivated?.Invoke();
    }
}
