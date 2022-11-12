using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHeightController : MonoBehaviour
{
    //reset position varaibles v1
    //[SerializeField] Transform resetTransform;
    //[SerializeField] Player player;
    //[SerializeField] Camera playerHead;

    //Scale variables v2
    [SerializeField] float defaultHeight;
    [SerializeField] Camera aCamera;


    void onEnable()
    {
        Resize();
    }


    public void Resize()
    {
        float headHeight = aCamera.transform.localPosition.y;
        float scale = defaultHeight / headHeight;
        transform.localScale = Vector3.one * scale;
    }


    //method v1
    /*public void ResetPosition()
    {
        var rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;
        player.transform.Rotate(0, rotationAngleY, 0);

        var distanceDiff = resetTransform.position - playerHead.transform.position;

        player.transform.position += distanceDiff;
    }*/
}
