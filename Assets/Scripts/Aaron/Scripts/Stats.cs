using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    public float currHealth;
    public float maxHealth;
    public bool isDead;

    public void Die()
    {
        currHealth = 0;
        Destroy(gameObject);
    }
}
