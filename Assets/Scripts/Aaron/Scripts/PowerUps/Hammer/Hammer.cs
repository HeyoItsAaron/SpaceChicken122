// Aaron Williams
// 11/3/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { Throw, Return, Idle}
public class Hammer : MonoBehaviour
{
    //variables 
    public float hammerSpeed;
    Rigidbody hammerRigidBody;
    State state;

    public Transform playerHand;

    //built-in methods
    void Start()
    {
        state = State.Idle;
        hammerRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Throw:
                {
                    hammerRigidBody.velocity = hammerRigidBody.velocity.normalized * hammerSpeed;
                }

                break;
            case State.Return:
                {
                    if(Vector3.SqrMagnitude(playerHand.position - transform.position) > 16)
                    {
                        Vector3 direction = (playerHand.position - transform.position).normalized;
                        hammerRigidBody.velocity = direction * hammerSpeed;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }

                break;
            case State.Idle:
                {
                    
                }

                break;
        }
    }
    //my methods

    public void ThrowHammer()
    {
        state = State.Throw;
        Debug.Log("Mjolnir was Thrown with a velocity of" + hammerRigidBody.velocity);
    }
    public void ReturnHammer()
    {
        state = State.Return;
        Debug.Log("Mjolnir was Returned to it's user");
    }
    public void IdleHammer()
    {
        state = State.Idle;
        Debug.Log("Mjolnir is idle");
    }

}