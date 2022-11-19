// Written by Aidan Urbina
// Aaron Williams converted to Network Script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkSpawner : MonoBehaviour
{
    // variables
    public GameObject[] spawners;
    public string[] Enemies;
    public string enemy3;
    public int waveNumber = 0;
    public int enemyAmount = 0;
    public int enemiesKilled = 0;
    //public WristUI ui;
    // hi
  

    // Start is called before the first frame update
    void Start()
    {
        // initialized array with set number of spawners
        spawners = new GameObject[5];
        //ui = GameObject.FindObjectOfType<WristUI>();

        // fills array with children spawners
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        //StartWave();
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        // if T is pressed spawn chickens
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemy();
        }
        // if enemys killed greater than enemies spawned move to next wave
        if (enemiesKilled >= enemyAmount)
        {
            NextWave();
        }
    }

    // spawn method
    private void SpawnEnemy()
    {
        // set random enemy and random spawn position
        int randomEnemy = Random.Range(0, Enemies.Length);
        int spawnerId = Random.Range(0, spawners.Length);
        // sees if round is divisible by 5 if so spawn special enemy
        if(waveNumber % 5 != 0)
        {
            PhotonNetwork.InstantiateRoomObject(Enemies[randomEnemy], spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
        }
        if(waveNumber % 5 ==0)
        {
            PhotonNetwork.InstantiateRoomObject(Enemies[randomEnemy], spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
            spawnerId = Random.Range(0, spawners.Length);
            PhotonNetwork.InstantiateRoomObject(enemy3, spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
        }
    }

    /*
    private void StartWave()
    {
        waveNumber = 1;
        enemyAmount = 2;
        enemiesKilled = 0;

        for(int i = 0; i < enemyAmount; i++)
        {
            SpawnEnemy();
        }
    }
    */

    public void NextWave()
    {
        waveNumber++;
        enemyAmount += 2;
        enemiesKilled = 0;
        //ui.LinkWaveUI();

        for (int i = 0; i < enemyAmount; i++)
        {
            SpawnEnemy();
        }
    }
}
