public interface IInteractible
{
    void Interact(Player player);
    void Highlight(bool shouldHighlight);
    string GetInteractionPrompt();
}
