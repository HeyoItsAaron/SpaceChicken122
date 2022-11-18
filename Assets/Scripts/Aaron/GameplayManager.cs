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

    // methods
    void Start()
    {
        waveNumber = 0;
        spawner = GameObject.FindObjectOfType<testSpawner>();
    }
    void Update()
    {
        if (enemyAmount == 0)
        {
            spawner.NextWave();
        }
    }

    public void StartWave()
    {
        //spawner.SpawnEnemy(this many);
    }
}
