using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetworkStatManager : MonoBehaviourPun
{
    // variables 
    Stats thingStats;
    float defaultDamageAmount = 50;

    // methods
    [PunRPC]
    public void DoDamage(float damageAmount)
    {
        if (!thingStats.isDead)
        {
            thingStats.currHealth -= damageAmount;
            if(thingStats.currHealth <= 0)
            {
                thingStats.Die();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Light Bullet")
        {
            if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 15f);
            }
        }
        if (collision.gameObject.tag == "Medium Bullet")
        {
            if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 25f);
            }
        }
        if (collision.gameObject.tag == "Heavy Bullet")
        {
            if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 35f);
            }
        }
        if (collision.gameObject.tag == "HAMMER")
        {
            if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 50f);
            }
        }
    }

}
