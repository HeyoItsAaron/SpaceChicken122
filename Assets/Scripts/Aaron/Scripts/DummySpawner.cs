//Aaron Williams
//11/3/2022
//inpsired vby Aidens spawn script

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    // variables
    public GameObject spawner;
    public GameObject dummy;
    private GameObject myDummy;

    // Start is called before the first frame update
    private void Start()
    {
        SpawnDummy();
    }

    // Update is called once per frame
    void Update()
    {
        if (myDummy.IsDestroyed() == true)
        {
            SpawnDummy();
        }
    }

    // spawn method
    private void SpawnDummy()
    {
        myDummy = Instantiate(dummy, spawner.transform.position, spawner.transform.rotation);
    }
}