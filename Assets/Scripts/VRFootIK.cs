//Aaron Williams
//10/20/2022
//tutorial used: https://www.youtube.com/watch?v=Wk2_MtYSPaM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFootIK : MonoBehaviour
{
    //private variables

    private Animator animator;

    //public variables

    public Vector3 footOffset;

    [Range(0,1)]
    public float rightFootPositionWeight = 1;
    [Range(0, 1)]
    public float leftFootPositionWeight = 1;

    [Range(0, 1)]
    public float rightFootRotationWeight = 1;
    [Range(0, 1)]
    public float leftFootRotationWeight = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //adjusts feet to not go inside of ground
    private void OnAnimatorIK(int layerIndex)
    {
        //right foot

        //get positions of feett
        Vector3 rightFootPosition = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        Vector3 leftFootPosition = animator.GetIKPosition(AvatarIKGoal.LeftFoot);

        //get ground position with raycast going down from 1 meter above foot
        RaycastHit hit;

        //check if we hit the ground
        bool hasHit = Physics.Raycast(rightFootPosition + Vector3.up, Vector3.down, out hit);

        //if true, we set IK prosition of the right foot by first giving the weight AKA amount of influence the IK will affect the animation
        if (hasHit)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPositionWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);

            //gets target rotation where foot lays flat on the ground
            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);

            //sets rotation to target (above)
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotationWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            //if there's no ground, set weight to zero
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }


        //same stuff but for left foot
        hasHit = Physics.Raycast(leftFootPosition + Vector3.up, Vector3.down, out hit);

        if (hasHit)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPositionWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + footOffset);

            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotationWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            //if there's no ground, set weight to zero
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }
}
