using Photon.Realtime;
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
    WristUI ui;

    PowerUp powerUp;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        currHealth = maxHealth;

        currEnergy = 50;
        currCurrency = 0;

        rb = GetComponent<Rigidbody>();
        ui = GameObject.Find("XR Origin").GetComponentInChildren<WristUI>();
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
    // + health
    public void Gainhealth(int healthAdded)
    {
        currHealth += healthAdded;
        ui.LinkHealthUI();
    }
    // - health
    public void TakeDamage(int damage)
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
    public void AddEnergy(int energyAdded)
    {
        currEnergy += energyAdded;
        ui.LinkEnergyUI();
    }
    // - energy (Ammo)
    public void UseEnergy(int energyUsed)
    {
        currEnergy -= energyUsed;
        ui.LinkEnergyUI();
    }

    // currency
    // + currency
    public void AddCurrency(int currencyAdded)
    {
        currCurrency += currencyAdded;
        ui.LinkCurrencyUI();
    }
    // - currency
    public void UseCurrency(int currencyUsed)
    {
        currCurrency -= currencyUsed;
        ui.LinkCurrencyUI();
    }

    // ON COLLISION // Damage + PowerUps
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
