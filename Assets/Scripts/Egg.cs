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
    public EggChicken EggChicken;
    [SerializeField] int damage;
    public Transform currrentTarget;


    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Collider>().enabled = true;
        rb = GetComponent<Rigidbody>();
    }

    private void update()
    {
        currrentTarget = EggChicken.currentTarget;
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
            currrentTarget.GetComponent<TestPlayer>().TakeDamage(damage);
            currrentTarget.GetComponent<TestPlayer>().CheckHealth();
        }
        
    }

}
