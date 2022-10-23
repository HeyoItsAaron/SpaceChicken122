//Aaron Williams
//10/20/2022
//tutorial used: https://www.youtube.com/watch?v=8REDoRu7Tsw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GOAL IS TO USE HEADSET TO CHANGE DIRECTION X AND Y PARAMETERS IN AMINATOR FOR ROBOT KYLE

public class VRAnimatorController : MonoBehaviour
{
    //private variables

    private Animator animator;
    private Vector3 previousPosition;
    private VRRig vrRig; //this will give us vr headset reference; we created this script in Tutorial 1

    //Public variables

    public float speedThreshold = 0.1f;

    [Range(0,1)]
    public float smoothing = 1;

    // Start is called before the first frame update
    void Start()
    {
        //initialize variables
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previousPosition = vrRig.head.vrTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Compute the speed of the head
        Vector3 headsetSpeed = vrRig.head.vrTarget.position - previousPosition / Time.deltaTime;

        //since we only care about horizontal speed, so we set the vertical speed to 0
        headsetSpeed.y = 0;

        //Local Speed
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPosition = vrRig.head.vrTarget.position;

        //Set Animator Values
        float previousDirectionX = animator.GetFloat("DirectionX");
        float previousDirectionY = animator.GetFloat("DirectionY");

        animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedThreshold);

        //dont want it to go above 1 so it gets clamped
        animator.SetFloat("DirectionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
        animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));
    }
}
