using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;

        this.healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        //Debug.Log("Connect");
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("HealthBar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }
}
