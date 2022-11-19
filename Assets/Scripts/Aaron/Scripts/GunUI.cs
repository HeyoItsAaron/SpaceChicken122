using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class GunUI : MonoBehaviour
{

    public Image ammoCountBar;
    public TextMeshProUGUI ammoCountText;

    [Range(0, 100)]
    public float ammoCountFill;
    public float ammoCountMax = 100.0f;

    public PlayerStats player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerStats>();
        }
        LinkAmmoCount();
    }

    public void LinkAmmoCount()
    {
        ammoCountFill = player.currEnergy;
        ammoCountBar.fillAmount = (ammoCountFill / 100.0f);
        ammoCountText.text = player.currEnergy.ToString();
    }
}
