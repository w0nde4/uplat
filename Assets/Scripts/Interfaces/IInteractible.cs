using UnityEngine;

public interface IInteractible
{
    void Interact(MonoBehaviour interactor);
    void Highlight(bool shouldHighlight);
    bool IsHighlighted();
    string GetInteractionPrompt();
}
