using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HammerPowerUp : MonoBehaviour, PowerUp
{
    public GameObject pickupEffect;
    public Hammer hammerPrefab;

    public Player player;

    private Hammer hammer;
    private Rigidbody hammerRigidBody;
    private Vector3 targetLocation;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEntered worked");
            targetLocation = gameObject.transform.position;
            ApplyPowerUp();
        }
    }

    void ApplyPowerUp()
    {
        Debug.Log("Starting to apply power up worked");
        //do pRtiklez hehe
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //apply effect to player
            //call down hammer at power-up location
        player.currentPowerUpDuration = 60;
        player.ItsHammerTime();
        hammer = Instantiate(hammerPrefab, new Vector3(targetLocation.x, targetLocation.y + 100, targetLocation.x), Quaternion.Euler(new Vector3(0, 0, 180)));
        hammerRigidBody = hammer.GetComponent<Rigidbody>();
        hammerRigidBody.velocity = Vector3.down * 100;

        //remove power-up object
        Destroy(gameObject);
    }
}
