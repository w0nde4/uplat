using UnityEngine;

[CreateAssetMenu(fileName = "DashSettings", menuName = "Player/Dash Settings")]
public class DashSettings : ScriptableObject
{
    [Header("Dash Properties")]
    public float Force = 24f;
    public float Duration = 0.2f;
    public float Cooldown = 1f;
    public bool AllowAirDash = false;

    [Header("Physics Properties")]
    public float GravityMultiplierDuringDash = 0.1f;
    public float VerticalVelocityRetention = 0.5f;
    public float EndDashVelocityMultiplier = 0.5f;

    [Header("Collision Detection")]
    public float ObstacleDetectionDistance = 0.5f;
    public LayerMask ObstacleLayer;
}
