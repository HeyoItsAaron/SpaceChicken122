// Aaron Williams
// 11/3/2022

using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum State { Throw, Return, Idle}
public class Hammer : MonoBehaviour
{
    //variables 
    public float hammerSpeed;
    Rigidbody hammerRigidBody;
    State state;
    PlayerStats player;
    public Transform playerHand;
    public bool hasTouchedHammer;

    //built-in methods
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        state = State.Idle;
        hammerRigidBody = GetComponent<Rigidbody>();
        XROrigin rig = FindObjectOfType<XROrigin>();
        playerHand = rig.transform.Find("Camera Offset/RightHand Controller");
        hasTouchedHammer = false;
    }

    void Update()
    {
        switch (state)
        {
            case State.Throw:
                {
                    hammerRigidBody.velocity = hammerRigidBody.velocity.normalized * hammerSpeed;
                    if (hasTouchedHammer == false)
                        hasTouchedHammer = true;
                }

                break;
            case State.Return:
                {
                    if(Vector3.SqrMagnitude(playerHand.position - transform.position) > 4)
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
    public void SetItemInHand()
    {
        player.hasItemInHand = true;
        StartCoroutine(WaitSeconds());
    }
    public void SetHandEmpty()
    {
        player.hasItemInHand = false;
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Grab Ray").SetActive(false);
    }

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