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
            health.OnDamageTaken += ReduceDamage;
        }

        StartCoroutine(ShieldTimer());
    }

    private void ReduceDamage(GameObject damager)
    {
        Debug.Log($"Shield reduced damage from {damager.name}");
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
            health.OnDamageTaken -= ReduceDamage;
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