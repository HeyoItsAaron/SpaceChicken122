using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Billboard : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI WaveNum;
    public GameplayManager game;

    void Start()
    {
        game = FindObjectOfType<GameplayManager>();
    }
    void Update()
    {
        if (game == null)
        {
            game = FindObjectOfType<GameplayManager>();
        }
        gameObject.GetComponent<PhotonView>().RPC("LinkWave", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void LinkWave()
    {
        WaveNum.text = game.waveNumber.ToString();
    }
}
