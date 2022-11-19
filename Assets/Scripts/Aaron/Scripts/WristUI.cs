// Aaron Williams
// 11/4/2022

using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WristUI : MonoBehaviourPun
{
    //only show to player (not over network)
    //only show when wrist is raised or when menu button held

    //variables
    public PlayerStats player;
    public NetworkSpawner spawner;

    public Image healthBar; //count-ish
    public Image powerUpBar; //duration
    public Image ammoCountBar; //count
    public TextMeshProUGUI currencyCount;
    public TextMeshProUGUI waveText;

    [Range(0, 100)]
    public float healthFill;
    public float healthMax = 100.0f;

    [Range(0, 100)]
    public float powerUpFill;
    public float powerUpMax = 100.0f;

    [Range(0, 100)]
    public float ammoCountFill;
    public float ammoCountMax = 100.0f;

    //methods
    void OnEnable()
    {
        //player = GameObject.FindObjectOfType<Player>();
        spawner = GameObject.FindObjectOfType<NetworkSpawner>();
        linkAllStats();
    }
    void Update()
    {
        //linkPlayerStats();
        //fillBars();
    }
    //link UI to playerStats
    //link all stats
    public void linkAllStats()
    {
        gameObject.GetComponent<PhotonView>().RPC("LinkHealthUI", RpcTarget.AllBuffered);
        gameObject.GetComponent<PhotonView>().RPC("LinkPowerUpUI", RpcTarget.AllBuffered);
        gameObject.GetComponent<PhotonView>().RPC("LinkEnergyUI", RpcTarget.AllBuffered);
        gameObject.GetComponent<PhotonView>().RPC("LinkCurrencyUI", RpcTarget.AllBuffered);
        gameObject.GetComponent<PhotonView>().RPC("LinkWaveUI", RpcTarget.AllBuffered);
    }

    //Link individual stats
    [PunRPC]
    public void LinkHealthUI()
    {
        healthFill = player.currHealth;
        healthBar.fillAmount = (healthFill / 100.0f);

    }
    [PunRPC]
    public void LinkPowerUpUI()
    {
        powerUpFill = player.currentPowerUpDuration;
        powerUpBar.fillAmount = (powerUpFill / 100.0f);

    }
    [PunRPC]
    public void LinkEnergyUI()
    {
        ammoCountFill = player.currEnergy;
        ammoCountBar.fillAmount = (ammoCountFill / 100.0f);

    }
    [PunRPC]
    public void LinkCurrencyUI()
    {
        currencyCount.text = "$" + player.currCurrency.ToString("N2");

    }
    [PunRPC]
    public void LinkWaveUI()
    {
        waveText.text = spawner.waveNumber.ToString();

    }
    public void ToggleVisibilityOverNet()
    {
        gameObject.GetComponent<PhotonView>().RPC("ToggleVisibility", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        linkAllStats();
    }
}
