// Ali Rashid
// 10/27/22

using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    public enum HandModelType { Left, Right };

    public HandModelType handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}
