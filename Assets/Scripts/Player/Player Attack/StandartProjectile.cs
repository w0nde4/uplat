using System;
using System.Collections.Generic;
using UnityEngine;

public class StandartProjectile : Projectile
{
    public static event Action<GameObject, GameObject, int> OnProjectileHit;

    private float maxDistance;
    private Vector3 startPosition;
    private int damage;
    private bool piercing;
    private int pierceCount;
    private List<GameObject> hitTargets = new();

    public void SetupExtra(float maxDistance, int damage, bool piercing = false, int pierceCount = 0)
    {
        this.maxDistance = maxDistance;
        this.damage = damage;
        this.piercing = piercing;
        this.pierceCount = pierceCount;

        startPosition = transform.position;
    }

    private void Update()
    {
        float distanceTraveled = Vector3.Distance(transform.position, startPosition);
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnHit(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if (hitTargets.Contains(target)) return;

        if (target.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.TakeDamage(damage, owner);
            Debug.Log(target.name + " получил " + damage + " урона.");
            OnProjectileHit?.Invoke(owner, target, damage);
        }

        hitTargets.Add(target);

        if (piercing)
        {
            pierceCount--;
            if (pierceCount <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
