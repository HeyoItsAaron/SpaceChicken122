using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currHealth;
    public int maxHealth;
    public bool isDead = false;

    public float currentPowerUpDuration = 0;

    public int currEnergy; //this is "ammmo"
    public int currCurrency;

    public enum currentPowerUp { None, Hammer, Health }

    Rigidbody rb;

    PowerUp powerUp;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currHealth = maxHealth;

        currEnergy = 50;
        currCurrency = 0;

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

    //health
    public void TakeDamage(int damage)
    {
        currHealth -= damage;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    //energy "Ammo"
    public void UseEnergy(int energyUsed)
    {
        currEnergy -= energyUsed;
    }

    //currency
    public void UseCurrency(int currencyUsed)
    {
        currCurrency -= currencyUsed;
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

    }

}
