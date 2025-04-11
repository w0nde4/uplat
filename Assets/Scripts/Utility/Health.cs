using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isInvulnerableAfterHit = false;
    [SerializeField] private float invulnerabilityDuration = 0.5f;

    private int currentHealth;
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public event Action<int, int> OnHealthChanged; 
    public event Action<GameObject> OnDamageTaken;
    public event Action OnDeath;

    public event Func<int, GameObject, int> OnModifyDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
            }
        }

        if (Debug.isDebugBuild && CompareTag("Player"))
        {
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                TakeDamage(10, gameObject);
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Heal(10);
            }
        }
    }

    public void TakeDamage(int damage, GameObject damager)
    {
        if (isInvulnerable || damage <= 0 || !IsAlive())
            return;

        if(OnModifyDamage != null)
        {
            damage = OnModifyDamage.Invoke(damage, damager);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke(damager);

        if (isInvulnerableAfterHit)
        {
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public void Heal(int amount)
    {
        if (!IsAlive() || amount <= 0)
            return;

        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Negative heal");
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke();
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}
