using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected string abilityDescription;
    [SerializeField] protected int abilityCost;
}
