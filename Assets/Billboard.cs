using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

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
        gameObject.GetComponent<PhotonView>().RPC("LinkWave", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void LinkWave()
    {
        WaveNum.text = spawner.waveNumber.ToString();
    }
}
