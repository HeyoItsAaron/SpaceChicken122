using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject Wall_Decoration_002(1)

    // Update is called once per frame
    void Start()
    {
        var random = new Random();
        var zlist = new list<double> { 75.72f, 89.04f, 103.05 };
        var xlist = new list<double> { -68.93f , -80.66f, -56.04f };
        double z = random.Next(zlist);
        double x = random.Next(xlist);
        Vector 3 randomSpawnPosition = new Vector3(x, 6.27f, z);
        Istantiate(Wall_Decoration_002(1), randomSpawnPosition, Quaternion.identity);
    }
}
