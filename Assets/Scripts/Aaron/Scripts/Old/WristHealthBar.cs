//Aaron Williams
//DEPRECATED
/*


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
    public Image powerUpUsageBar;
    public Image ammoCountBar;

    [Range(0, 100)]
    public float playerHealth = 0;

    [Range(0, 100)]
    public float playerPowerUpUsage = 0;

    [Range(0, 100)]
    public float playerAmmoCount = 0;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthChange(playerHealth);
        PowerUpUsageChange(playerPowerUpUsage);
        AmmoCountChange(playerAmmoCount);
    }

    void HealthChange(float healthValue)
    {
        float amount = (healthValue / 100.0f);
        healthBar.fillAmount = amount;
    }
    void PowerUpUsageChange(float PowerUpUsage)
    {
        float amount = (PowerUpUsage / 100.0f);
        powerUpUsageBar.fillAmount = amount;
    }
        void AmmoCountChange(float AmmoCount)
    {
        float amount = (AmmoCount / 100.0f);
        ammoCountBar.fillAmount = amount;
    }
}
*/
