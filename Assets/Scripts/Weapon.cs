using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed = 20;

    public ParticleSystem muzzleFlash;



    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    void Update()
    {
        
    }

    public void FireBullet(ActivateEventArgs arg)
    {
        // Spawns a clone of the gameobject bullet
        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        // Sets the cloned bullet's position to the spawnPoint (in front of muzzle)
        spawnedBullet.transform.position = spawnPoint.position;
        // Adds the a velocity in the forward direction of the bullet
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnedBullet, 5);
        muzzleFlash.Play();
    }

}
