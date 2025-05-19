using UnityEngine;

[CreateAssetMenu(fileName = "RunSettings", menuName = "Player/Run Settings")]
public class RunSettings : ScriptableObject
{
    public float MaxSpeed = 5f;
    public float Acceleration = 30f;
    public float Deceleration = 20f;
}
