using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class VRMap
{

    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    public Transform root;
    [Range(0,1)]
    public float turnSmoothness = 1;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    private Vector3 headBodyOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (!root)
            root = transform;

        headBodyOffset = root.position - head.rigTarget.position;
    }

    // Update is called once per frame
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
        // Put your update code here
    //}
    //void FixedUpdate()
    ///{
        root.position = head.rigTarget.position + headBodyOffset;
        root.forward = Vector3.Lerp(root.forward,
        Vector3.ProjectOnPlane(head.vrTarget.forward,Vector3.up).normalized, turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
