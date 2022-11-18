using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // variables
    public float sharedCurrency;
    public int waveNumber;
    public int enemyAmount;
    public testSpawner spawner;
    int spawnThisMany;
    bool isSafe;

    // methods
    void OnEnable()
    {
        waveNumber = 0;
        spawner = GameObject.FindObjectOfType<testSpawner>();
    }
    void Update()
    {
        
    }
    public void StartWave()
    {
        waveNumber++;
        spawnThisMany = waveNumber * 2;
        //spawner.SpawnEnemy((waveNumber * 2));
    }
}
