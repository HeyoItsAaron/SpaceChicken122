using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Billboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WaveNum;
    public NetworkSpawner spawner;
    //currencyCount.text = "$" + player.currCurrency.ToString("N2");
    // Start is called before the first frame update
    void Start()
    {
        
        spawner = FindObjectOfType<NetworkSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        WaveNum.text = spawner.waveNumber.ToString();
    }

    public void LinkWave()
    {

    }
}
