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
                if (level % 2 == 0)
                    DamageUp();
            }
        }
    }
    private int level
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
    private int addictiveDamage
    {
        get
        {
            return PlayerPrefs.GetInt("AddDamage", 0);
        }
        set
        {
            PlayerPrefs.SetInt("AddDamage", value);
        }
    }
    private void Start()
    {
        healthSystem = new HealthSystem(100);
        healthBar.Setup(healthSystem);

        //level = PlayerPrefs.GetInt("level", 1);
        level = 1;
    }
    public void LevelUp()
    {
        healthSystem.IncreaseHealth(level);

        level++;
        exp -= 25;

        //PlayerPrefs.SetInt("level", level);

        Debug.Log(healthSystem.GetHealth());
    }
    public void DamageUp()
    {

    }
}
