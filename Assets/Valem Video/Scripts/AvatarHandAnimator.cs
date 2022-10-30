//Aaron Williams
//10/28/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//THIS SCRIPT ATTACHES TO THE AVATAR TO CONTROL HAND ANIMATIONS

public class AvatarHandAnimator : MonoBehaviour
{

    //variables

    //this says the characteristics I'm looking for are tracked in 3d space AND handheld, eliminating the possibility for the headset to be picked up
    private InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand;

    private InputDevice targetDevice;
    private Animator avatarAnimator;
    List<InputDevice> vrHandControllers = new List<InputDevice>();

    //methods

    // Start is called before the first frame update
    void GetControllers()
    {
        //makes an empty list of an input device with the same characteristics that aren't mentioned yet
        //List<InputDevice> vrHandControllers = new List<InputDevice>();

        //fills the above list with devices having a couple similar characteristics as defined in desiredCharacteristics
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, vrHandControllers);
        foreach (var controller in vrHandControllers)
        {
            Debug.Log(controller.name + controller.characteristics);
        }
    }

    // Update is called once per frame
    void UpdateHandAnimation()
    {
        if(targetDevice.characteristics == InputDeviceCharacteristics.Left)
        {
            //checks left controller inputs

            //Trigger Pull
                //checks left trigger input to set animator
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerPullValue))
            {
                avatarAnimator.SetFloat("Left Trigger", triggerPullValue);
            }
            else
            {
                avatarAnimator.SetFloat("Left Trigger", 0);
            }

            //Grip
                //checks left grab input to set animator
            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float GripPressValue))
            {
                avatarAnimator.SetFloat("Left Grab", GripPressValue);
            }
            else
            {
                avatarAnimator.SetFloat("Left Grab", 0);
            }

            //pointer finger
                //checks whether or not the trigger button is touched, if not then it points
            if (!targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerTouched))
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Point Layer"), 1);
            }
            else
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Point Layer"), 0);
            }

            //thumb
                //checks whether or not the joystick button is touched, if not then it puts a thumbs up
            if (!targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool isJoystickTouched))
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Thumb Layer"), 1);
            }
            else
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Left Thumb Layer"), 0);
            }
        }

        //checks right controller inputs

        if (targetDevice.characteristics == InputDeviceCharacteristics.Right)
        {
            //checks left controller inputs

            //Trigger Pull
                //checks left trigger input to set animator
            if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerPullValue))
            {
                avatarAnimator.SetFloat("Right Trigger", triggerPullValue);
            }
            else
            {
                avatarAnimator.SetFloat("Right Trigger", 0);
            }

            //Grip
                //checks left grab input to set animator
            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float GripPressValue))
            {
                avatarAnimator.SetFloat("Right Grab", GripPressValue);
            }
            else
            {
                avatarAnimator.SetFloat("Right Grab", 0);
            }

            //pointer finger
                //checks whether or not the trigger button is touched, if not then it points
            if (!targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerTouched))
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Point Layer"), 1);
            }
            else
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Point Layer"), 0);
            }

            //thumb
                //checks whether or not the joystick button is touched, if not then it puts a thumbs up
            if (!targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool isJoystickTouched))
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Thumb Layer"), 1);
            }
            else
            {
                avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex("Right Thumb Layer"), 0);
            }
        }
        
    }
    void Start()
    {
        //grabs the animator of the avatar
        avatarAnimator = GetComponent<Animator>();
        GetControllers();
    }
    void update()
    {
                UpdateHandAnimation();

    }
}
