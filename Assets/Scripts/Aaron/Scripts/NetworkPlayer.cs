// Aaron Williams
// 10/12/2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.Rendering;

public class NetworkPlayer : MonoBehaviour
{
    // Variables

    public List<GameObject> avatars;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private PhotonView photonView;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;
    private GameObject spawnedAvatar;


    // Methods

    // Start is called before the first frame update
    void Start()
    {
        // Gets Photon View of object Network Player is attached to
        photonView = GetComponent<PhotonView>();

        // Gets XROrigin (VR Headset pos, main camera, and controllers pos and actions, etc)
        // of object Network Player is attached to
        XROrigin rig = FindObjectOfType<XROrigin>();

        // Gets Main Camera transform / this is the VR Headset,
        // LeftHand Controller and RightHand Controller transforms/ these are the VR Controllers,
        // Transform = Position and Rotation
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        //Retruns true if the Photon View is "owned" by the local player
        if(photonView.IsMine)
            //this sets the local avatar so everyone in the game sees it.
            photonView.RPC("LoadAvatar", RpcTarget.AllBuffered, PlayerPrefs.GetInt("AvatarID"));
    }

    //Function that is responsible to load an avatar among the avatar list
    [PunRPC]
    public void LoadAvatar(int index)
    {
        if (spawnedAvatar)
            Destroy(spawnedAvatar);

        spawnedAvatar = Instantiate(avatars[index], transform);
        AvatarInfo avatarInfo = spawnedAvatar.GetComponent<AvatarInfo>();

        avatarInfo.head.SetParent(head, false);
        avatarInfo.leftHand.SetParent(leftHand, false);
        avatarInfo.rightHand.SetParent(rightHand, false);

        leftHandAnimator = avatarInfo.leftHandAnimator;
        rightHandAnimator = avatarInfo.rightHandAnimator;
    }
    private void OnEnable()
    {
        if (RenderPipelineManager.currentPipeline != null)
        {
            RenderPipelineManager.beginCameraRendering += RenderPipelineBeforeRenderCallback;
        }
        else
        {
            Camera.onPreRender += BuiltinPipelineBeforeRenderCallback;
        }
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= RenderPipelineBeforeRenderCallback;
        Camera.onPreRender -= BuiltinPipelineBeforeRenderCallback;
    }

    private void RenderPipelineBeforeRenderCallback(ScriptableRenderContext argA, Camera argB)
      => BeforeRenderUpdate();

    private void BuiltinPipelineBeforeRenderCallback(Camera argA)
      => BeforeRenderUpdate();

    private void BeforeRenderUpdate()
    {
        // Update is called once per frame
        //void Update()
    //{

        if(photonView.IsMine)
        {          
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
      
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (!handAnimator)
            return;

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }


    void MapPosition(Transform target,Transform rigTransform)
    {    
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
