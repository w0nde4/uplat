using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public abstract class PowerUp : ScriptableObject
{
    [Header("Basic Information")]
    [SerializeField] private string powerUpName;
    [SerializeField][TextArea] private string powerUpDescription;
    [SerializeField] private Sprite icon;
    [SerializeField] private Rarity rarity;

    [Header("Prefabs and Visual Effects")]
    [SerializeField] private GameObject instancePrefab;
    [SerializeField] private GameObject activationVFXPrefab;
    [SerializeField] private GameObject deactivationVFXPrefab;
    [SerializeField] private AudioClip activationSFX;
    [SerializeField] private AudioClip deactivationSFX;

    [Header("Behavior Settings")]
    [SerializeField] private bool isPassive;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float effectDuration;
    [Tooltip("Set to -1 for unlimited uses")]
    [SerializeField] private int maxUses = -1;
    [SerializeField] private int baseCost;

    [Header("Events")]
    public UnityEvent<Player> OnActivated;
    public UnityEvent<Player> OnDeactivated;

    // Runtime properties
    private bool isEffectActive = false;
    private int remainingUses = -1;
    private float cooldownEndTime = 0f;
    private Coroutine effectCoroutine;

    // Public getters
    public string PowerUpName => powerUpName;
    public string PowerUpDescription => powerUpDescription;
    public GameObject InstancePrefab => instancePrefab;
    public bool IsPassive => isPassive;
    public float Cooldown => cooldownTime;
    public float EffectDuration => effectDuration;
    public int BaseCost => baseCost;
    public Rarity RarityType => rarity;
    public Sprite Icon => icon;
    public bool IsEffectActive => isEffectActive;
    public int RemainingUses => remainingUses;
    public bool IsOnCooldown => Time.time < cooldownEndTime;
    public float CooldownTimeRemaining => Mathf.Max(0, cooldownEndTime - Time.time);
    public int MaxUses => maxUses;

    // Abstract methods that derived classes must implement
    public abstract void ApplyEffect(Player player);
    public abstract void RemoveEffect(Player player);

    // Optional methods that derived classes can override
    public virtual void OnInitialize(Player player) { }
    public virtual void OnUpdate(Player player) { }

    // Helper methods for component access and property modification
    protected T GetComponent<T>(Player player) where T : Component
    {
        if (player == null) return null;
        return player.GetComponent<T>();
    }

    protected void ModifyProperty<T, V>(T component, Func<T, V> getter, Action<T, V> setter, V newValue, ref V originalValue) where T : Component
    {
        if (component == null) return;
        originalValue = getter(component);
        setter(component, newValue);
    }

    public virtual void Initialize(Player player)
    {
        remainingUses = maxUses;
        isEffectActive = false;
        cooldownEndTime = 0f;
        OnInitialize(player);
    }

    public virtual void OnAcquired(Player player)
    {
        Initialize(player);

        if (isPassive)
        {
            ApplyEffect(player);
            isEffectActive = true;
            OnActivated?.Invoke(player);

            if (activationVFXPrefab != null)
            {
                Instantiate(activationVFXPrefab, player.transform.position, Quaternion.identity);
            }

            if (activationSFX != null && player.TryGetComponent<AudioSource>(out var audioSource))
            {
                audioSource.PlayOneShot(activationSFX);
            }
        }

        Debug.Log($"{player.name} acquired {powerUpName}");
    }

    public virtual void OnRemoved(Player player)
    {
        if (effectCoroutine != null)
        {
            player.StopCoroutine(effectCoroutine);
            effectCoroutine = null;
        }

        RemoveEffect(player);
        isEffectActive = false;
        OnDeactivated?.Invoke(player);

        if (deactivationVFXPrefab != null)
        {
            Instantiate(deactivationVFXPrefab, player.transform.position, Quaternion.identity);
        }

        if (deactivationSFX != null && player.TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.PlayOneShot(deactivationSFX);
        }

        Debug.Log($"{player.name} removed {powerUpName}");
    }

    public virtual bool TryUse(Player player)
    {
        if (isPassive) return false;
        if (IsOnCooldown) return false;
        if (maxUses > 0 && remainingUses <= 0) return false;

        Use(player);
        return true;
    }

    protected virtual void Use(Player player)
    {
        if (isPassive) return;

        // Handle cooldown
        if (cooldownTime > 0)
        {
            cooldownEndTime = Time.time + cooldownTime;
        }

        // Handle limited uses
        if (maxUses > 0)
        {
            remainingUses--;
        }

        // Apply effect
        ApplyEffect(player);
        isEffectActive = true;
        OnActivated?.Invoke(player);

        if (activationVFXPrefab != null)
        {
            Instantiate(activationVFXPrefab, player.transform.position, Quaternion.identity);
        }

        if (activationSFX != null && player.TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.PlayOneShot(activationSFX);
        }

        // Handle duration
        if (effectDuration > 0)
        {
            if (effectCoroutine != null)
            {
                player.StopCoroutine(effectCoroutine);
            }
            effectCoroutine = player.StartCoroutine(RemoveEffectAfterDelay(player));
            Debug.Log($"{powerUpName} effect will expire in {effectDuration} seconds");
        }
    }

    public virtual IEnumerator RemoveEffectAfterDelay(Player player)
    {
        yield return new WaitForSeconds(effectDuration);

        RemoveEffect(player);
        isEffectActive = false;
        OnDeactivated?.Invoke(player);

        if (deactivationVFXPrefab != null)
        {
            Instantiate(deactivationVFXPrefab, player.transform.position, Quaternion.identity);
        }

        if (deactivationSFX != null && player.TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.PlayOneShot(deactivationSFX);
        }

        effectCoroutine = null;
    }
}
