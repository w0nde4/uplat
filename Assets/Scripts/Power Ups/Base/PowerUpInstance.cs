using UnityEngine;

public enum PowerUpContext
{
    World,
    Shop
}

[RequireComponent(typeof(Collider2D))]
public class PowerUpInstance : MonoBehaviour, IInteractible
{
    [SerializeField] private PowerUpData powerUpData;
    [SerializeField] private PowerUpContext context;
    
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;

    private Color originalColor;
    private bool isHighlighted = false;

    private IPowerUpInteractionStrategy interactionStrategy;

    public PowerUpData PowerUpData => powerUpData;

    public void Init(PowerUpData powerUp, IPowerUpInteractionStrategy strategy)
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

    public bool IsHighlighted()
    {
        return isHighlighted;
    }

    public void Interact(MonoBehaviour interactor)
    {
        if(interactionStrategy == null)
        {
            Debug.LogWarning("Interaction strategy not set!");
            return;
        }

        if(interactor is MonoBehaviour player)
        {
            interactionStrategy?.Interact(this, player);
        }
    }

    public string GetInteractionPrompt()
    {
        return "Pick up " + powerUpData.PowerUpName;
    }
}
