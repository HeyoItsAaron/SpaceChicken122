// Aaron Williams
// 10/12/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.Rendering;

public class NetworkPlayerStats : MonoBehaviour
{
    // Variables
    //public string 

    public float currHealth;
    public float maxHealth;
    public bool isDead;

    public float currentPowerUpDuration;

    public float currEnergy; //this is "ammmo"
    public float maxEnergy;

    public double currCurrency;
    public double maxCurrency;

    // Methods

    // Start is called before the first frame update
    void OnAwake()
    {
        currHealth = 100;

    }

    
}
