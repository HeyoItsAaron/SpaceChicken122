using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCall : MonoBehaviour
{
    public 
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CallHammer();
        }
    }

    void CallHammer()
    {
        //call down hammer at power-up location

        //apply effect to player

        //remove power-up object
    }
}
