using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Stats : MonoBehaviourPun
{
    public float currHealth;
    public float maxHealth;
    public bool isDead;

    public virtual void Die()
    {
        currHealth = 0;
        Destroy(gameObject);
    }
}
