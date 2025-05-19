using UnityEngine;

[CreateAssetMenu(fileName = "Keyboard", menuName = "Input/Keyboard")]
public class KeyboardInput : ScriptableObject, IPlayerInput
{
    [SerializeField] private KeyCode jumpKey = KeyCode.W;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode interactKey = KeyCode.X;
    [SerializeField] private KeyCode attackKey = KeyCode.F;
    [SerializeField] private KeyCode switchAttackKey = KeyCode.C;
    
    private readonly string horizontalAxis = "Horizontal";

    public bool AttackPressed => Input.GetKeyDown(attackKey);
    public bool SwitchAttackPressed => Input.GetKeyDown(switchAttackKey);
    public bool JumpPressed => Input.GetKeyDown(jumpKey);
    public bool JumpReleased => Input.GetKeyUp(jumpKey);
    public bool DashPressed => Input.GetKeyDown(dashKey);
    public float HorizontalInput => Input.GetAxis(horizontalAxis);
    public bool InteractPressed => Input.GetKeyDown(interactKey);
}
