using UnityEngine;

[CreateAssetMenu(fileName = "TimedActivePowerUp", menuName = "Power Ups/Timed Active")]
public abstract class TimedActivePowerUp : TimedPowerUpBase, IActivePowerUp
{
    [Header("Cooldown Settings")]
    [SerializeField] protected CooldownSystem cooldown = new CooldownSystem();

    public bool IsOnCooldown => cooldown.IsOnCooldown;
    public float CooldownTimeRemaining => cooldown.CooldownTimeRemaining;

    public bool TryUse(MonoBehaviour coroutineRunner = null)
    {
        if (IsOnCooldown) return false;

        cooldown.StartCooldown();

        if (timedEffect.HasDuration && coroutineRunner != null)
        {
            OnActivated?.Invoke();
            StartTimedEffect(coroutineRunner);
        }
        else
        {
            ApplyEffect();
            OnActivated?.Invoke();
        }

        return true;
    }

    public override void OnRemoved()
    {
        if (IsEffectActive)
        {
            RemoveEffect();
            OnDeactivated?.Invoke();
        }
    }
}
