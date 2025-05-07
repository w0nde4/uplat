using UnityEngine;

[CreateAssetMenu(fileName = "PassiveExample", menuName = "Power Ups/Passive/Example")]
public class ExamplePassive : PassivePowerUp
{
    public override void ApplyEffect()
    {
        Debug.Log("Took " + this);
    }

    public override void RemoveEffect()
    {
        Debug.Log("Remove " + this);
    }
}
