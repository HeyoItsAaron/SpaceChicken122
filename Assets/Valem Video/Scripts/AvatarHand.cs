//Aaron Williams
//10/28/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AvatarHand : MonoBehaviour
{
    Animator avatarAnimator;
    public string triggerPullParameter;
    public string grabPressParameter;
    public string triggerTouchLayer;
    public string ThumbTouchLayer;
    public string Controller;


    // Start is called before the first frame update
    void Start()
    {
        avatarAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    internal void SetGrip()
    {

    }

    internal void SetTrigger()
    {

    }

    internal void SetGripTouch()
    {

    }

    internal void SetTriggerTouch()
    {

    }

    void animateHand()
    {
        if()
    }
}
