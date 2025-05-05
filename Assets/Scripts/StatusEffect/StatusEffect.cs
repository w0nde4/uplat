using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    [SerializeField] protected string effectName;
    [SerializeField] protected float duration;
    [SerializeField] protected bool isStackable;
    [SerializeField] protected int maxStacks = 1;

    protected int currentStacks = 1;
    protected float timeRemaining;

    public string EffectName => effectName;
    public float Duration => duration;
    public bool IsStackable => isStackable;
    public int MaxStacks => maxStacks;
    public int CurrentStacks => currentStacks;
    public float TimeRemaining => timeRemaining;

    public virtual void Initialize(GameObject target, GameObject effector) { }

    public virtual bool CanStackWith(StatusEffect other) 
    {
        return EffectName == other.EffectName && isStackable;
    }

    public virtual void StackEffect(StatusEffect newEffect) 
    {
        currentStacks = Mathf.Min(currentStacks + 1, maxStacks);
        UpdateDuration();
    }

    public virtual void RemoveStack(StatusEffect effect)
    {
        currentStacks = Mathf.Max(currentStacks - 1, 0);
        if (currentStacks == 0) RemoveEffect();
        else UpdateDuration();
    }

    public virtual void ApplyEffect(GameObject target, GameObject effector)
    {
        UpdateDuration();
    }

    public virtual void Tick(float deltaTime)
    {
        timeRemaining -= deltaTime;
        if (timeRemaining <= 0)
        {
            RemoveEffect();
        }
    }

    public virtual void RemoveEffect() { }

    public virtual void UpdateDuration()
    {
        timeRemaining = duration;
    }

    public virtual void OnApply() { }
    public virtual void OnRemove() { }
}
