using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenStats : MonoBehaviour
{
    public int currHealth;
    public int maxHealth;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    public void CheckHealth()
    {
        if(currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
