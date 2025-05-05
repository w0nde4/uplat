using UnityEngine;

public class HitFeedbackContext
{
    public int DamageAmount { get; set; }
    public Vector3 HitDirection { get; set; }
    public Vector3 HitPoint { get; set; }
    public bool IsCritical { get; set; }
}