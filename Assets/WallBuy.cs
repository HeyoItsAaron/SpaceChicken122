// Aaron Williams
// 11/16/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WallBuy : MonoBehaviour
{
    // variables
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject weaponSpawnPoint;
    [SerializeField] private float price;
    private Player player;


    // methods
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    public void BuyWeapon()
    {
        if (player.currCurrency >= price)
        {
            player.currCurrency -= price;
            PhotonNetwork.Instantiate(weaponPrefab.name, weaponSpawnPoint.transform.position, weaponSpawnPoint.transform.rotation);
        }
    }

}
