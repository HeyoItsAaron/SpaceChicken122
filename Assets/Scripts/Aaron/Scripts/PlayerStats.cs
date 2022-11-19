using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

public class PlayerStats: Stats
{
    // variables
    public bool hasItemInHand;
    int avatarID;
    public float currentPowerUpDuration;
    public float currEnergy; //this is "ammmo"
    public float maxEnergy;
    public float currCurrency;
    //public float currHealth;
    //public float maxHealth;
    //public bool isDead;
    XROrigin origin;

    // methods
    void Start()
    {
        hasItemInHand = false;
        //origin = FindObjectOfType<XROrigin>();
        isDead = false;
        maxHealth = 100;
        currHealth = 100;
        currEnergy = 50;
        currCurrency = 50;
    }

    void Update()
    {
        CheckHealth(currHealth, maxHealth);
    }
    //gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25f);

    //THIS WORKS
    [PunRPC]
    public override void Die()
    {
        isDead = true;
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
        //if (currHealth != 0)
        //    currHealth = 0;
        //if (photonView.IsMine)
        //    photonView.RPC("LoadAvatar", RpcTarget.AllBuffered, 5);
        //currEnergy = 0;
        //currCurrency = 0;
        //origin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
        //origin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = false;
        //origin.GetComponent<HandHammerConnection>().enabled = false;
    }
    public override void CheckHealth(float currHealth, float maxHealth)
    {
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0f && isDead == false)
        {
            currHealth = 0f;
            //isDead = true;
            photonView.RPC("Die", RpcTarget.AllBuffered);
            //Die();
        }
    }
    [PunRPC]
    public void Respawn()
    {
        if (photonView.IsMine)
            photonView.RPC("LoadAvatar", RpcTarget.AllBuffered, PlayerPrefs.GetInt("AvatarID"));
        {
            maxHealth = 100;
            currHealth = 100;
            currEnergy = 50;
            currCurrency = 0;
            isDead = false;
            origin.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
            origin.GetComponent<ActionBasedContinuousTurnProvider>().enabled = true;
            origin.GetComponent<HandHammerConnection>().enabled = true;
        }
    }

    [PunRPC]
    public override void TakeDamage(float damageAmount)
    {
        if (!isDead)
        {
            currHealth -= damageAmount;
            if (currHealth <= 0)
            {
                Die();
            }
        }
    }
    [PunRPC]
    public void AddHealth(float healthAmount)
    {
        if(currHealth + healthAmount > maxHealth)
            currHealth = maxHealth;
        else
            currHealth += healthAmount;
    }

    [PunRPC]
    public void AddEnergy(float energyAmount)
    {
        if (currEnergy + energyAmount > maxEnergy)
            currEnergy = maxEnergy;
        else
            currEnergy += energyAmount;
    }

    [PunRPC]
    public void LoseEnergy(float energyAmount)
    {
        if (currEnergy - energyAmount < 0)
            currEnergy = 0;
        else
            currEnergy -= energyAmount;
    }

    [PunRPC]
    public void AddCurrency(float currencyAmount)
    {
            currCurrency += currencyAmount;
    }
    [PunRPC]
    public void LoseCurrency(float currencyAmount)
    {
        if (currCurrency - currencyAmount < 0)
            currCurrency = 0;
        else
            currCurrency -= currencyAmount;
    }

    //[PunRPC]
    //public void AddPowerUpDuration(float powerUpDuration)
    //{
    //    
    //}

}
