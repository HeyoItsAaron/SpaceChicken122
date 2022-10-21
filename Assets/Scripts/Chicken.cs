using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Chicken : ChickenStats
{
    private Rigidbody[] rbs;
    public Transform myTarget;
    public Transform currentTarget;
    public NavMeshAgent myAgent;
    public int range;
    public float distance;
    //public Vector3 startPos;
    [SerializeField] float stoppingDistance;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DistCheck", 0, 0.5f);
        //startPos = this.transform.position;
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
        /*
        if (currentTarget != null)
        {
            myAgent.destination = currentTarget.transform.position;
        }
        else if (myAgent.destination != startPos)
        {
            myAgent.destination = startPos;
            anim.SetBool("isWalking", true);
        }
        */

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

        if (currHealth >= maxHealth)
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


    void ActivateRagdoll()
    {
        foreach (var item in rbs)
        {
            item.isKinematic = false;
        }
    }

    void DisactivateRagdoll()
    {
        foreach (var item in rbs)
        {
            item.isKinematic = true;
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
        /*
        else if (dist > range)
        {
            currentTarget = null;
            StopEnemy();
        }
        */
    }
    

    private void StopEnemy()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        myAgent.enabled = false;
        

    }

    private void Attack()
    {
        anim.SetBool("isAttacking", true);
        myAgent.enabled = false;
    }

    private void FindTarget()
    {
        myAgent.enabled = true;
        anim.SetBool("isWalking", true);
        anim.SetBool("isAttacking", false);
        myAgent.SetDestination(myTarget.transform.position);
        
    }

    public override void Die()
    {
        myAgent.enabled = false;
        anim.SetBool("isDead", true);
        Destroy(gameObject, 5);
    }

}
