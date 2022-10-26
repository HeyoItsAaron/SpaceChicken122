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
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        transform.LookAt(myTarget);

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
    }

}
