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
    GrabRay grabRay;
    public InputActionReference toggleUI;
    public InputActionReference toggleMenu;
    public InputActionReference toggleGrabRay;
    // Start is called before the first frame update
    void Start()
    {
        toggleGrabRay.action.performed += RightPrimaryButtonPressed;
        grabRay = FindObjectOfType<GrabRay>();

        toggleUI.action.performed += LeftPrimaryButtonPressed;
        ui = FindObjectOfType<WristUI>();

        toggleMenu.action.performed += LeftMenuButtonPressed;
        menu = FindObjectOfType<Menu>();

        if(ui != null)
            ui.ToggleVisibility();
        if (menu != null)
            menu.ToggleVisibility();
        GameObject.Find("Grab Ray").SetActive(false);
    }

    void Update()
    {
        if(grabRay is null)
        {
            grabRay = FindObjectOfType<GrabRay>();
        }
        if (ui is null)
        {
            ui = FindObjectOfType<WristUI>();
            if(ui = FindObjectOfType<WristUI>())
                ui.ToggleVisibility();
        }
        if (menu is null)
        {
            menu = FindObjectOfType<Menu>();
            if(menu = FindObjectOfType<Menu>())
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
