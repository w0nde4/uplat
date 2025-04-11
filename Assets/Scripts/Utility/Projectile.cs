using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected GameObject owner;
    protected Vector2 direction;
    protected float speed;
    protected float lifetime;
    protected LayerMask targetLayer;
    protected bool hasCollided = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Initialize(GameObject owner, Vector2 direction, float speed, float lifetime, LayerMask targetLayer)
    {
        this.owner = owner;
        this.direction = direction.normalized;
        this.speed = speed;
        this.lifetime = lifetime;
        this.targetLayer = targetLayer;

        rb.linearVelocity = GetInitialVelocity();

        Destroy(gameObject, lifetime);
    }

    protected virtual Vector2 GetInitialVelocity()
    {
        return direction * speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided || collision.gameObject == owner) return;

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            OnHit(collision);
        }
        else
        {
            OnMiss(collision);
        }
    }

    protected abstract void OnHit(Collider2D collision);

    protected virtual void OnMiss(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
