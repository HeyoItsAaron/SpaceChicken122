using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using UnityEngine;
using System.Collections;

public class Bow : XRGrabNetworkInteractable
{
    public PlayerStats player;

    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.selectEntered.AddListener(Grabbed);
        grabbable.selectExited.AddListener(NotGrabbed);
        player = GameObject.FindObjectOfType<PlayerStats>();
    }
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerStats>();
        }
    }
    void Grabbed(BaseInteractionEventArgs arg)
    {
        player.hasItemInHand = true;
        StartCoroutine("DisableGrabRayDelay");
    }
    void NotGrabbed(BaseInteractionEventArgs arg)
    {
        player.hasItemInHand = false;
        StopAllCoroutines();
    }
    IEnumerator DisableGrabRayDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Grab Ray").SetActive(false);
    }
}
