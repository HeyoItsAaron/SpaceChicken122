using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCheck : MonoBehaviour
{
    public bool hasItemInHand;
    public void Start()
    {
        hasItemInHand = false;
    }
    public void SetItemInHand()
    {
        hasItemInHand = true;
        if(GameObject.Find("Grab Ray").activeInHierarchy)
            StartCoroutine(WaitSeconds());
    }
    public void SetHandEmpty()
    {
        hasItemInHand = false;
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Grab Ray").SetActive(false);
    }
}
