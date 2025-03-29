using UnityEngine;

[CreateAssetMenu(fileName = "BAttack", menuName = "Attack Strategies/B")]
public class BAttackStrategy : AttackStrategy
{
    public override void PerformAttack(FSMState state, int comboStep)
    {
        string comboText = comboStep == 0 ? "First" : comboStep == 1 ? "Second" : "Last";

        if (state is HighHPState)
        {
            Debug.Log($"B: Full! ({comboText} hit)");
        }   
        else if (state is LowHPState)
        {
            Debug.Log($"B: Low! ({comboText} hit)");
        }
    }
}