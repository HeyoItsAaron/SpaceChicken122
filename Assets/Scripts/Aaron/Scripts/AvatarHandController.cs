using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarHandController : MonoBehaviour
{

    public InputActionProperty leftTriggerPull;
    public InputActionProperty leftGrabPull;
    public InputActionProperty leftPointTouch;
    public InputActionProperty leftThumbTouch;

    public InputActionProperty rightTriggerPull;
    public InputActionProperty rightGrabPull;
    public InputActionProperty rightPointTouch;
    public InputActionProperty rightThumbTouch;

    public Animator avatarAnimator;

    void UpdateLeftHand()
    {
        //set pinch
        float triggerValueLeft = leftTriggerPull.action.ReadValue<float>();
        avatarAnimator.SetFloat("Left Trigger", triggerValueLeft);

        //set grab
        float grabValueLeft = leftGrabPull.action.ReadValue<float>();
        avatarAnimator.SetFloat("Left Grab", grabValueLeft);

        /*
        //set point
        bool isPointTouchLeft = System.Convert.ToBoolean(leftPointTouch.action.ReadValue<float>());
        if (isPointTouchLeft != true)
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Point Layer"), 1);
        }
        else
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Point Layer"), 0);
        }

        //set thumb
        bool isThumbTouchLeft = System.Convert.ToBoolean(leftThumbTouch.action.ReadValue<float>());
        if (isThumbTouchLeft != true)
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Thumb Layer"), 1);
        }
        else
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Thumb Layer"), 0);
        }
        */

    }

    void UpdateRightHand()
    {
        //Right Hand
        float triggerValueRight = rightTriggerPull.action.ReadValue<float>();
        avatarAnimator.SetFloat("Right Trigger", triggerValueRight);

        float grabValueRight = rightGrabPull.action.ReadValue<float>();
        avatarAnimator.SetFloat("Right Grab", grabValueRight);

        /*
        bool isPointTouchRight = System.Convert.ToBoolean(rightPointTouch.action.ReadValue<float>());
        if (isPointTouchRight != true)
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Point Layer"), 1);
        }
        else
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Point Layer"), 0);
        }

        bool isThumbTouchRight = System.Convert.ToBoolean(rightThumbTouch.action.ReadValue<float>());
        if (isThumbTouchRight != true)
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Thumb Layer"), 1);
        }
        else
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Thumb Layer"), 0);
        }
        */
    }
    void FixedUpdate()
    {
        UpdateLeftHand();
        UpdateRightHand();
    }
}
