using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnCollision : MonoBehaviour
{
    Rigidbody rigidBody;
    public Hammer hammerPrefab;
    Hammer hammer;
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

        hammer = Instantiate(hammerPrefab, targetLocation, targetRotation);
        Destroy(gameObject);
    }
}
