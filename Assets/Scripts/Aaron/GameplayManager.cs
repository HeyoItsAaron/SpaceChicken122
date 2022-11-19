using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // variables
    //public float sharedCurrency;
    public int waveNumber;
    public int enemyAmount = 1;
    public NetworkSpawner spawner;

    Vector3 spawnpoint = new Vector3(76, 6, 90);


    public float spawnRate = 1.0f;
    public float timesBetweenWaves = 5.0f;

    [SerializeField] private bool _isWaveActive;
    [SerializeField] private bool _stopSpawning;

    // methods
    void Start()
    {
        waveNumber = 1;
        //spawner = GameObject.FindObjectOfType<NetworkSpawner>();
        StartCoroutine(WaveSpawner());
    }
    void Update()
    {
        //if(spawner == null)
        //{
        //    spawner = GameObject.FindObjectOfType<NetworkSpawner>();
        //    StartCoroutine(WaveSpawner());
        //}
    }
    IEnumerator WaveSpawner()
    {
        Debug.Log("COROUTINE STARTED");
        while(_isWaveActive == true && _stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9.3f, -9.3f), 7f, 0f);
            int randomenemy = Random.Range(0, 5);
            _isWaveActive = false;

            for (int i = 0; i<enemyAmount; i++)
            {
                PhotonNetwork.InstantiateRoomObject("NormalChicken", spawnpoint, Quaternion.Euler(0,0,0));
                yield return new WaitForSeconds(spawnRate);
            }

            enemyAmount += 1;
            waveNumber += 1;
            _isWaveActive = true;
            yield return new WaitForSeconds(timesBetweenWaves);
        }
    }
}
