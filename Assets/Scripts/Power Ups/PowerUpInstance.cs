using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum PowerUpContext
{
    World,
    Shop
}
[RequireComponent(typeof(Collider2D))]
public class PowerUpInstance : MonoBehaviour, IInteractible
{
    [SerializeField] private PowerUp powerUpData;
    [SerializeField] private PowerUpContext context;
    
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;

    private Color originalColor;
    private bool isHighlighted = false;
    private bool isOnLayer;

    private Shop shop;
    private IPowerUpInteractionStrategy interactionStrategy;

    public PowerUp PowerUpData => powerUpData;

    public void Init(PowerUp powerUp, IPowerUpInteractionStrategy strategy)
    {
        this.powerUpData = powerUp;
        this.interactionStrategy = strategy;
    }

    private void Start()
    {
        if (highlightRenderer != null)
            originalColor = highlightRenderer.color;

        if (gameObject.layer != LayerMask.NameToLayer("Interactible"))
        {
            Debug.LogError($"Set {gameObject} layer to interactible");
        }

        if (interactionStrategy == null)
        {
            if (context == PowerUpContext.World)
            {
                interactionStrategy = new WorldPowerUpInteraction();
            }
            else
            {
                Debug.LogError("ShopPowerUpInteraction must be set manually via Init");
            }
        }
    }

    public void Highlight(bool shouldHighlight)
    {
        if (highlightRenderer == null) return;
        isHighlighted = shouldHighlight;
        highlightRenderer.color = shouldHighlight ? highlightColor : originalColor;
    }

    public bool IsHighlighed() => isHighlighted;

    public void Interact(Player player)
    {
        if(interactionStrategy == null)
        {
            Debug.LogWarning("Interaction strategy not set!");
            return;
        }
        interactionStrategy?.Interact(this, player);
    }

    public string GetInteractionPrompt()
    {
        return "Pick up " + powerUpData.PowerUpName;
    }
}
