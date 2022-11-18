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
  

    // Start is called before the first frame update
    private void Start()
    {
        spawners = new GameObject[5];
        //ui = GameObject.FindObjectOfType<WristUI>();

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemy();
        }
        if (enemiesKilled >= enemyAmount)
        {
            NextWave();
        }
    }

    // spawn method
    private void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, Enemies.Length);
        int spawnerId = Random.Range(0, spawners.Length);
        if(waveNumber % 5 != 0)
        {
            PhotonNetwork.Instantiate(Enemies[randomEnemy], spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
        }
        if(waveNumber % 5 ==0)
        {
            PhotonNetwork.Instantiate(Enemies[randomEnemy], spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
            spawnerId = Random.Range(0, spawners.Length);
            PhotonNetwork.Instantiate(enemy3, spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
        }
    }

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
