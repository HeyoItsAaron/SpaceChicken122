// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

// Inherit from Chicken Stats 
public class Chicken : ChickenStats
{
    // variables
    private Rigidbody[] rbs;
    public Transform myTarget;
    public Transform currentTarget;
    public NavMeshAgent myAgent;
    public int range;
    public float distance;
    [SerializeField] float stoppingDistance;
    Animator anim;
    float lastAttackTime = 0;
    float attackCooldown = 2;
    private int damage = 10;
    Spawner spawn;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DistCheck", 0, 0.5f);
        myAgent = GetComponent<NavMeshAgent>();
        rbs = GetComponentsInChildren<Rigidbody>();
        DisactivateRagdoll();
        anim = GetComponent<Animator>();
        maxHealth = 100;
        currHealth = maxHealth;
        myTarget = GameObject.FindWithTag("Player").transform;
        spawn = FindObjectOfType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        //atransform.LookAt(myTarget);

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
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            currentTarget.GetComponent<TestPlayer>().TakeDamage(damage);
            currentTarget.GetComponent<TestPlayer>().CheckHealth();
        }
        
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
        myAgent.enabled = false;
        anim.SetBool("isDead", true);
        Destroy(gameObject, 5);
        spawn.enemiesKilled++;
    }

    public void TakeDamage(int damage)
    {
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
    }

}
