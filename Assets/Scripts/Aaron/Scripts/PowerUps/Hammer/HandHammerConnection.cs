// Aaron Williams
// 11/3/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class HandHammerConnection : MonoBehaviour
{
    //variables
    //public InputActionReference leftTriggerPull;
    public InputActionReference rightTriggerPull;
    public Hammer hammer;
    //public bool isHammerTime = true;

    // Start is called before the first frame update
    void Start()
    {
        hammer = GameObject.FindObjectOfType<Hammer>();
        rightTriggerPull.action.performed += RightTriggerPulled;
        //leftTriggerPull.action.performed += LeftTriggerPulled;
    }

    // Update is called once per frame
    void Update()
    {
        if(hammer == null)
        {
            hammer = GameObject.FindObjectOfType<Hammer>();
        }
    }
    /*
    void TriggerPulledWhileHammerTime()
    {
        /*
        if ((isHammerTime == true) && (leftTriggerPull.action.ReadValue<float>() > 0))
        {
            hammer.ReturnHammer();
        }
        
    }
    */
    void RightTriggerPulled(InputAction.CallbackContext context)
    {
        hammer.ReturnHammer();
    }
    //void LeftTriggerPulled(InputAction.CallbackContext context)
    //{
    //    hammer.ReturnHammer();
    //}

}