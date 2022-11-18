using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAmmo : MonoBehaviour
{
    // variables
    [SerializeField] private float energyBought;
    [SerializeField] private float price;
    //private Player player;


    // methods
    void Start()
    {
        //player = GameObject.FindObjectOfType<Player>();
    }

    public void BuyEnergy()
    {
        //if (player.currCurrency >= price)
        //{
            //player.currCurrency -= price;
            //player.currEnergy += energyBought;
        //}
    }
}
