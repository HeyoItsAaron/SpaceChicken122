using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTrigger : MonoBehaviour
{
    public BoxCollider hitBoxCollider;
    public GameObject thisDude;
    public GameObject thisDudesRig;
    //public Animator thisDudesAnimator;

    //these are for the colliders and rigidbodies of the ragdoll
    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        getRagdollComponents();
        RagdollModeOff();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "HAMMER" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Dummy")
        {
            RagdollModeOn();
        }
    }

    // this grabs the colliders and rigidbodies from this dude
    void getRagdollComponents()
    {
        ragdollColliders = thisDudesRig.GetComponentsInChildren<Collider>();
        ragdollRigidbodies = thisDudesRig.GetComponentsInChildren<Rigidbody>();

    }

    void RagdollModeOn()
    {
        //enables animator
        //thisDudesAnimator.enabled = false;

        //enables colliders
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = true;
        }

        //"enables" rigidbodies
        foreach (Rigidbody rigidbody in ragdollRigidbodies)
        {
            //kinematic means they won't be moving around or affecting this dude
            rigidbody.isKinematic = false;
        }

        //emables hit box collider
        hitBoxCollider.enabled = false;
        //enables main rigidbody
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(despawnAfterSeconds());

    }
    void RagdollModeOff()
    {
        //disables colliders
        foreach (Collider collider in ragdollColliders)
        {
            collider.enabled = false;
        }

        //"disables" rigidbodies
        foreach (Rigidbody rigidbody in ragdollRigidbodies)
        {
            //kinematic means they won't be moving around or affecting this dude
            rigidbody.isKinematic = true;
        }

        //enables animator
        //thisDudesAnimator.enabled = true;
        //emables hit box collider
        hitBoxCollider.enabled = true;
        //enables main rigidbody
        GetComponent<Rigidbody>().isKinematic = false;

    }

    IEnumerator despawnAfterSeconds()
    {
        yield return new WaitForSeconds(5);
        Destroy(thisDude);
    }


}
