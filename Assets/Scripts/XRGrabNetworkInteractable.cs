using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractable : XRGrabInteractable
{
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor) //in video was just called "OnSelectEnter"
    {
            // this will change ownership of object
            // by requesting ownership from current local player
            // request doesn't need approval as long as Ownership Transfer
            // in PhotonView is set to Takeoever
        photonView.RequestOwnership(); 
        base.OnSelectEntered(interactor);
    }
}
