// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CSCore;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

// Inherit from Chicken Stats 
public class Chicken : ChickenStats
{
    // variables
    private Rigidbody[] rbs;
    [SerializeField] float stoppingDistance;
    Animator anim;
    float lastAttackTime = 0;
    float attackCooldown = 2;
    private int damage = 10;
    Spawner spawn;
    public float turnRate;
    private NetworkPlayer[] networkPlayers;

    // test variables
    int lightCount = 0;
    int mediumCount = 0;
    int heavyCount = 0;
    int hammerCount = 0;
    int fallingCount = 0;
    PhotonView PV;


    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Animator>().SetFloat("offset", Random.Range(0.0f, 1.0f));
        InvokeRepeating("DistCheck", 0, 0.5f);
        myAgent = GetComponent<NavMeshAgent>();
        rbs = GetComponentsInChildren<Rigidbody>();
        DisactivateRagdoll();
        anim = GetComponent<Animator>();
        maxHealth = 100;
        currHealth = maxHealth;
        myTarget = GameObject.FindWithTag("Player").transform;
        spawn = FindObjectOfType<Spawner>();

        networkPlayers = FindObjectsOfType<NetworkPlayer>();

    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z));

        CheckHealth(currHealth, maxHealth);

        // If else statements to check distance from enemy and call certain method

        if ( distance < stoppingDistance)
        {
            Attack();
        }

        else
        {
            if (distance > range)
            {
                StopEnemy();
            }

            if (distance < range)
            {
                //FindTarget();
                Search();
            }
            if(currentTarget.IsDestroyed())
            {
                //FindTarget();
                Search();
            }
        }
    }


    // methods 

    public void ActivateRagdoll()
    {
        foreach (var item in rbs)
        {
            item.isKinematic = false;
        }
    }

    public void DisactivateRagdoll()
    {
        foreach (var item in rbs)
        {
            item.isKinematic = true;
        }
    }

    // check distance from enenmy
    
    void Search()
    {
        for (int i = 0; i < networkPlayers.Count(); i++)
        {
            float DistanceFromPlayer = Vector3.Distance(networkPlayers[i].head.position, transform.position);

            if (DistanceFromPlayer <= range)
            {
                FindTarget(i);
            }
        }
    }
    
    
    public void DistCheck()
    {
         float dist = Vector3.Distance(this.transform.position, myTarget.transform.position);

        if (dist < range)
        {
            currentTarget = myTarget;
            distance = dist;
        }
    }


    // Stop enemy

    private void StopEnemy()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        myAgent.enabled = false;
        

    }

    // Attack player

    private void Attack()
    {
        anim.SetBool("isAttacking", true);
        myAgent.enabled = false;
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            currentTarget.GetComponent<TestPlayer>().TakeDamage(damage);
            currentTarget.GetComponent<TestPlayer>().CheckHealth();
        }
        
    }
    
    // Find current target

    private void FindTarget(int num)
    {
        myAgent.enabled = true;
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
        myAgent.SetDestination(myTarget.transform.position);
        
    }

    // Die method

    public override void Die()
    {
        AudioSource.PlayClipAtPoint(deathClip, transform.position); 
        myAgent.enabled = false;
        anim.SetBool("isDead", true);
        Destroy(gameObject, 5);
        spawn.enemiesKilled++;
    }

    public void TakeDamage(int damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        AudioSource.PlayClipAtPoint(hurt, transform.position);
        currHealth -= damage;
    }

    // old
    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "Light Bullet")
        {
            TakeDamage(15);
        }
        if (collision.gameObject.tag == "Medium Bullet")
        {
            TakeDamage(25);
        }
        if (collision.gameObject.tag == "Heavy Bullet")
        {
            TakeDamage(35);
        }
        if (collision.gameObject.tag ==  "HAMMER")
        {
            TakeDamage(50);
        }
    }

    // test this
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Light Bullet")
        {
            lightCount++;
            if(lightCount > 7)
            {
                Die();
            }
        }
        if (collision.gameObject.tag == "Medium Bullet")
        {
            mediumCount++;
            if (mediumCount > 5)
            {
                Die();
            }
        }
        if (collision.gameObject.tag == "Heavy Bullet")
        {
            heavyCount++;
            if (heavyCount > 3)
            {
                Die();
            }
        }
        if (collision.gameObject.tag == "HAMMER")
        {
            hammerCount++;
            if (lightCount > 2)
            {
                Die();
            }
        }
        if (collision.gameObject.tag == "FALLING HAMMER")
        {
            fallingCount++;
            if (fallingCount > 1)
            {
                Die();
            }
        }
    }
    */

}
