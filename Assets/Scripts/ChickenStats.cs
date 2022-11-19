// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenStats : Stats
{
    Spawner spawn;

    public Transform myTarget;
    public Transform currentTarget;
    public float tempDist;
    public NavMeshAgent myAgent;
    public int range;
    public float distance;

    public AudioClip deathClip;
    public AudioClip hurt;
    public AudioClip walk;


    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = 100;
        //currHealth = maxHealth;
        spawn = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckHealth();
    }


    // Methods

    public void CheckHealth(float currHealth, float maxHealth)
    {
        if(currHealth >= maxHealth)
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

    public override void Die()
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
