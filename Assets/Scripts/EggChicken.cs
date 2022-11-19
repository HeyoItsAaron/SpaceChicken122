// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;
using Vector3 = UnityEngine.Vector3;

// Inherit from Chicken Stats 
public class EggChicken : ChickenStats
{
    // variables
    private Rigidbody[] rbs;
    [SerializeField] float stoppingDistance;
    Animator anim;
    [SerializeField] int damage;
    NetworkSpawner spawn;
    private NetworkPlayer[] networkPlayers;
    public NetworkPlayer closerPlayer;
    public float turnRate;

    //shooting variables
    public Transform spawnPoint;
    float speed = 5;
    float fireRate = 3f;
    float waitTime = 1.8f;
    public GameObject egg;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("offset", Random.Range(0.0f, 1.0f));
        InvokeRepeating("DistCheck", 0, 0.5f);
        myAgent = GetComponent<NavMeshAgent>();
        rbs = GetComponentsInChildren<Rigidbody>();
        anim = GetComponent<Animator>();
        maxHealth = 100;
        currHealth = maxHealth;
        myTarget = GameObject.FindWithTag("Player").transform;
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
        Search();

        CheckHealth(currHealth, maxHealth);
        fireRate -= Time.deltaTime;

        // If else statements to check distance from enemy and call certain method
        if (distance < stoppingDistance)
        {
            if (fireRate <= 0)
            {
                waitTime -= Time.deltaTime;
                Attack();
                if(waitTime <= 0)
                {
                    FireEgg(currentTarget);
                    fireRate = 3f;
                    waitTime = 1.3f;
                }        
            }
        }

        else
        {
            if (distance > range)
            {
                StopEnemy();
                waitTime = 1.8f;
            }
            if (distance < range)
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
            if (Vector3.Distance(networkPlayers[i].head.position, transform.position) > Vector3.Distance(closerPlayer.head.position, transform.position))
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
    }

    // spawn egg  
    public void FireEgg(Transform target)
    {
        GameObject spawnedEgg = Instantiate(egg);
        spawnedEgg.transform.position = Vector3.Lerp(spawnPoint.position, target.transform.position, speed * Time.deltaTime);
        spawnedEgg.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(egg, 5);
    } 

    // Find current target
    private void FindTarget()
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
        StartCoroutine(despawnAfterSeconds());
        //spawn.enemiesKilled++;
        spawn.enemyAmount--;
    }

    // RPC Damage function for chicken damage over network
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
        if (collision.gameObject.tag == "HAMMER")
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
