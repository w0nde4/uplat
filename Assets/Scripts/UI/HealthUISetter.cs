using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUISetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpTextAmount;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Lifecycle player;

    private Health health;

    private void OnEnable()
    {
        if (player != null)
        {
            health = player.Health;
            if (health != null)
            {
                health.OnChanged += UpdateHealthBar;
                UpdateHealthBar(health.Current, health.Max);
            }
            else Debug.LogError("Player's Health is null!");
        }
        else Debug.LogError("Player reference is null in HealthUISetter!");
    }

    private void OnDisable()
    {
        if (health != null) health.OnChanged -= UpdateHealthBar;
    }

    public void UpdateHealthBar(int current, int max)
    {
        if (hpTextAmount != null)
        {
            hpTextAmount.text = $"{Mathf.Round(current)}/{max}";
        }
        
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }
}
