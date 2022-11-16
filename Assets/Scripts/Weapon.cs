using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed = 20;
    public float ammoCost = 10;
    public AudioSource gunshot;
    public AudioClip clip;
    public float volume = 0.5f;

    public ParticleSystem muzzleFlash;



    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
        muzzleFlash.Stop();
    }

    void Update()
    {
        
    }

    public void FireBullet(ActivateEventArgs arg)
    {

        muzzleFlash.Play();
        gunshot.PlayOneShot(clip, volume);
        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        spawnedBullet.transform.position = spawnPoint.position;
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnedBullet, 5);
    }  

}
