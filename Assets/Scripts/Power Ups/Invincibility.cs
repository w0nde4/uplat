using UnityEngine;

[CreateAssetMenu(fileName = "Invincibility", menuName = "PowerUps/Active/Invincibility")]
public class Invincibility : PowerUp
{
    [Header("Invincibility Settings")]
    [SerializeField] private Material invincibilityMaterial;

    private Health health;
    private Material originalMaterial;
    private Renderer playerRenderer;

    public override void OnInitialize(Player player)
    {
        health = player.GetComponent<Health>();
        playerRenderer = player.GetComponentInChildren<Renderer>();

        if (playerRenderer != null)
        {
            originalMaterial = playerRenderer.material;
        }
    }

    public override void ApplyEffect(Player player)
    {
        if (health == null) health = player.GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("PlayerHealth component not found on player!");
            return;
        }

        health.AddInvulnerabilitySource();

        if (playerRenderer != null && invincibilityMaterial != null)
        {
            playerRenderer.material = invincibilityMaterial;
        }
    }

    public override void RemoveEffect(Player player)
    {
        if (health == null) return;

        health.RemoveInvulnerabilitySource();

        if (playerRenderer != null && originalMaterial != null)
        {
            playerRenderer.material = originalMaterial;
        }
    }
}
