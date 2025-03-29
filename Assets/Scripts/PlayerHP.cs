using System;
using Unity.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;

    private float currentHealth;
    private float damage = 10;
    private float heal = 10;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public static event Action <float> OnHealthChange;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        DoDamage();
        DoHeal();
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Negative damage");
        }

        if (currentHealth - damage < 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }

        if(currentHealth == 0)
        {
            Die();
        }

        OnHealthChange?.Invoke(currentHealth);

    }
    public void TakeHeal(float heal)
    {
        if (heal < 0)
        {
            throw new System.ArgumentOutOfRangeException("Negative heal");
        }
        if(currentHealth + heal > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += heal;
        }

        OnHealthChange?.Invoke(currentHealth);

    }


    public void DoDamage()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            TakeDamage(damage);
        }
    }

    public void DoHeal()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            TakeHeal(heal);
        }

    }

    public void Die()
    {
        Debug.Log("Dead");
    }

    
}
