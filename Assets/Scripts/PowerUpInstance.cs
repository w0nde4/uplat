using System;
using UnityEngine;

public enum PowerUpContext
{
    World,
    Shop
}

public class PowerUpInstance : MonoBehaviour, IInteractible
{
    [SerializeField] private PowerUp powerUpData;
    [SerializeField] private PowerUpContext context;
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;

    private Color originalColor;
    private bool isHighlighed = false;

    public PowerUp PowerUpData => powerUpData;
    
    public event Action<PowerUpInstance> OnInteracted;

    private void Awake()
    {
        if (highlightRenderer != null)
            originalColor = highlightRenderer.color;
    }

    public void Highlight(bool shouldHighlight)
    {
        if (highlightRenderer == null) return;
        isHighlighed = shouldHighlight;
        highlightRenderer.color = shouldHighlight ? highlightColor : originalColor;
    }

    public bool IsHighlighed() => isHighlighed;

    public void Interact(Player player)
    {
        OnInteracted?.Invoke(this);
    }

    public string GetInteractionPrompt()
    {
        return "Pick up " + powerUpData.PowerUpName;
    }
}
