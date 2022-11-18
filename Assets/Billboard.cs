using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Billboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI WaveNum;
    public testSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<testSpawner>();
    }
    void Update()
    {
        if (spawner == null)
        {
            spawner = FindObjectOfType<testSpawner>();
        }
        gameObject.GetComponent<PhotonView>().RPC("LinkWave", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void LinkWave()
    {
        WaveNum.text = spawner.waveNumber.ToString();
    }
}
