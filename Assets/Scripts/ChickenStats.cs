// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenStats : MonoBehaviour
{
    public int currHealth;
    public int maxHealth;
    public bool isDead = false;
    Spawner spawn;

    public Transform myTarget;
    public Transform currentTarget;
    public NavMeshAgent myAgent;
    public int range;
    public float distance;

    public AudioClip deathClip;
    public AudioClip hurt;
    public AudioClip walk;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currHealth = maxHealth;
        spawn = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }


    // Methods

    public void CheckHealth()
    {
        if(currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0 && isDead == false)
        {
            currHealth = 0;
            isDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
        if(spawn.enemiesKilled >= spawn.enemyAmount)
        {
            spawn.NextWave();
        }
        else
        {
            spawn.enemiesKilled++;
        }
    }
}
