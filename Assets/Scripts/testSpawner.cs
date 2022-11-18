using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class testSpawner : MonoBehaviour
{
    public GameObject[] spawners;
    public string[] Enemies;
    public string enemy3;
    public int waveNumber = 0;
    public int enemyAmount = 0;
    int randomEnemy;
    int spawnerId;

    // Start is called before the first frame update
    void Start()
    {
        spawners = new GameObject[5];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }

        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAmount == 0)
        {
            NextWave();
        }
    }

    public GameObject SpawnAt(int enemyPos, Vector3 pos, Quaternion rot)
    {
        GameObject chicken = PhotonNetwork.Instantiate(Enemies[enemyPos], pos, rot);
        return chicken;
    }

    public void NextWave()
    {
        waveNumber++;
        enemyAmount = waveNumber * 2;
        randomEnemy = Random.Range(0, Enemies.Length);
        spawnerId = Random.Range(0, spawners.Length);

        for (int i = 0; i < enemyAmount; i++)
        {
           SpawnAt(randomEnemy, spawners[spawnerId].transform.position, spawners[spawnerId].transform.rotation);
        }
    }

}
