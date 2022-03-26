using System;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;

    private float health;
    private float maxHealth;
    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
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
