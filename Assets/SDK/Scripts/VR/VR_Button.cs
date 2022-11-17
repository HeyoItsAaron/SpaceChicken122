using UnityEngine;
using UnityEngine.Events;
using VRShooterKit.DamageSystem;
using System.Collections.Generic;

namespace VRShooterKit
{
    public enum ButtonState
    {
        Pressing,
        UnPressing,
        Hold,
        Ready
    }

    public class VR_Button : Damageable
    {
        [SerializeField] private List<Collider> ignoreCoilliderList = null;
        [SerializeField] private Transform obstacleCastPoint = null;
        [SerializeField] private float obstacleCastRadius = 0.25f;
        [SerializeField] private float pressThreshold = 0.2f;
        [SerializeField] private float pressTime      = 0.0f;
        [SerializeField] private Vector3 pressingDir = Vector3.zero;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private UnityEvent onClick = null;
        

        private Vector3     unpressPosition   = Vector3.zero;
        private Vector3     pressPosition     = Vector3.zero;
        private ButtonState currentState      = ButtonState.Ready;
        private float       pressStartTime    = 0.0f;
        private float       unpressStartTime  = 0.0f;
        private bool        release           = false;
        private Collider triggerCollider = null;
        private Collider thisCollider = null;
        

        private void Awake()
        {
            //get button collider and set it as trigger
            thisCollider = GetComponent<Collider>();
            thisCollider.isTrigger = true;

            //we should ignore collision with things around the button like walls, button base ect
            //or will always be as pressed
            for (int n = 0; n < ignoreCoilliderList.Count; n++)
            {
                if(ignoreCoilliderList[n] != null)
                    Physics.IgnoreCollision(thisCollider , ignoreCoilliderList[n]);
            }
            
            //calculate positions
            unpressPosition = transform.position;
            pressPosition = transform.position + pressingDir * pressThreshold;            
        }

        private void Update()
        {
            if(currentState == ButtonState.Pressing)
                currentState = PressingUpdate();
            else if(currentState == ButtonState.UnPressing)
                currentState = UnPressingUpdate();
            else if(currentState == ButtonState.Hold)
                currentState = HoldUpdate();
        }

        //in this state the button go to his pressing position
        private ButtonState PressingUpdate()
        {
            float percComplete = (Time.time - pressStartTime) / pressTime;
            if (percComplete < 1)
            {
                transform.position = Vector3.Lerp(unpressPosition, pressPosition, percComplete);
                return ButtonState.Pressing;
            }            

            if(onClick != null)
                onClick.Invoke();

            return ButtonState.Hold;;
        }

        //in this state the button go to his unpressing position
        private ButtonState UnPressingUpdate()
        {
            float percComplete = (Time.time - unpressStartTime) / pressTime;
            if (percComplete < 1)
            {
                transform.position = Vector3.Lerp(pressPosition , unpressPosition , percComplete);
                return ButtonState.UnPressing;
            }

            return ButtonState.Ready;
        }

        //in this state we check if the button is being pressed
        private ButtonState HoldUpdate()
        {
            //we release the button or the collider trigger was disable?
            if ((release || (triggerCollider == null || !triggerCollider.enabled))  && !HaveObstacle())
            {
                release           = false;
                unpressStartTime  = Time.time;
                return ButtonState.UnPressing;
            }


            return ButtonState.Hold;
        }

        public void Click(Collider c)
        {
            if(currentState != ButtonState.Ready)
                return;

            triggerCollider   = c;
            release           = false;
            currentState      = ButtonState.Pressing;
            pressStartTime    = Time.time;
        }

        public void Release()
        {
            release = true;           
        }

        //something is pressing this button
        private void OnTriggerEnter(Collider other)
        {           
            Click(other);
        }

        //this button have a obstacle like the hand presisng it?
        private bool HaveObstacle()
        {           
            Collider[] colliderArray = Physics.OverlapSphere(obstacleCastPoint.position , obstacleCastRadius , obstacleLayerMask.value , QueryTriggerInteraction.Ignore);
            

            for (int n = 0 ; n < colliderArray.Length ; n++)
            {
                if (!ignoreCoilliderList.Contains( colliderArray[n]) && colliderArray[n] != thisCollider)
                {                   
                    return true;
                }
            }

            return false;
        }

        private void OnTriggerStay(Collider other)
        {
            if(currentState == ButtonState.Ready)
                Click(other);
        }


        private void OnTriggerExit(Collider other)
        {           
            Release();
        }

        public override void DoDamage(DamageInfo info)
        {
            Click(null);
        }

        private void OnDrawGizmosSelected()
        {
            if(obstacleCastPoint != null)
                Gizmos.DrawWireSphere( obstacleCastPoint.position , obstacleCastRadius );
        }
       
    }

}

