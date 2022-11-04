using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Photon.Pun.Demo.Asteroids;
using Photon.Voice;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class Egg : MonoBehaviour
{
    [SerializeField]

    Rigidbody rb;

    [SerializeField]
    public Transform spawnPoint;
    public GameObject egg;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }




    /*
    public void ShootEgg(Transform target)
    {
        //GameObject spawnedEgg = Instantiate(egg);
        //spawnedEgg.transform.position = spawnPoint.position;
        //spawnedEgg.transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        //spawnedEgg.GetComponent<Rigidbody>().velocity = spawnPoint.forward * speed;
        //Destroy(spawnedEgg, 5);
        Physics.gravity = Vector3.up * this.gravityScale;
        rb.useGravity = true;

        //rb = GetComponent<Rigidbody>();
        Vector3 dir = target.position - transform.position;
        rb.AddForce(dir * speed);
    }
    */

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        
    }

}
