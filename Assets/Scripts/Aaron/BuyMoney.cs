using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMoney : MonoBehaviourPun
{
    // variables
    [SerializeField] private float currencyBought = 200;
    //[SerializeField] private float price;
    public PlayerStats player;

    // methods
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerStats>();
    }

    public void BuyEnergyOnButtonPress()
    {
        gameObject.GetComponent<PhotonView>().RPC("BuyMoneyMethod", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void BuyMoneyMethod()
    {
        //if (player.currCurrency >= price)
        //{
        //    player.currCurrency -= price;
        player.currCurrency += currencyBought;
        //}
    }
}
