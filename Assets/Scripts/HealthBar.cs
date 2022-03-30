using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;

        this.healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        transform.Find("HealthBar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
        transform.parent.Find("HP Count").GetComponent<Text>().text = healthSystem.GetHealth().ToString();
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        //Debug.Log(233);

        transform.Find("HealthBar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);

        try
        {
            transform.parent.Find("HP Count").GetComponent<Text>().text = healthSystem.GetHealth().ToString();
        }
        catch { }
    }
}
