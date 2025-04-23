public interface IInteractible
{
    void Interact(PlayerInventoryWallet player);
    void Highlight(bool shouldHighlight);
    string GetInteractionPrompt();
}
