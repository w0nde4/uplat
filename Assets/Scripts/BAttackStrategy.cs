using UnityEngine;

[CreateAssetMenu(fileName = "BAttack", menuName = "Attack Strategies/B")]
public class BAttackStrategy : AttackStrategy
{
    [SerializeField] private float aoeRadius = 2.0f;
    //[SerializeField] private float aoeDamageMultiplier = 0.7f;

    public override void PerformAttack(FSMState state, int comboStep)
    {
        string comboText = comboStep == 0 ? "First" : comboStep == 1 ? "Second" : "Last";

        if (state is HighHPState)
        {
            Debug.Log($"B: AOE Attack! ({comboText} hit)");
        }
        else if (state is LowHPState)
        {
            Debug.Log($"B: Quick Attack! ({comboText} hit)");
        }
    }

    public override float GetAttackRange()
    {
        return aoeRadius;
    }
}