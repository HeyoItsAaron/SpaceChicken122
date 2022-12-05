using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToArea1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void  OnTriggerEven(Collider Col)
    {
        Col.transform.position = new Vector3 (123.8f, 9.5f, 86.72f);
    }
}
