using System.Collections;
using UnityEngine;

public class BombSmokeProjectile : MonoBehaviour
{
    private GameObject owner;
    private Vector2 direction;
    private float speed;
    private float maxDistance;
    private float explosionRadius;
    private int damage;
    private float damageTickTime;
    private float smokeCloudDuration;
    private LayerMask targetLayer;
    private GameObject explosionPrefab;
    private Color smokeColor;

    private Vector3 startPosition;
    private float distanceTraveled;
    private bool exploded = false;

    public void Initialize(GameObject owner, Vector2 direction, float speed, float maxDistance,
                          float explosionRadius, int damage, float damageTickTime,
                          float smokeCloudDuration, LayerMask targetLayer,
                          GameObject explosionPrefab, Color smokeColor)
    {
        this.owner = owner;
        this.direction = direction.normalized;
        this.speed = speed;
        this.maxDistance = maxDistance;
        this.explosionRadius = explosionRadius;
        this.damage = damage;
        this.damageTickTime = damageTickTime;
        this.smokeCloudDuration = smokeCloudDuration;
        this.targetLayer = targetLayer;
        this.explosionPrefab = explosionPrefab;
        this.smokeColor = smokeColor;

        startPosition = transform.position;

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction.x, direction.y + 0.5f) * speed;
    }

    private void Update()
    {
        if (exploded) return;

        distanceTraveled = Vector3.Distance(transform.position, startPosition);

        if (distanceTraveled >= maxDistance)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (exploded) return;

        if (collision.gameObject != owner)
        {
            Explode();
        }
    }
    private void Explode()
    {
        exploded = true;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );

            if (explosion.TryGetComponent<Renderer>(out var explosionRenderer))
            {
                explosionRenderer.material.color = smokeColor;
            }

            explosion.transform.localScale = Vector3.one * explosionRadius;

            Destroy(explosion, smokeCloudDuration);
        }

        StartCoroutine(DealDamageOverTime());

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
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

            foreach (Collider2D collider in colliders)
            {
                IDamagable damageable = collider.GetComponent<IDamagable>();
                if (damageable != null && damageable.IsAlive())
                {
                    damageable.TakeDamage(damage, owner);
                }
            }

            elapsedTime += damageTickTime;
            yield return new WaitForSeconds(damageTickTime);
        }
    }
}
