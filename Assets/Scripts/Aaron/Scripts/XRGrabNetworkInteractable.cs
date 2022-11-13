// Aaron Williams
// 10/26/2022
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractable : XRGrabInteractable
{
    // Variables

    private PhotonView photonView;

    // Methods

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    //Allows the Grabbing of things over the network in Photon PUN
    protected override void OnSelectEntering(SelectEnterEventArgs Args) //or OnSelectEntered idk
    {
        photonView.RequestOwnership();
        base.OnSelectEntering(Args);
        // ownership determines who can grab what in Photon PUN
        // requesting ownership on an item that is tagged "Takeover"
        // in their Photon Views allows that player to grab the object freely
    }
}
