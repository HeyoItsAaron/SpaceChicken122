using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrabRay : MonoBehaviour
{
    public void ToggleVisibilityOverNet()
    {
        gameObject.GetComponent<PhotonView>().RPC("ToggleVisibility", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
