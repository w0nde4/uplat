using UnityEngine;

public interface IPowerUpConsumer
{
    bool CanRecieve(PowerUp powerUp);
    bool TryRecieve(PowerUp powerUp);
}
