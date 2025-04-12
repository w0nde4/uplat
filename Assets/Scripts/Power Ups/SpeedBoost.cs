using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "PowerUp/SpeedBoost")]
public class SpeedBoost : PowerUp
{
    [SerializeField] private float speedMultiplier = 1.3f;
    
    private float startSpeedMultiplier;
    public override void ApplyEffect(Player player)
    {
        var movement = GetComponent<PlayerMovement>(player);
        if (movement == null) return;

        ModifyProperty(movement,
            m => m.SpeedMultiplier,
            (m, val) => m.SpeedMultiplier = val,
            speedMultiplier,
            ref startSpeedMultiplier);
    }

    public override void RemoveEffect(Player player)
    {
        var movement = GetComponent<PlayerMovement>(player);
        if(movement == null) return;

        movement.SpeedMultiplier = startSpeedMultiplier;
    }
}
