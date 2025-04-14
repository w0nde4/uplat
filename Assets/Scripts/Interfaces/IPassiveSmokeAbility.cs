using UnityEngine;

/// <summary>
/// Interface for passive abilities
/// </summary>
public interface IPassiveSmokeAbility
{
    bool IsActive { get; }
    bool CheckActivationCondition(GameObject user);
    void Activate(GameObject user);
    void Deactivate(GameObject user);
    void UpdatePassiveEffect(GameObject user, float deltaTime);
    
}
