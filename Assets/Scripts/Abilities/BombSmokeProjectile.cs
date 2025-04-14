using System.Collections;
using UnityEngine;

public class BombSmokeProjectile : Projectile
{
    private float maxDistance;
    private float explosionRadius;
    private int damage;
    private float smokeCloudDuration;
    private GameObject explosionPrefab;
    private Color smokeColor;

    private Vector3 startPosition;
    private bool exploded = false;

    public void SetupExtra(float maxDistance, float explosionRadius, int damage,
                           float smokeCloudDuration, GameObject explosionPrefab,
                           Color smokeColor, LayerMask targetLayer)
    {
        this.maxDistance = maxDistance;
        this.explosionRadius = explosionRadius;
        this.damage = damage;
        this.smokeCloudDuration = smokeCloudDuration;
        this.explosionPrefab = explosionPrefab;
        this.smokeColor = smokeColor;
        this.targetLayer = targetLayer;

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
            StartCoroutine(SmokeDecoy());
            exploded = true;
        }
    }

    protected override void OnHit(Collider2D collision)
    {
        if (!exploded)
        {
            StartCoroutine(SmokeDecoy());
            exploded = true;
        }
    }

    private IEnumerator SmokeDecoy()
    {
        if (TryGetComponent<Renderer>(out var rend)) rend.enabled = false;
        if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;

        yield return new WaitForSeconds(0.5f);

        float timer = 0f;

        while (timer < smokeCloudDuration)
        {
            AttractEnemies();
            timer += Time.deltaTime;
            yield return null;
        }

        Explode();
    }

    private void AttractEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        foreach (var c in colliders)
        {
            if (c.TryGetComponent<IAttractable>(out var enemy))
            {
                enemy.AttractTo(transform.position, smokeCloudDuration);
            }
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (explosion.TryGetComponent<Renderer>(out var r))
                r.material.color = smokeColor;

            explosion.transform.localScale = Vector3.one * explosionRadius;
            Destroy(explosion, 2f);
        }

        ApplyEffects();
        Destroy(gameObject);
    }

    private void ApplyEffects()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent<IDamagable>(out var dmg) && dmg.IsAlive())
            {
                dmg.TakeDamage(damage, owner);
            }

            if (col.TryGetComponent<IDisorientable>(out var disorient))
            {
                disorient.ApplyDisorientation(2f);
            }
        }
    }
}
