using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StopOnCollision : MonoBehaviour
{
    Rigidbody rigidBody;
    public GameObject hammerPrefab;
    GameObject hammer;
    Vector3 targetLocation;
    Quaternion targetRotation;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.isKinematic = true;
        targetLocation = gameObject.transform.position;
        targetRotation = gameObject.transform.rotation;

        hammer = PhotonNetwork.Instantiate("Network Hammer", targetLocation, targetRotation);
        Destroy(gameObject);
    }
}
