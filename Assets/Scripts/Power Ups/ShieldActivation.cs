using UnityEngine;

[CreateAssetMenu(fileName = "ShieldActivation", menuName = "Power Ups/Examples/Shield")]
public class ShieldActivation : TimedActivePowerUp, IDamageInterceptor
{
    [SerializeField] private float blockDamagePercent = 30;

    public override void ApplyEffect()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("Shield activated!");

            var damageHandler = player.GetComponent<DamageHandler>();
            if (damageHandler != null)
            {
                damageHandler.RegisterInterceptor(this);
            }
        }
    }

    public bool CanApplyDamage(GameObject damager) => true;

    public int ModifyDamage(int damage, GameObject damagable)
    {
        var damageMultiplier = 1f - blockDamagePercent / 100;
        if (damageMultiplier <= 0) return damage; 
        return Mathf.RoundToInt(damage * damageMultiplier);
    }

    public override void RemoveEffect()
    {
        Debug.Log("Shield deactivated!");
    }
}
