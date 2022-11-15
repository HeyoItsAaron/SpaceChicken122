using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggle : MonoBehaviour
{
    WristUI ui;
    Menu menu;
    public InputActionReference toggleUI;
    public InputActionReference toggleMenu;
    // Start is called before the first frame update
    void Start()
    {
        toggleUI.action.performed += LeftPrimaryButtonPressed;
        ui = GameObject.FindObjectOfType<WristUI>();

        toggleMenu.action.performed += LeftMenuButtonPressed;
        menu = GameObject.FindObjectOfType<Menu>();
    }


    void LeftPrimaryButtonPressed(InputAction.CallbackContext context)
    {
        ui.ToggleVisibility();
    }
    void LeftMenuButtonPressed(InputAction.CallbackContext context)
    {
        menu.ToggleVisibility();
    }

}
