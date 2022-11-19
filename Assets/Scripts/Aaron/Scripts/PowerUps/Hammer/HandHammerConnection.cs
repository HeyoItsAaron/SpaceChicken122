// Aaron Williams
// 11/3/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class HandHammerConnection : MonoBehaviour
{
    // variables
    public InputActionReference rightTriggerPull;
    public Hammer hammer;
    
    // methods
    void Start()
    {
        hammer = GameObject.FindObjectOfType<Hammer>();
        rightTriggerPull.action.performed += RightTriggerPulled;
    }
    void Update()
    {
        if(hammer == null)
        {
            hammer = GameObject.FindObjectOfType<Hammer>();
        }
    }
    void RightTriggerPulled(InputAction.CallbackContext context)
    {
        if(hammer.hasTouchedHammer == true && GameObject.FindObjectOfType<HoldCheck>().hasItemInHand == false)
            hammer.ReturnHammer();
    }
}