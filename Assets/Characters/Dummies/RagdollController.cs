//Aaron Williams
//10/26/2022
//tutorial watched: https://www.youtube.com/watch?v=PVf8rQgcymQ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    //
    public Collider[] ragdollColliders;
    public Rigidbody[] ragdollRigidbodies;
    public GameObject BodyRig;
    public Animator BodyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //
    }

    void GetRagdollCompenents()
    {
        ragdollColliders = BodyRig.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = BodyRig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdollToggleOn()
    {
        //
    }
    void RagdollToggleOff()
    {
        //
    }
}
