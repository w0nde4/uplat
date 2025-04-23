using UnityEngine;

public class KeyboardInput : MonoBehaviour, IPlayerInput
{
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    //public Vector2 Movement { get; private set; }

    private void Update()
    {
        JumpPressed = Input.GetKeyDown(KeyCode.W);
        JumpReleased = Input.GetKeyUp(KeyCode.W);
        //Movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
