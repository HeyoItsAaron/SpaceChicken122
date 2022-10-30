// Written by Aidan Urbina

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyNavMesh : MonoBehaviour
{
    // Variables
    public GameObject myTarget;
    public GameObject currentTarget;
    public NavMeshAgent myAgent;
    public int range;
    public int tetherRange;
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DistCheck", 0, 0.5f);
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null)
        {
            myAgent.destination = currentTarget.transform.position;
        }
        else if (myAgent.destination != startPos)
        {
            myAgent.destination = startPos;
        }
    }

    // Check distance from current target
    public void DistCheck()
    {
        float dist = Vector3.Distance(this.transform.position, myTarget.transform.position);

        if (dist < range)
        {
            currentTarget = myTarget;
        }

        else if (dist > tetherRange)
        {
            currentTarget = null;
        }
    }
}
