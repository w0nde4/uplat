using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmokeUISetter : MonoBehaviour
{
    [SerializeField] private Smoke smoke;
    [SerializeField] private Slider smokeSlider;
    [SerializeField] private TextMeshProUGUI smokeAmountText;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color normalColor = Color.blue;
    [SerializeField] private Color lowSmokeColor = Color.red;
    [SerializeField] private float lowSmokeThreshold = 0.3f;

    [Header("Ability UI")]
    [SerializeField] private GameObject[] abilityIcons;
    [SerializeField] private Image[] cooldownOverlays;

    private void OnEnable()
    {
        if (smoke != null)
        {
            smoke.OnSmokeChanged += UpdateSmokeBar;
            smoke.OnAbilityUsed += UpdateAbilityCooldown;
        }
    }

    private void OnDisable()
    {
        if (smoke != null)
        {
            smoke.OnSmokeChanged -= UpdateSmokeBar;
            smoke.OnAbilityUsed -= UpdateAbilityCooldown;
        }
    }

    private void Start()
    {
        if (smoke != null)
        {
            UpdateSmokeBar(smoke.CurrentSmokeAmount, smoke.MaximumSmokeAmount);
        }
    }

    private void UpdateSmokeBar(float current, float max)
    {
        if (smokeSlider != null)
        {
            smokeSlider.maxValue = max;
            smokeSlider.value = current;
        }

        if (smokeAmountText != null)
        {
            smokeAmountText.text = $"{Mathf.Round(current)}/{max}";
        }

        if (fillImage != null)
        {
            float percentage = current / max;
            fillImage.color = percentage <= lowSmokeThreshold ? lowSmokeColor : normalColor;
        }
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            fillImage.color = lowSmokeColor;
            yield return new WaitForSeconds(0.1f);
            fillImage.color = normalColor;
            yield return new WaitForSeconds(0.1f);
        }

        float percentage = smoke.CurrentSmokeAmount / smoke.MaximumSmokeAmount;
        fillImage.color = percentage <= lowSmokeThreshold ? lowSmokeColor : normalColor;
    }

    private void UpdateAbilityCooldown(SmokeAbility ability)
    {
        int index = smoke.Abilities.IndexOf(ability);
        if (index >= 0 && index < cooldownOverlays.Length)
        {
            StartCoroutine(UpdateCooldownOverlay(index, ability));
        }
    }

    private IEnumerator UpdateCooldownOverlay(int index, SmokeAbility ability)
    {
        Image overlay = cooldownOverlays[index];
        overlay.fillAmount = 1f;

        while (overlay.fillAmount > 0)
        {
            overlay.fillAmount = 1f - smoke.GetAbilityCooldownProgress(ability);
            yield return null;
        }
    }
}
