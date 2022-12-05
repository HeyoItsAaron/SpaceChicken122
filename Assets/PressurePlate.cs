using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
   public static int keyCount;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "player")
        {
            keyCount++;
            Destroy (gameObject);
        }
    }
}
