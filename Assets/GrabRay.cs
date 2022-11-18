using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRay : MonoBehaviour
{
    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
