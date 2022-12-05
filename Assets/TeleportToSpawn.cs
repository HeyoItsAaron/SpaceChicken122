using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider col)
    {
        col.Transform.position = new Vector3(-25.33, 1.56, 101.11);
    }
}
