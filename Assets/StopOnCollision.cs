using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnCollision : MonoBehaviour
{
    Rigidbody rigidbody;
    public Hammer hammerPrefab;
    Hammer hammer;
    Vector3 targetLocation;
    Quaternion targetRotation;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        rigidbody.isKinematic = true;
        targetLocation = gameObject.transform.position;
        targetRotation = gameObject.transform.rotation;

        hammer = Instantiate(hammerPrefab, targetLocation, targetRotation);
        Destroy(gameObject);
    }
}
