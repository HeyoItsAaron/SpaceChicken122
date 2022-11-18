using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using static Unity.VisualScripting.Member;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    //Base variables

    public InputActionReference toggleReference = null;

    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed;
    public float ammoCost;

    //Shotgun
    public bool isShotgun;
    public float spread;
    public int bulletsFired;
    //private List<Quaternion> bullets;


    //AutoFire
    //public bool isAuto;
    public float fireRate;

    //Audio
    public AudioSource gunshot;
    public AudioClip clip;
    public float volume = 0.5f;

    // Effects
    public ParticleSystem muzzleFlash;

    private Coroutine _current;

    public bool isGrabbed;

    void Start()
    {
        // Once weapon is grabbed, waits for trigger to be pressed
     //    XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
      //   grabbable.activated.AddListener(FireBullet);
        // Muzzle flash is enabled on start
        muzzleFlash.Stop();
        isGrabbed = false;
    }

    void Update()
    {
        float triggerValue = toggleReference.action.ReadValue<float>();
        if(triggerValue < 0.5 && isGrabbed == true)
        {
            Debug.Log("Stop fire");
            StopFire();
        }
        if(triggerValue > 0.5 && isGrabbed == true)
        {
            Debug.Log("Start fire");
            BeginFire();
        }    
    }

    void Grabed(SelectEnterEventArgs arg)
    {
        //BeginFire();
        isGrabbed = true;
    }
    void notGrabbed(SelectExitEventArgs arg)
    {
        //BeginFire();
        isGrabbed = false;
    }
    // void StopFireBullet(DeactivateEventArgs arg)
    // {
    //     StopFire();
    //  }
    void BeginFire()
    {
        if (_current != null)
        {
            StopCoroutine(_current);
        }
        _current = StartCoroutine(Shoot());
    }
    void StopFire()
    {
        if (_current != null)
        {
            StopCoroutine(_current);
        }

    }
    private IEnumerator Shoot()
    {
        while(true)
        {
            // Makes an object of the audio source to destory to prevent it spawning infinite times
            AudioSource newAS = Instantiate(gunshot);
            newAS.PlayOneShot(clip, volume);
            Destroy(newAS.gameObject, 3);

            // Muzzle Flash
            muzzleFlash.Play();

            if (!isShotgun)
            {
                Debug.Log("Normal shoot");
                GameObject spawnedBullet = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
                spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
                Destroy(spawnedBullet, 5);
            }
            else
            {
                Debug.Log("Shot gun shoot");
                for (int i = 0; i < bulletsFired; i++)
                {
                    //Random spread
                    float x = UnityEngine.Random.Range(-spread, spread);
                    float y = UnityEngine.Random.Range(-spread, spread);

                    Vector3 direction = spawnPoint.transform.position + new Vector3(x,y,0);

                    // Bullet Spawning
                    GameObject spawnedBullet = Instantiate(bullet, direction, spawnPoint.transform.rotation);
                    spawnedBullet.GetComponent<Rigidbody>().velocity = direction * fireSpeed;
                    Destroy(spawnedBullet, 5);
                }
            }
            yield return new WaitForSeconds(1f / fireRate);
        }

    }
}