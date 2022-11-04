using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currHealth;
    public int maxHealth;
    public bool isDead = false;

    public float currentPowerUpDuration = 0;

    public int currentAmmoCount;

    Rigidbody rb;

    PowerUp powerUp;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }
    public void CheckHealth()
    {
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("egg"))
        {
            TakeDamage(10);
        }
        if (other.gameObject.CompareTag("PowerUp"))
        {
            powerUp = other.gameObject.GetComponent<PowerUp>();
            powerUp.ApplyPowerUp();
        }
        if (other.gameObject.CompareTag("Hammer"))
        {
            ItsHammerTime();
        }
    }
    public void ItsHammerTime()
    {
        Hammer hammer = GameObject.Find("Hammer").GetComponent<Hammer>();
        currentPowerUpDuration -= currentPowerUpDuration * Time.deltaTime;

        if(currentPowerUpDuration == 0)
        {
            Destroy(hammer);
            //hammer.ByeByeHammer();
        }
    }
}
