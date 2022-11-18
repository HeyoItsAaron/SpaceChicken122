using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        if(gameObject.activeInHierarchy)
            GameObject.FindObjectOfType<HoldCheck>().hasItemInHand = true;
        if(!gameObject.activeInHierarchy)
            GameObject.FindObjectOfType<HoldCheck>().hasItemInHand = false;
    }
}
