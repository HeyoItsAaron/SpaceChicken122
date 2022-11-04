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
    public Player player;

    public Image healthBar; //count-ish
    public Image powerUpBar; //duration
    public Image ammoCountBar; //count

    [Range(0, 100)]
    public float healthFill = 0;
    public float healthMax = 100.0f;

    [Range(0, 100)]
    public float powerUpFill = 0;
    public float powerUpMax = 100.0f;

    [Range(0, 100)]
    public float ammoCountFill = 0;
    public float ammoCountMax = 100.0f;




    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("XR Origin").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        linkPlayerStats();
        fillBars();
        //HealthChange(playerHealth);
        //healthBar.fillAmount = 0;
        //powerUpBar.fillAmount = GetPowerUp();
        //AmmoCountChange(playerAmmoCount);
    }

    //link UI to playerStats
    public void linkPlayerStats()
    {
        healthFill = player.currHealth;
        powerUpFill = player.currentPowerUpDuration;
        ammoCountFill = player.currentAmmoCount;
    }

    //fill bars according to stats
    public void fillBars()
    {
        healthBar.fillAmount = (healthFill / 100.0f);
        powerUpBar.fillAmount = (powerUpFill / 100.0f);
        ammoCountBar.fillAmount = (ammoCountFill / 100.0f);
    }

    /*
    //usage chanegs for use in player and stuff
    public void HealthChange(float healthValue)
    {
        float amount = (healthValue / 100.0f);
        healthBar.fillAmount = amount;
    }

    public void PowerUpUsageChange(float PowerUpDuration)
    {
        float amount = (PowerUpDuration / 100.0f);
        powerUpBar.fillAmount = amount;
    }

    public void AmmoCountChange(float AmmoCount)
    {
        float amount = (AmmoCount / 100.0f);
        ammoCountBar.fillAmount = amount;
    }

    //Health
    public float GetHealth()
    {
        return healthFill;
    }

    //PowerUps
    public float GetPowerUp()
    {
        return powerUpFill;
    }

    public void AddPowerUp()
    {
        powerUpFill += player.powerUpDuration;
        powerUpFill -= player.powerUpDuration * Time.deltaTime;
        
    }

    //Ammo
    public float GetAmmoCount()
    {
        return ammoCountFill;
    }
    */
}
