using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using static Unity.VisualScripting.Member;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{
    // Base variables
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed;
    public float energyCost;
    public int maxBulletsPerMag;
    private int bulletCounter;
    public int reloadTime;
    public int bulletsFiredPerShot;
    private bool canShoot;

    // Shotgun
    public bool isShotgun;
    public float spread;

    // AutoFire
    //public bool isAuto;
    public float fireRate;

    // Audio
    public AudioSource gunshot;
    public AudioClip clipAudio;
    public float volume = 0.5f;

    // Effects
    public ParticleSystem muzzleFlash;

    // Cooroutine for shoot function
    private Coroutine _current;

   // Player stats
    public PlayerStats player;

    void Start()
    {
        //Once weapon is grabbed, waits for it to be grabbed and the trigger to be activated/deactivated
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.selectExited.AddListener(notGrabbed);
        grabbable.activated.AddListener(BeginFire);
        grabbable.deactivated.AddListener(StopFire);
        // Muzzle flash is enabled on start so we disable it
        muzzleFlash.Stop();
        canShoot = true;

        //Player stats object
        player = GameObject.FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerStats>();
        }
    }
    [PunRPC]
    void PlayGunshot()
    {
        Debug.Log("Play Gunshot");
        gunshot.clip = clipAudio;
        gunshot.volume = volume;
        gunshot.spatialBlend = 1;
        gunshot.minDistance = 25;
        gunshot.maxDistance = 100;
        gunshot.Play();
    }
    void notGrabbed(BaseInteractionEventArgs arg)
    {
        StopAllCoroutines();
        //isGrabbed = false;
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
    IEnumerator DespawnBullet(GameObject aSpawnedBullet)
    {
        yield return new WaitForSeconds(3);
        PhotonNetwork.Destroy(aSpawnedBullet);
    }
    IEnumerator Reload()
    {
        bulletCounter = 0;
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }
    [PunRPC]
    public IEnumerator Shoot()
    {
        if(bulletCounter >= maxBulletsPerMag)
        {
            StartCoroutine("Reload");
        }
        else
        {
            while(true)
            {
                // Can afford
                if(player.currEnergy < energyCost)
                {
                    break;
                }
                // Needs to reload
                if (bulletCounter >= maxBulletsPerMag)
                {
                    StartCoroutine("Reload");
                    break;
                }
                if(canShoot == false)
                {
                    break;
                }
                bulletCounter++;
                Debug.Log("" + bulletCounter);
                //player loses energy on fire

                player.currEnergy -= energyCost;
                // Makes an object of the audio source to destory to prevent it spawning infinite times
                //AudioSource newAS = Instantiate(gunshot);
                //newAS.PlayOneShot(clip, volume);
                //Destroy(newAS.gameObject, 2.5f);
                photonView.RPC("PlayGunshot", RpcTarget.AllBuffered);

                // Muzzle Flash
                muzzleFlash.Play();

                for (int i = 0; i < bulletsFiredPerShot; i++)
                {
                    //Random spread
                    float x = UnityEngine.Random.Range(-spread, spread);
                    float y = UnityEngine.Random.Range(-spread, spread);

                    Vector3 direction = spawnPoint.transform.position + new Vector3(x, y, 0);

                    // Bullet Spawning
                    GameObject spawnedBullet = PhotonNetwork.Instantiate(bullet.name, direction, spawnPoint.transform.rotation);
                    Debug.Log("" + bullet.name);
                    spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.transform.forward * fireSpeed;
                    StartCoroutine(DespawnBullet(spawnedBullet));
                }
                yield return new WaitForSeconds(1f / fireRate);
            }
        }
    }
}