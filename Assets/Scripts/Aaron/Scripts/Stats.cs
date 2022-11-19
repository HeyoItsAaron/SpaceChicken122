using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Stats : MonoBehaviourPun
{
    public float currHealth;
    public float maxHealth;
    public bool isDead;

    [PunRPC]
    public virtual void Die()
    {
        currHealth = 0;
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public virtual void TakeDamage(float damageAmount)
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
    public virtual void CheckHealth(float currHealth, float maxHealth)
    {
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0f && isDead == false)
        {
            currHealth = 0f;
            isDead = true;
            Die();
        }
    }
}
