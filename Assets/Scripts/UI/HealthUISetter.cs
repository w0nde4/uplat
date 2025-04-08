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
    }

    private void Start()
    {
        if (playerHealth != null)
        {
            UpdateHealthBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
    }

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    public void UpdateHealthBar(int current, int max)
    {
        if (hpTextAmount != null)
        {
            hpTextAmount.text = $"{Mathf.Round(current)}/{max}";
        }
        healthSlider.value = current;
    }
}
