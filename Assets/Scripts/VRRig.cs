//Aaron Williams
//10/20/2022
//tutorial used: https://www.youtube.com/watch?v=tBYl-aSxUe0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    //public variables

    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    //methods

    //method sets targets to the rigs for vr with position offsets
    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset); //converting to a rotation value
    }
}
public class VRRig : MonoBehaviour
{
    //public variables
    public float turnSmoothness;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstraint;
    private Vector3 headBodyOffset; //initial difference between head and body //why is this private

    //methods

    // Start is called before the first frame update
    void Start()
    {
        //at start sets inital value for head-body offset
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void FixedUpdate() //bdoy no delay with head in FIXED, in LATE there is a little but it's classy?
    {
        //changes position of Game Object, in this case
        transform.position = headConstraint.position + headBodyOffset;

        //aligns forward axis of our character (blue) with the up axis of head of our character (green)
        transform.forward = Vector3.Lerp(transform.forward,
            Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness); //if you do just = headConstraint.up it will rotate on all axis and we just want the y.

        //this is so positions will always be mapped and match
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
