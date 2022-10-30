using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class AvatarHandController : MonoBehaviour
{
    ActionBasedController controller;
    AvatarHand hand;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
        hand.SetGripTouch(controller.selectAction.action.ReadValue<bool>());
        hand.SetTriggerTouch(controller.activateAction.action.ReadValue<bool>());
        //controller.TryGetFeatureValue(CommonUsages.trigger, out float triggerPullValue);
    }
}
