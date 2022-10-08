using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider healthBar;
    public Health health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar.value != health.GetCurrHealth()/health.MaxHealth){
            healthBar.value = health.GetCurrHealth()/health.MaxHealth;
        }
    }
}
