using UnityEngine;

public class KnockbackEffect : MonoBehaviour, IHitFeedback
{
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private ForceMode2D forceMode = ForceMode2D.Impulse;
    [SerializeField] private bool scaleWithDamage = true;
    [SerializeField] private float damageToForceRatio = 0.5f;

    public void ApplyEffect(GameObject target, GameObject damager, HitFeedbackContext context)
    {
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float actualForce = knockbackForce;
            if (scaleWithDamage)
            {
                actualForce += context.DamageAmount * damageToForceRatio;
            }

            Vector3 force = context.HitDirection * actualForce;
            force.y = upwardForce;
            rb.AddForce(force, forceMode);
        }
    }
}
