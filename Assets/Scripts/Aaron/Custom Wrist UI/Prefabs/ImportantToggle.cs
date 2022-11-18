// Aaron Williams
// 11/16/2022

using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImportantToggle : MonoBehaviour
{
    WristUI ui;
    Menu menu;
    GrabRay grabRay;
    public InputActionReference toggleUI;
    public InputActionReference toggleMenu;
    public InputActionReference toggleGrabRay;
    // Start is called before the first frame update
    void Start()
    {
        toggleGrabRay.action.performed += RightPrimaryButtonPressed;
        grabRay = GameObject.FindObjectOfType<GrabRay>();

        toggleUI.action.performed += LeftPrimaryButtonPressed;
        ui = GameObject.FindObjectOfType<WristUI>();

        toggleMenu.action.performed += LeftMenuButtonPressed;
        menu = GameObject.FindObjectOfType<Menu>();


        ui.ToggleVisibility();
        menu.ToggleVisibility();
        grabRay.ToggleVisibility();
    }


    void LeftPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        ui.ToggleVisibility();
    }
    void LeftMenuButtonPressed(InputAction.CallbackContext context)
    {
        menu.ToggleVisibility();
    }
    void RightPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        grabRay.ToggleVisibility();
    }
}
