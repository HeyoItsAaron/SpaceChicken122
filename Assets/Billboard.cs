using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Billboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WaveNum;
    public NetworkSpawner spawner;
    
    void Start()
    {
        spawner = FindObjectOfType<NetworkSpawner>();
    }
    void Update()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<NetworkSpawner>();
        }
        LinkWave();
    }

    public void LinkWave()
    {
        WaveNum.text = spawner.waveNumber.ToString();
    }
}
