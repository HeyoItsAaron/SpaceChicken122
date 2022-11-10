using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggle : MonoBehaviour
{
    WristUI ui;
    public InputActionReference toggleUI;
    // Start is called before the first frame update
    void Start()
    {
        toggleUI.action.performed += RightTriggerPulled;
        ui = GameObject.FindObjectOfType<WristUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RightTriggerPulled(InputAction.CallbackContext context)
    {
        ui.ToggleVisibility();
    }
}
