using UnityEngine;

public interface IPlayerInput
{
    bool JumpPressed { get; }
    bool JumpReleased { get; }

    //Vector2 Movement { get; }
}
