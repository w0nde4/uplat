using UnityEngine;

public class ShieldSmokeEffect : MonoBehaviour
{
    private float damageReduction;
    private float shieldDuration;
    private GameObject shieldVisual;
    private Health health;
    private float originalDamageHandler;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void ActivateShield(float duration, float damageReduction, float radius,
                            float rotationSpeed, GameObject shieldPrefab, Color shieldColor)
    {
        this.shieldDuration = duration;
        this.damageReduction = damageReduction;

        if (shieldPrefab != null)
        {
            if (shieldVisual != null)
            {
                Destroy(shieldVisual);
            }

            shieldVisual = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            shieldVisual.transform.SetParent(transform, false);
            shieldVisual.transform.localPosition = Vector3.zero;
            shieldVisual.transform.localScale = Vector3.one * radius;

            if (shieldVisual.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = shieldColor;
            }

            ShieldRotator rotator = shieldVisual.AddComponent<ShieldRotator>();
            rotator.SetRotationSpeed(rotationSpeed);
        }

        if (health != null)
        {
            health.OnModifyDamage += ApplyDamageReduction;
        }

        StartCoroutine(ShieldTimer());
    }

    private int ApplyDamageReduction(int baseDamage, GameObject damager)
    {
        int reducedDamage = Mathf.CeilToInt(baseDamage * (1 - damageReduction));
        Debug.Log($"[Shield] Reduced damage: {baseDamage} → {reducedDamage}"); 
        return reducedDamage;
    }

    private System.Collections.IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(shieldDuration);
        DeactivateShield();
    }

    private void DeactivateShield()
    { 
        if (health != null)
        {
            health.OnModifyDamage -= ApplyDamageReduction;
        }

        if (shieldVisual != null)
        {
            Destroy(shieldVisual);
            shieldVisual = null;
        }
    }

    private void OnDestroy()
    {
        DeactivateShield();
    }
}