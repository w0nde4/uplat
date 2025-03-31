using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUISetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpTextAmount;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Player player;

    private Health playerHealth;

    private void Awake()
    {
        playerHealth = player.GetComponent<Health>();

        var maxHealth = playerHealth.MaxHealth;
        var currentHealth = playerHealth.CurrentHealth;
        healthSlider.maxValue = maxHealth;

        ChangeHealth(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += ChangeHealth;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged += ChangeHealth;
    }

    public void ChangeHealth(int currentHealth, int maxHealth)
    {
        hpTextAmount.text = currentHealth.ToString();
        healthSlider.value = currentHealth;
    }
}
