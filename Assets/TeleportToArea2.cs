using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToArea2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEvent(Collider Col)
    {
        Col.Transform.Position = new Vector3 (61.29214f, 16.16f, 311.88f);
    }
}
