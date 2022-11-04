using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WristUI : MonoBehaviour
{
    //only show to player (not over network)
    //only show when wrist is raised or when menu button held

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




    void Start()
    {
        //player = GameObject.Find("XR Origin").GetComponent<Player>();
    }

    void Update()
    {
        linkPlayerStats();
        fillBars();
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
}
