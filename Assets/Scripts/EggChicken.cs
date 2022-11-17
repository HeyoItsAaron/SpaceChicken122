// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
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
    Spawner spawn;
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
        DisactivateRagdoll();
        anim = GetComponent<Animator>();
        maxHealth = 100;
        currHealth = maxHealth;
        distance = 100000000;
        myTarget = GameObject.FindWithTag("Player").transform;
        spawn = FindObjectOfType<Spawner>();

    }

    // Update is called once per frame
    void Update()
    {

        //transform.LookAt(new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z));


        //stoppingDistance = range;
        CheckHealth(currHealth, maxHealth);
        //transform.LookAt(currentTarget);
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
        //anim.SetBool("isAttacking", false);
        myAgent.SetDestination(myTarget.transform.position);

    }

    // Die method

    public override void Die()
    {
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
        myAgent.enabled = false;
        anim.SetBool("isDead", true);
        Destroy(gameObject, 5);
        if (spawn.enemiesKilled >= spawn.enemyAmount)
        {
            spawn.NextWave();
        }
        else
        {
            spawn.enemiesKilled++;
        }
    }

    public void TakeDamage(int damage)
    {
        AudioSource.PlayClipAtPoint(hurt, transform.position);
        currHealth -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.collider.gameObject.CompareTag("Light Bullet"))
        {
            TakeDamage(15);
        }
        if (collision.collider.gameObject.CompareTag("Medium Bullet"))
        {
            TakeDamage(25);
        }
        if (collision.collider.gameObject.CompareTag("Heavy Bullet"))
        {
            TakeDamage(35);
        }
        if (collision.collider.gameObject.CompareTag("HAMMER"))
        {
            TakeDamage(50);
        }
    }


}
