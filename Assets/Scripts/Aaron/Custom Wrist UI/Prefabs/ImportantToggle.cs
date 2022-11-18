// Aaron Williams
// 11/16/2022

using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class ImportantToggle : MonoBehaviour
{
    WristUI ui;
    Menu menu;
    GameObject grabRay;
    public InputActionReference toggleUI;
    public InputActionReference toggleMenu;
    public InputActionReference toggleGrabRay;
    // Start is called before the first frame update
    void Start()
    {
        toggleGrabRay.action.performed += RightPrimaryButtonPressed;
        grabRay = GameObject.Find("XR Origin/Camera Offset/RightHand Controller/Grab Ray");

        toggleUI.action.performed += LeftPrimaryButtonPressed;
        ui = FindObjectOfType<WristUI>();

        toggleMenu.action.performed += LeftMenuButtonPressed;
        menu = FindObjectOfType<Menu>();


        ui.ToggleVisibility();
        menu.ToggleVisibility();
        grabRay.SetActive(false);
    }

    void Update()
    {
        if(grabRay == null)
        {
            grabRay = GameObject.Find("Main Camera/Camera Offset/RightHand Controller/Grab Ray");
            grabRay.SetActive(false);
        }
        if (ui == null)
        {
            ui = FindObjectOfType<WristUI>();
            ui.ToggleVisibility();
        }
        if (menu == null)
        {
            menu = FindObjectOfType<Menu>();
            menu.ToggleVisibility();
        }
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
