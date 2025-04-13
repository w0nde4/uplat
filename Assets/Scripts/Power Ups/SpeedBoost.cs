using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "PowerUps/Passive/SpeedBoost")]
public class SpeedBoost : PowerUp
{
    [SerializeField] private float speedMultiplier = 1.3f;
    
    private float startSpeedMultiplier;
    private float effectEndTime;

    public override void ApplyEffect(Player player)
    {
        var movement = GetComponent<PlayerMovement>(player);
        if (movement == null) return;

        ModifyProperty(movement,
            m => m.SpeedMultiplier,
            (m, val) => m.SpeedMultiplier = val,
            speedMultiplier,
            ref startSpeedMultiplier);

        if(EffectDuration > 0)
        {
            effectEndTime = Time.time + EffectDuration;
            player.StartCoroutine(RemoveEffectAfterDelay(player));
        }
    }

    public override void RemoveEffect(Player player)
    {
        var movement = GetComponent<PlayerMovement>(player);
        if(movement == null) return;

        movement.SpeedMultiplier = startSpeedMultiplier;
    }

    public override IEnumerator RemoveEffectAfterDelay(Player player)
    {
        base.RemoveEffectAfterDelay(player);
        yield return null;
    }
}
