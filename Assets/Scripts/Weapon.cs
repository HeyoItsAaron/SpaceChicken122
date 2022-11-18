using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.VisualScripting.Member;

public class Weapon : MonoBehaviour
{
    //Base variables
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed;
    public float ammoCost;

    //Shotgun
    public bool isShotgun;
    public float spread;
    public int bulletsFired;

    //AutoFire
    public bool isAuto;
    public float timeBetweenShots;

    //Audio
    public AudioSource gunshot;
    public AudioClip clip;
    public float volume = 0.5f;

    // Effects
    public ParticleSystem muzzleFlash;



    void Start()
    {
        // Once weapon is grabbed, waits for trigger to be pressed
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
        // Muzzle flash is enabled on start
        muzzleFlash.Stop();
    }

    void Update()
    {

    }

    void FireBullet(ActivateEventArgs arg)
    {
        if (isAuto == true)
        {
            Invoke("Shoot()", timeBetweenShots);
        }
        else
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //if(ammo > 1) Sample ammo check
        {
            // Makes an object of the audio source to destory to prevent it spawning infinite times
            AudioSource newAS = Instantiate(gunshot);
            newAS.PlayOneShot(clip, volume);
            Destroy(newAS.gameObject, 3);

            // Muzzle Flash
            muzzleFlash.Play();

            if (isShotgun == true)
            {
                for (int i = 0; i < bulletsFired; i++)
                {
                    //Random spread
                    float x = UnityEngine.Random.Range(-spread, spread);
                    float y = UnityEngine.Random.Range(-spread, spread);
                    Vector3 Direction = spawnPoint.transform.position + new Vector3(x, y, 0);

                    // Bullet Spawning
                    GameObject spawnedBullet = Instantiate(bullet, Direction, spawnPoint.transform.rotation);
                    spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
                    Destroy(spawnedBullet, 5);
                }

            }
            else
            {
                GameObject spawnedBullet = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
                spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
                Destroy(spawnedBullet, 5);
            }

            //ammo = ammo - ammoCost; sample ammo update on shoot
        }
    }

}