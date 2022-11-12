using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float currHealth;
    public float maxHealth;
    public bool isDead;

    public float currentPowerUpDuration;

    public float currEnergy; //this is "ammmo"
    public double currCurrency;

    //public enum currentPowerUp { None, Hammer, Health };

    Rigidbody rb;
    WristUI ui;

    PowerUp powerUp;

    // Start is called before the first frame update
    void Start()
    {
        loadSpawnStats();
        rb = GetComponent<Rigidbody>();
        ui = GameObject.FindObjectOfType<WristUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    //load spawn stats
    public void loadSpawnStats()
    {
        maxHealth = 100f;
        currHealth = 85;
        currEnergy = 50f;
        currCurrency = 69.00f;
        isDead = false;
        //ui.linkAllStats();
    }

    // trigger death on 0 health
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
    // + health
    public void Gainhealth(float healthAdded)
    {
        currHealth += healthAdded;
        ui.LinkHealthUI();
    }
    // - health
    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        ui.LinkHealthUI();
    }
    // Destroy on Death
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    //energy (Ammo)
    // + energy (Ammo)
    public void AddEnergy(float energyAdded)
    {
        currEnergy += energyAdded;
        ui.LinkEnergyUI();
    }
    // - energy (Ammo)
    public void UseEnergy(float energyUsed)
    {
        currEnergy -= energyUsed;
        ui.LinkEnergyUI();
    }

    // currency
    // + currency
    public void AddCurrency(float currencyAdded)
    {
        currCurrency += currencyAdded;
        ui.LinkCurrencyUI();
    }
    // - currency
    public void UseCurrency(float currencyUsed)
    {
        currCurrency -= currencyUsed;
        ui.LinkCurrencyUI();
    }

    // ON COLLISION ----> Damage + PowerUps
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
