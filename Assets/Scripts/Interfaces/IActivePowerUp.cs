using UnityEngine;

public interface IActivePowerUp 
{
    public bool TryUse(MonoBehaviour coroutineRunner = null);
    bool IsOnCooldown {  get; }
    float CooldownTimeRemaining { get; }
}
