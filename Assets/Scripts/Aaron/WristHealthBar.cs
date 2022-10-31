using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WristHealthBar : MonoBehaviour
{
    //move on health +/-
    //attach to wrist
    //only show to player (not over network)
    //only show when wrist is raised

    //variables
    public Image healthBar;

    [Range(0, 100)]
    public float playerHealth = 0;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthChange(playerHealth);
    }

    void HealthChange(float healthValue)
    {
        float amount = (healthValue / 100.0f) * 360.0f / 360;
        healthBar.fillAmount = amount;

    }
}
