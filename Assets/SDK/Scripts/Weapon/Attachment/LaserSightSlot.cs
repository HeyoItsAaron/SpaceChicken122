using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRShooterKit
{
    public class LaserSightSlot : MonoBehaviour
    {
        [SerializeField] private VR_DropZone dropSlot = null;
                
        private void Awake()
        {
            dropSlot.OnDrop.AddListener( OnDrop );
            dropSlot.OnUnDrop.AddListener( OnUnDrop );
        }

        private void OnDrop(VR_Grabbable grabbable)
        {
            LaserSight laserSight = grabbable.GetComponentInChildren<LaserSight>();

            if (laserSight != null)
            {
                laserSight.OnEnableLaserSight();
                laserSight.transform.root.parent = dropSlot.DropPoint;
            }
                

            
        }

        private void OnUnDrop(VR_Grabbable grabbable)
        {
            LaserSight laserSight = grabbable.GetComponentInChildren<LaserSight>();

            if (laserSight != null)
            {
                laserSight.OnDisableLaserSight();
                laserSight.transform.root.parent = null;
            }
                
        }

    }

}
