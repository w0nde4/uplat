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
    [SerializeField] private LayerMask interactibleLayer;
    [SerializeField] private SpriteRenderer highlightRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;

    private Color originalColor;
    private bool isHighlighted = false;
    private Collider2D col;

    private IPowerUpInteractionStrategy interactionStrategy;

    public PowerUpData PowerUpData => powerUpData;

    public void Init(PowerUpData powerUp, IPowerUpInteractionStrategy strategy)
    {
        this.powerUpData = powerUp;
        this.interactionStrategy = strategy;
    }

    private void Start()
    {
        if(TryGetComponent(out Collider2D col))
            col.isTrigger = true;

        if (highlightRenderer != null)
            originalColor = highlightRenderer.color;

        int layer = interactibleLayer.value;
        
        if (layer != 0)
        {
            int firstEnabledLayer = (int)Mathf.Log(layer, 2);
            gameObject.layer = firstEnabledLayer;
        }
        else
        {
            Debug.LogError("LayerMask is empty!");
        }

        if (interactionStrategy == null)
        {
            if (context == PowerUpContext.World)
                interactionStrategy = new WorldPowerUpInteraction();
            else
                Debug.LogError("ShopPowerUpInteraction must be set manually via Init");
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

        if(interactor is IMoneyHandler moneyHandler && interactor is IPowerUpConsumer powerUpConsumer)
        {
            interactionStrategy?.Interact(this, moneyHandler, powerUpConsumer);
        }
    }

    public string GetInteractionPrompt()
    {
        return "Pick up " + powerUpData.PowerUpName;
    }
}
