using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "player" && PressurePlate.keyCount > 0)
        {
            PressurePlate.keyCount--;
            Destroy(gameObject);
        }
    }
}
