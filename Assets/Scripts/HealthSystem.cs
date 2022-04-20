using System;
using UnityEngine;

public class HealthSystem
{
    public enum HealthType
    {
        Player,
        Enemy,
        Boss,
        Object,
        None
    }

    public event EventHandler OnHealthChanged;

    private float health;
    private float maxHealth;
    private HealthType healthType;

    public HealthSystem()
    {
        maxHealth = 100;
        health = maxHealth;
    }
    public HealthSystem(HealthType type, int level)
    {
        if (type == HealthType.Player)
        {
            maxHealth = 100 + level * 10;
            health = maxHealth;
        }
        else if (type == HealthType.Enemy)
        {
            maxHealth = 100 + level * 20;
            health = maxHealth;
        }
        else if (type == HealthType.Boss)
        {
            maxHealth = 130 + level * 30;
            health = maxHealth;

            healthType = type;
        }
        else if (type == HealthType.Object)
        {
            maxHealth = 130 + level * 35;
            health = maxHealth;

            healthType = type;
        }
    }
    public HealthSystem(int level, float healthCount)
    {
        maxHealth = 100 + level * 10;
        health = healthCount;
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetHealthPercent()
    {
        return (float)health / maxHealth;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
            health = 0;

        if (OnHealthChanged != null)
            OnHealthChanged(this, EventArgs.Empty);
    }
    public void Heal()
    {
        health = maxHealth;

        if (OnHealthChanged != null)
            OnHealthChanged(this, EventArgs.Empty);

        //Debug.Log("max: " + maxHealth + "\nhealth:" + health);
    }
    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > maxHealth)
            health = maxHealth;

        if (OnHealthChanged != null)
            OnHealthChanged(this, EventArgs.Empty);
    }

    public void IncreaseHealth(float level)
    {
        maxHealth = 100 + level * 10;
        Heal();
    }
}
