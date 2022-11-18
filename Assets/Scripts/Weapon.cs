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
        //Once weapon is grabbed, waits for it to be grabbed and the trigger to be activated/deactivated
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.selectEntered.AddListener(Grabbed);
        grabbable.selectExited.AddListener(notGrabbed);
        grabbable.activated.AddListener(BeginFire);
        grabbable.deactivated.AddListener(StopFire);
        // Muzzle flash is enabled on start
        muzzleFlash.Stop();
        isGrabbed = false;
    }

    void Update()
    {
        
    }

    void Grabbed(BaseInteractionEventArgs arg)
    {
        isGrabbed = true;
    }
    void notGrabbed(BaseInteractionEventArgs arg)
    {
        StopAllCoroutines();
        isGrabbed = false;
    }
    void BeginFire(BaseInteractionEventArgs arg)
    {
        if (_current != null)
        {
            StopCoroutine(_current);
        }
        _current = StartCoroutine(Shoot());
    }
    void StopFire(BaseInteractionEventArgs arg)
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
                GameObject spawnedBullet = Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
                spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
                Destroy(spawnedBullet, 5);
            }
            else
            {
                for (int i = 0; i < bulletsFired; i++)
                {
                    //Random spread
                    float x = UnityEngine.Random.Range(-spread, spread);
                    float y = UnityEngine.Random.Range(-spread, spread);

                    Vector3 direction = spawnPoint.transform.position + new Vector3(x,y,0);

                    // Bullet Spawning
                    GameObject spawnedBullet = Instantiate(bullet, direction, spawnPoint.transform.rotation);
                    spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.transform.forward * fireSpeed;
                    Destroy(spawnedBullet, 5);
                }
            }
            yield return new WaitForSeconds(1f / fireRate);
        }

    }
}