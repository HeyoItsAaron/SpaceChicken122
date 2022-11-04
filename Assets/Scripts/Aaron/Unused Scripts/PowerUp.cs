using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    public PowerUpEffect powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        powerUpEffect.Apply(other.gameObject);
    }
}
