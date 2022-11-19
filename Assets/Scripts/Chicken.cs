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
    NetworkSpawner spawn;
    public float turnRate;
    private NetworkPlayer[] networkPlayers;
    public NetworkPlayer closerPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("offset", Random.Range(0.0f, 1.0f));
        myAgent = GetComponent<NavMeshAgent>();
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        maxHealth = 100;
        currHealth = maxHealth;

        spawn = FindObjectOfType<NetworkSpawner>();
        networkPlayers = FindObjectsOfType<NetworkPlayer>();
        myTarget = networkPlayers[0].head;
        currentTarget = myTarget;
        distance = Vector3.Distance(networkPlayers[0].head.position, transform.position);
        closerPlayer = networkPlayers[0];
    }

    // Update is called once per frame
    void Update()
    {
        // run the search every frame
        Search();

        // Check the helth of the chicken every frame
        CheckHealth(currHealth, maxHealth);

        // If the distance from target is less than stopping distance attack
        if ( distance < stoppingDistance)
        {
            Attack();
        }

        else
        {
            // if distance greater than range stop enemy
            if (distance > range)
            {
                StopEnemy();
            }
            // otherwise search for target
            if (distance < range || currentTarget.IsDestroyed())
            {
                FindTarget();
            }
        }
    }


    // methods 

    // check distance from enenmy
    void Search()
    {
        for (int i = 0; i < networkPlayers.Count(); i++)
        {
            if (Vector3.Distance(networkPlayers[i].head.position, transform.position) < Vector3.Distance(closerPlayer.head.position, transform.position))
                closerPlayer = networkPlayers[i];
        }
        currentTarget = closerPlayer.head;
        distance = Vector3.Distance(currentTarget.position, transform.position);
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
            closerPlayer.GetComponent<NetworkPlayer>().TakePeck();
            //currentTarget.GetComponent<TestPlayer>().CheckHealth();
        }
    }
    
    // Find current target
    private void FindTarget()
    {
        myAgent.enabled = true;
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
        myAgent.SetDestination(currentTarget.transform.position); 
    }

    // Overwritten Die method
    public override void Die()
    {
        AudioSource.PlayClipAtPoint(deathClip, transform.position); 
        myAgent.enabled = false;
        anim.SetBool("isDead", true);
        StartCoroutine(despawnAfterSeconds());
        //spawn.enemiesKilled++;
        spawn.enemyAmount--;
    }

    // RPC Damage function for chiken damage
    [PunRPC]
    public void DoDamage(float damageAmount)
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

    // On bullet collision have chicken take damage
    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.tag == "Light Bullet")
        {
            gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 15f);
        }
        if (collision.gameObject.tag == "Medium Bullet")
        {
            gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 25f);
        }
        if (collision.gameObject.tag == "Heavy Bullet")
        {
            gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 35f);
        }
        if (collision.gameObject.tag ==  "HAMMER")
        {
            gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 50f);
        }
    }

    // Allows for a wait time for Chicken animation to play before despawn
    IEnumerator despawnAfterSeconds()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }

}
