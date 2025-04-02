using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject owner;
    private Vector2 direction;
    private float speed;
    private int damage;
    private LayerMask targetLayer;
    private bool piercing;
    private int pierceCount;
    private List<GameObject> hitTargets = new List<GameObject>();

    public void Initialize(GameObject owner, Vector2 direction, float speed, int damage,
                          float lifetime, LayerMask targetLayer, bool piercing, int maxPierceCount)
    {
        this.owner = owner;
        this.direction = direction.normalized;
        this.speed = speed;
        this.damage = damage;
        this.targetLayer = targetLayer;
        this.piercing = piercing;
        this.pierceCount = maxPierceCount;

        Destroy(gameObject, lifetime);
    }

    private void Start()
    {
        Debug.Log("Projectile spawned: " + gameObject);
    }

    void Update()
    {
        Debug.Log("Projectile is moving...");
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;

        else if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            GameObject target = collision.gameObject;

            if (hitTargets.Contains(target))
                return;

            IDamagable damagable = target.GetComponent<IDamagable>();
            
            if (damagable != null)
            {
                damagable.TakeDamage(damage, owner);
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
        else
        {
            Destroy(gameObject);
        }
    }
}
