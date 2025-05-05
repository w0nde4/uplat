using System;
using UnityEngine;

[Serializable]
public class CooldownSystem
{
    [SerializeField] private float cooldownTime;
    private float cooldownEndTime = 0f;

    public bool IsOnCooldown => Time.time < cooldownEndTime;
    public float CooldownTimeRemaining => Mathf.Max(0, cooldownEndTime - Time.time);
    public float CooldownTime => cooldownTime;

    public void StartCooldown()
    {
        if (cooldownTime > 0)
        {
            cooldownEndTime = Time.time + cooldownTime;
        }
    }

    public void ResetCooldown()
    {
        cooldownEndTime = 0f;
    }
}
