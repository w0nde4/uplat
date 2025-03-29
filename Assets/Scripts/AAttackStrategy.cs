using UnityEngine;

[CreateAssetMenu(fileName = "AAttack", menuName = "Attack Strategies/A")]
public class AAttackStrategy : AttackStrategy
{
    public override void PerformAttack(FSMState state, int comboStep)
    {
        string comboText = comboStep == 0 ? "First" : comboStep == 1 ? "Second" : "Last";

        if (state is HighHPState)
        {
            Debug.Log($"A: Full! ({comboText} hit)");
        }
        else if (state is LowHPState)
        {
            Debug.Log($"A: Low! ({comboText} hit)");
        }
    }
}