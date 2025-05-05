using UnityEngine;

[CreateAssetMenu(fileName = "PassivePowerUp", menuName = "Power Ups/Passive")]
public abstract class PassivePowerUp : PowerUpData, IPassivePowerUp
{
    public override abstract void ApplyEffect();
    public override abstract void RemoveEffect();

    public override void OnAcquired()
    {
        ApplyEffect();
        OnActivated?.Invoke();
    }
    public override void OnRemoved()
    {
        RemoveEffect();
        OnDeactivated?.Invoke();
    }
}
