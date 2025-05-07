using UnityEngine;

[CreateAssetMenu(fileName = "TimedPassivePowerUp", menuName = "Power Ups/Timed Passive")]
public abstract class TimedPassivePowerUp : TimedPowerUpBase, IPassivePowerUp
{
    public override void OnAcquired()
    {
        OnActivated?.Invoke();
    }

    public override void OnRemoved()
    {
        
    }
}
