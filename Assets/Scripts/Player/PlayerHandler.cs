using CodeMonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public HealthBar healthBar;
    public HealthSystem healthSystem;

    private int exp;
    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;

            if (exp / 25 >= 1)
            {
                LevelUp();
                if (Level % 2 == 0)
                    DamageUp();
            }
        }
    }
    public int Level
    {
        get
        {
            return PlayerPrefs.GetInt("Level", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Level", value);
        }
    }
    public int AddictiveDamage
    {
        get
        {
            return PlayerPrefs.GetInt("AddDamage", 1);
        }
        set
        {
            PlayerPrefs.SetInt("AddDamage", value);
        }
    }


    private void Start()
    {
        healthSystem = new HealthSystem(Level, PlayerPrefs.GetFloat("PlayerHealth", 100 + Level * 10));
        healthBar.Setup(healthSystem);

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        //level = 1;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        PlayerPrefs.SetFloat("PlayerHealth", healthSystem.GetHealth());

        if (healthSystem.GetHealth() <= 0)
        {
            if (PlayerPrefs.GetInt("LevelRecord", 0) < Level)
                PlayerPrefs.SetInt("LevelRecord", Level);

            transform.gameObject.SetActive(false);
            GameAssets.isPlayerDeath = true;

            ScreenLoad.LoadScene("StartMenu");
        }
    }

    public void LevelUp()
    {
        Level++;
        Exp -= 25;

        healthSystem.IncreaseHealth(Level);
    }
    public void DamageUp()
    {
        AddictiveDamage++;
    }
}
