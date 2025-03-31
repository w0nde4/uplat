using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "AAttack", menuName = "Attack Strategies/A")]
public class AAttackStrategy : AttackStrategy
{
    [SerializeField] private float highHpDamageMult = 1.2f;
    [SerializeField] private float lowHpDamageMult = 0.8f;
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

    public override int CalculateDamage(FSMState state, int comboStep)
    {
        float stateMult = 1.0f;

        switch(state)
        {
            case HighHPState:
                stateMult = highHpDamageMult;
                break;
            case LowHPState:
                stateMult = lowHpDamageMult;
                break;
        }

        float comboMult = 1.0f + (comboStep * 0.2f);
        return Mathf.RoundToInt(baseDamage * stateMult * comboMult);
    }
}