using UnityEngine;

public interface IHitFeedback
{
    void ApplyEffect(GameObject target, GameObject damager, HitFeedbackContext context);
}
