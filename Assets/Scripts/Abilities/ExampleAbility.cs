using UnityEngine;

[CreateAssetMenu(fileName = "Example", menuName = "Abilities/SmokeAbility/Example")]
public class ExampleAbility : SmokeAbility
{
    protected override void PerformAbility(GameObject user)
    {
        Debug.Log("Used " + abilityName);
    }
}
