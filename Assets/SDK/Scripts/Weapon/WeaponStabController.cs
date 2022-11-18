using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRShooterKit.WeaponSystem
{

    public class WeaponStabController : MonoBehaviour
    {

        [SerializeField] private Rigidbody thisRB = null;
        [SerializeField] private VR_Grabbable thisGrabbable = null;
        [SerializeField] private FastCollisionListener collisionListener = null;
        [SerializeField] private float requireVelocity = 2.0f;

        private FixedJoint joint = null;
        private bool isStab = false;
        private float breakDistance = 0.5f;
        private float currentBreakVelocity = 0.0f;
        private VR_Controller currentGrabController = null;

        private const float VELOCITY_TO_BREAK_FORCE = 10.0f;
        private const float VELOCITY_TO_BREAK_DISTANCE = 0.05f;
        private const float VELOCITY_TO_BREAK_VELOCITY = 0.65f;
        private const float MASS_SCALE = 10.0f;

        private void Update()
        {
            if (CanCheckForStabCollisions())
            {
                CheckForStabCollision();
            }
            
            else if(currentGrabController != null)
            {
                               
                float d = Vector3.Distance(currentGrabController.transform.position, currentGrabController.OriginalParent.position);
                if (d > 0.15f && currentGrabController.Velocity.magnitude > currentBreakVelocity)
                {
                    BreakStab();
                }
                
                
            }

        }

        private void CheckForStabCollision()
        {
            List<Collider> colliderList = collisionListener.CheckForCollisionsThisFrame();

            for (int n = 0; n < colliderList.Count; n++)
            {
                Rigidbody rb = colliderList[n].attachedRigidbody;

                if (rb != null)
                {
                    StickToRigidBody(rb);
                    break;
                }
            }
        }

        public void StickToRigidBody(Rigidbody rb)
        {
            isStab = true;
            currentGrabController = thisGrabbable.GrabController;

            if (thisGrabbable.CurrentGrabState == GrabState.Grab)
            {
                thisGrabbable.transform.SetParent(null);
                Destroy(currentGrabController.GetComponent<Joint>());                
                currentGrabController.SetPositionControlMode(MotionControlMode.Free);
                currentGrabController.transform.SetParent(thisGrabbable.transform);
            }

            joint = GetComponent<FixedJoint>();

            if (joint == null)
                joint = gameObject.AddComponent<FixedJoint>();

            float breakForce = currentGrabController.Velocity.magnitude * VELOCITY_TO_BREAK_FORCE;

            joint.connectedBody = rb;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;
            joint.massScale = MASS_SCALE;

            currentBreakVelocity = currentGrabController.Velocity.magnitude * VELOCITY_TO_BREAK_VELOCITY;
        }


        private bool CanCheckForStabCollisions()
        {
            return !isStab && thisGrabbable.CurrentGrabState == GrabState.Grab;
        }

        private bool CanBreakJoint()
        {
            float d = Vector3.Distance(currentGrabController.transform.position, currentGrabController.OriginalParent.position);
            return d > breakDistance;
        }
        
        private void BreakStab()
        {
            
            if (!isStab)
            {
                return;
            }
           
            //currentGrabController.UseRotationOffset = true;
            currentGrabController.SetPositionControlMode(MotionControlMode.Engine);
            currentGrabController.ForceGrab(thisGrabbable);

            isStab = false;
        }

        

        

    }
}



