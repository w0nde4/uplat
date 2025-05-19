using UnityEngine;

public interface IComboable
{
    public int MaxComboStep { get; }
    public float ComboResetTime { get; }
    public void AdvanceComboStep();
    public void ResetCombo();
    public int GetComboStep();

}