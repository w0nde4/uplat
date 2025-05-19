public interface IPlayerInput
{
    bool JumpPressed { get; }
    bool JumpReleased { get; }
    bool DashPressed { get; }
    float HorizontalInput { get; }
    bool InteractPressed { get; }
}
