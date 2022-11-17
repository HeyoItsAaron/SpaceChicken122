using UnityEngine;
using System.Collections.Generic;
using VRShooterKit.DamageSystem;

namespace VRShooterKit.WeaponSystem 
{
    public class StabCollider : MonoBehaviour
    {
        [SerializeField] private Rigidbody thisRB = null;
        [SerializeField] private VR_Grabbable thisGrabbable = null;
        [SerializeField] private FastCollisionListener collisionListener = null;
        [SerializeField] private Transform stabRayPoint = null;
        [SerializeField] private float minVelocity = 2.0f;
        [SerializeField] private float breakForce = 100.0f;
        [SerializeField] private float breakTorque = 100.0f;
        [SerializeField] private float breakDistance = 0.5f;
        [SerializeField] private float stabDamage = 50.0f;
        [SerializeField] private bool dropOnStab = false;
        
        private FixedJoint joint = null;
        private bool isStab = false;
        private const float grabStabAngle = 30.0f;
        private const float unGrabStabAngle = 65.0f;
        private DamageInfo damageInfo = null;

        private void Awake()
        {
            thisGrabbable.OnGrabStateChange.AddListener(OnGrabStateChange);
            BuildDamageInfo();
        }

        private void BuildDamageInfo()
        {
            damageInfo = new DamageInfo();
            damageInfo.canDismember = false;
            damageInfo.damageType = DamageType.Physical;
            damageInfo.dmg = stabDamage;
            damageInfo.forceMode = ForceMode.Force;
            damageInfo.hitDir = Vector3.zero;           
        }

        private void OnGrabStateChange(GrabState state)
        {
            if (state == GrabState.Grab || state == GrabState.Flying)
            {
                if (isStab)
                {
                    isStab = false;
                    DestroyStabJoint();
                }
            }
        }

        private void Update()
        {
            //check for stab collisions
            if (CanCheckForStabCollision())
            {
                //the fast collision listener give us the colliders his collide this frame
                List<Collider> colliderList = collisionListener.CheckForCollisionsThisFrame();
                CheckForStabCollision(colliderList);
            }
            //if we are in a stab we can try to break using our hand
            else if (CanBreakUsingHand())
            {
                CheckIfHandCanBreakJoint();
            }

        }

        private void CheckIfHandCanBreakJoint()
        {
            float d = CalculateDistanceToVisualHandPosition(); 
            if (d > breakDistance)
            {
                RemoveStab();
            }
        }

        private bool CanCheckForStabCollision()
        {
            return !isStab && thisGrabbable.CurrentGrabState != GrabState.Flying && CalculateVelocity() > minVelocity;
        }

        private bool CanBreakUsingHand()
        {
            return thisGrabbable.GrabController != null;
        }

        private float CalculateDistanceToVisualHandPosition()
        {
            return Vector3.Distance(thisGrabbable.GrabController.transform.position, thisGrabbable.GrabController.OriginalParent.position);
        }

        private void RemoveStab()
        {
            DestroyStabJoint();
            isStab = false;
            thisGrabbable.GrabController.UseRotationOffset = true;
            thisGrabbable.GrabController.SetPositionControlMode(MotionControlMode.Engine);
            VR_Controller controlller = thisGrabbable.GrabController;
            controlller.ForceGrab(thisGrabbable);
        }

        private void DestroyStabJoint()
        {
            if (joint == null)
            {
                Destroy(joint);
            }
        }


        private float CalculateVelocity()
        {
            if (thisGrabbable.CurrentGrabState == GrabState.Grab && thisGrabbable.GrabController != null)
            {
                return thisGrabbable.GrabController.Velocity.magnitude;
            }

            return thisRB.velocity.magnitude;
        }

        private void CheckForStabCollision(List<Collider> colliderList)
        {
            for (int n = 0; n < colliderList.Count; n++)
            {
                Collider col = colliderList[n];
                Rigidbody rb = col.GetComponent<Rigidbody>();

                if (StabIntentIsValid(col) && rb != null)
                {
                    TryToDoStabDamage(col);
                    StickToRigidBody(rb);
                    break;
                }
            }
        }

        private void TryToDoStabDamage(Collider col)
        {
            Damageable[] damageable = col.GetComponents<Damageable>();

            if (damageable != null)
            {
                for (int n = 0; n < damageable.Length; n++)
                {
                    damageable[n].DoDamage(damageInfo);
                }
            }           

           
        }

        private bool StabIntentIsValid(Collider col)
        {
            int layer = 1 << col.gameObject.layer;
            RaycastHit hitInfo;
            if (Physics.Raycast(stabRayPoint.position, stabRayPoint.forward, out hitInfo, float.MaxValue, layer))
            {
                Vector3 forward = stabRayPoint.forward;
                Vector3 velocity = Vector3.zero;
                float minAngle = 0.0f;

                if (thisGrabbable.CurrentGrabState == GrabState.Grab)
                {
                    velocity = thisGrabbable.GrabController.Velocity.normalized;
                    minAngle = grabStabAngle;
                }
                else if (thisGrabbable.CurrentGrabState == GrabState.UnGrab)
                {
                    velocity = thisRB.velocity.normalized;
                    minAngle = unGrabStabAngle;
                }
                else
                {
                    return false;
                }

                float angle = Vector3.Angle(forward, velocity);
                

                return angle < minAngle;
            }

            return false;
        }

        public void StickToRigidBody(Rigidbody rb)
        {
            isStab = true;
            if (thisGrabbable.CurrentGrabState == GrabState.Grab && !dropOnStab)
            {
                thisGrabbable.transform.SetParent(null);
                Destroy(thisGrabbable.GrabController.GrabPoint.GetComponent<Joint>());
                thisGrabbable.GrabController.UseRotationOffset = false;
                thisGrabbable.GrabController.SetPositionControlMode(MotionControlMode.Free);
                thisGrabbable.GrabController.transform.SetParent(thisGrabbable.transform);
            }
            else if (thisGrabbable.CurrentGrabState == GrabState.Grab && dropOnStab)
            {
                thisGrabbable.ForceDrop();
            }
            
            joint = GetComponent<FixedJoint>();

            if (joint == null)
                joint = gameObject.AddComponent<FixedJoint>();

            joint.connectedBody = rb;
            joint.enableCollision = false;
            joint.breakForce = breakForce;
            joint.breakTorque = breakTorque;
                        
                
        }


    }
}
