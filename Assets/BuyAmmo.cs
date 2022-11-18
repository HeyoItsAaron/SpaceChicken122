using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuyAmmo : MonoBehaviourPun
{
    // variables
    [SerializeField] private float energyBought;
    [SerializeField] private float price;
    public PlayerStats player;


    // methods
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if(player == null)
            player = GameObject.FindObjectOfType<PlayerStats>();
    }

    public void BuyEnergyOnButtonPress()
    {
        gameObject.GetComponent<PhotonView>().RPC("BuyEnergy", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void BuyEnergy()
    {
        if (player.currCurrency >= price)
        {
            player.currCurrency -= price;
            player.currEnergy += energyBought;
        }
    }
}
