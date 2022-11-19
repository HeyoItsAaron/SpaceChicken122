using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTutorial : MonoBehaviour
{
    public GameObject tutorial;
    // Start is called before the first frame update
    void Start()
    {
        tutorial = GameObject.Find("Tutorial");
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorial == null)
            tutorial = GameObject.Find("Tutorial");
    }

    public void ToggleTutorialOnButton()
    {
        tutorial.SetActive(!tutorial.activeInHierarchy);
    }
}
