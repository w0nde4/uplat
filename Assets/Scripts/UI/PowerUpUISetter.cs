using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUISetter : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private SmokeAbility trackedAbility;

    private void OnEnable()
    {
        trackedAbility.OnAbilityUsed += HandleAbilityUsed;
    }

    private void OnDisable()
    {
        trackedAbility.OnAbilityUsed -= HandleAbilityUsed;
    }

    private void HandleAbilityUsed(GameObject user, SmokeAbility ability, float cooldown)
    {
        if (ability == trackedAbility)
        {
            StartCoroutine(CooldownCountdown(cooldown));
        }
    }

    private IEnumerator CooldownCountdown(float time)
    {
        float remaining = time;
        while (remaining > 0)
        {
            remaining -= Time.deltaTime;
            float progress = Mathf.Clamp01(1 - remaining / trackedAbility.CooldownTime);
            image.fillAmount = progress;
            yield return null;
        }
        image.fillAmount = 1;
    }
}
