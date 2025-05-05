using UnityEngine;

[System.Serializable]
public class EffectChance
{
    public StatusEffect effect;
    [Range(0, 100)]
    public float chance = 100f;
}
