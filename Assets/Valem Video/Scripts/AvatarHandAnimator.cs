//Aaron Williams
//10/28/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static Zinnia.Rule.DominantControllerRule;

//THIS SCRIPT ATTACHES TO THE AVATAR TO CONTROL HAND ANIMATIONS

public class AvatarHandAnimator : MonoBehaviour
{

    //variables

    //this says the characteristics I'm looking for are tracked in 3d space AND handheld, eliminating the possibility for the headset to be picked up
    private InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand;
    private InputDevice targetDevice;
    private Animator avatarAnimator;
    public AvatarHand hand;
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
        //checks controller inputs

        //Trigger Pull
            //checks trigger input to set animator
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerPullValue))
        {
            avatarAnimator.SetFloat(hand.triggerPullParameter, triggerPullValue);
            Debug.Log(targetDevice.name + targetDevice.characteristics + "Has pulled the left trigger");
        }
        else
        {
            avatarAnimator.SetFloat(hand.triggerPullParameter, 0);
        }

        //Grip
            //checks left grab input to set animator
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float GripPressValue))
        {
            avatarAnimator.SetFloat(hand.grabPressParameter, GripPressValue);
        }
        else
        {
            avatarAnimator.SetFloat(hand.grabPressParameter, 0);
        }

        //pointer finger
            //checks whether or not the trigger button is touched, if not then it points
        if (!targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isTriggerTouched))
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex(hand.triggerTouchLayer), 1);
        }
        else
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex(hand.triggerTouchLayer), 0);
        }

        //thumb
            //checks whether or not the joystick button is touched, if not then it puts a thumbs up
        if (!targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool isJoystickTouched))
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex(hand.ThumbTouchLayer), 1);
        }
        else
        {
            avatarAnimator.SetLayerWeight(avatarAnimator.GetLayerIndex(hand.ThumbTouchLayer), 0);
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
