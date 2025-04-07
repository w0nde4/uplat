using System.Collections;
using UnityEngine;

public class BombSmokeProjectile : Projectile
{
    private float maxDistance;
    private float explosionRadius;
    private int damage;
    private float damageTickTime;
    private float smokeCloudDuration;
    private GameObject explosionPrefab;
    private Color smokeColor;

    private Vector3 startPosition;
    private bool exploded = false;

    public void SetupExtra(float maxDistance, float explosionRadius, int damage,
                           float damageTickTime, float smokeCloudDuration,
                           GameObject explosionPrefab, Color smokeColor)
    {
        this.maxDistance = maxDistance;
        this.explosionRadius = explosionRadius;
        this.damage = damage;
        this.damageTickTime = damageTickTime;
        this.smokeCloudDuration = smokeCloudDuration;
        this.explosionPrefab = explosionPrefab;
        this.smokeColor = smokeColor;

        startPosition = transform.position;
    }

    protected override Vector2 GetInitialVelocity()
    {
        return new Vector2(direction.x, direction.y + 0.5f).normalized * speed;
    }

    private void Update()
    {
        if (exploded) return;

        float distanceTraveled = Vector3.Distance(transform.position, startPosition);
        if (distanceTraveled >= maxDistance)
        {
            Explode();
        }
    }

    protected override void OnHit(Collider2D collision)
    {
        if (!exploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (explosion.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = smokeColor;
            }

            explosion.transform.localScale = Vector3.one * explosionRadius;
            Destroy(explosion, smokeCloudDuration);
        }

        StartCoroutine(DealDamageOverTime());

        if (TryGetComponent<Renderer>(out var bombRenderer))
        {
            bombRenderer.enabled = false;
        }

        if (TryGetComponent<Collider2D>(out var collider))
        {
            collider.enabled = false;
        }

        Destroy(gameObject, smokeCloudDuration);
    }

    private IEnumerator DealDamageOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < smokeCloudDuration)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IDamagable>(out var damageable) && damageable.IsAlive())
                {
                    damageable.TakeDamage(damage, owner);
                }
            }

            elapsedTime += damageTickTime;
            yield return new WaitForSeconds(damageTickTime);
        }
    }
}
