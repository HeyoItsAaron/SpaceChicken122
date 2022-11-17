using UnityEngine;
using System.Collections;

namespace VRShooterKit.WeaponSystem
{
    public class Arrow : Projectile
    {
        [SerializeField] private Rigidbody thisRB = null;
        [SerializeField] private Collider thisCollider = null;
        [SerializeField] private float smoothSpeed = 0.5f;
        [SerializeField] private Transform arrowHead = null;
        [SerializeField] private float arrowDepth = 0.8f;        
        [SerializeField] private bool shouldParent = false;
              

        private float distanceTraveled = 0.0f;
        private VR_Grabbable grabbable = null;
        private bool hitSomething = false;
        private Vector3 lastArrowHeadPosition = Vector3.zero;
        private const float minLerpAngle = 9.0f;      
        private Vector3 velocity;
        private float smoothTime;
        private float size = 0.0f;
        private bool grabProcessed = false;
        private bool stickToRB = false;
        private FixedJoint joint = null;
        private Vector3 shootDir = Vector3.zero;
        private bool frameFix = false;
        private float ignoreCollisionTimer = 0.0f;

        public float Size { get { return size;  } }

        private const float MASS_SCALE = 10.0f;
        private const float IGNORE_COLLISION_TIME = 0.05f;

        protected override void Awake()
        {
            base.Awake();

            grabbable = GetComponent<VR_Grabbable>();
            grabbable.OnGrabStateChange.AddListener( OnGrabStateChange );

            //calculate arrow size
            size = Vector3.Distance( transform.position , arrowHead.position );
        }

        private void Update()
        {
            if (stickToRB && joint != null && joint.connectedBody == null)
            {
                stickToRB = false;
                Destroy(joint);
                thisRB.isKinematic = false;
                thisCollider.enabled = true;
            }
        }

        private void FixedUpdate()
        {

            if (!CanFly())
                return;

            if (ignoreCollisionTimer < IGNORE_COLLISION_TIME)
            {
                ignoreCollisionTimer += Time.deltaTime;
            }

            LookForward();
            CheckForCollisions();

        }

        private void LateUpdate()
        {
            if (!CanFly())
                return;

            //LookForward();
            if (frameFix)
            {
                if(thisRB.velocity.magnitude > 0.1f)
                    frameFix = false;

                transform.forward = shootDir;
            }

                
        }

        //this arrow still on the air?
        private bool CanFly()
        {
            return launched && !hitSomething && grabbable.CurrentGrabState != GrabState.Grab;
        }

        //match arrow forward vector with rigidbody velocity
        private void LookForward()
        {
            //first calculate the angle difference between the arrow, and the rb velocity
            float angle = Vector3.Angle( transform.forward, thisRB.velocity.normalized );

            //if the angle difference is really low, then just adding it directly, there is no need for lerp
            if (angle <= minLerpAngle)
            {
                transform.forward = thisRB.velocity.normalized;               
            }            
           

            //if the difference is too much and it will be noticeable for the player the angle change,
            //then use smoothdamp to match your desire angle slowly
            else
            {                
                transform.forward = Vector3.SmoothDamp( transform.forward, thisRB.velocity.normalized , ref velocity , smoothSpeed  );               
            }

            Vector3 v = thisRB.velocity.normalized;

            if (v != Vector3.zero)
            {
                transform.forward = thisRB.velocity.normalized;
            }           
            
        }

        //look for collisions
        private void CheckForCollisions()
        {
            //we ignore collision for a small time so we dont collide with objects that the hand is already touching
            if (ShouldIgnoreCollision())
                return;

            RaycastHit[] hitArray = null;
            float d = ( lastArrowHeadPosition - arrowHead.position ).magnitude;
            Vector3 dir = ( arrowHead.position - lastArrowHeadPosition ).normalized;
            
            hitArray = Physics.RaycastAll( lastArrowHeadPosition, dir, d , shootInfo.hitLayer.value);

            distanceTraveled += d;

            float maxDistance = float.MinValue;
            RaycastHit hitInfo = new RaycastHit();
            bool processHit = false;

            for (int n = 0; n < hitArray.Length; n++)
            {
                if (hitArray[n].collider != playerCollider && hitArray[n].collider != thisCollider)
                {
                    float distance = ( hitArray[n].point - arrowHead.position ).magnitude;

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        hitInfo = hitArray[n];
                        processHit = true;
                    }
                }
            }

            lastArrowHeadPosition = arrowHead.position;

            //if we should process this hit
            if (processHit)
            {
                ProcessHit( hitInfo );
            }

        }

        private bool ShouldIgnoreCollision()
        {
            return ignoreCollisionTimer < IGNORE_COLLISION_TIME;
        }

        private void ProcessHit(RaycastHit hitInfo)
        {
           
            TryDoDamage( hitInfo.collider );

            //enable grabbable
            VR_Grabbable thisGrabbable = GetComponent<VR_Grabbable>();

            if (thisGrabbable != null)
                thisGrabbable.enabled = true;


            float sqrDistance = ( arrowHead.position - hitInfo.point ).sqrMagnitude;

            //check that the arrow is no to much inside object
            if (sqrDistance > arrowDepth * arrowDepth)
            {
                transform.position = hitInfo.point - transform.forward * arrowDepth;
            }


            
            SurfaceDetails surfaceDetails = hitInfo.collider.GetComponent<SurfaceDetails>();

            if (surfaceDetails == null || surfaceDetails.StickArrows)
            {
                TryToSitckInSurface(hitInfo);
            }

            hitSomething = true;
            launched = false;



        }

        private void TryToSitckInSurface(RaycastHit hitInfo)
        {
            Rigidbody rb = hitInfo.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                StickToRigidBody(rb);
            }
            else
            {
                VR_Grabbable grabbable = VR_Manager.instance.GetGrabbableFromCollider(hitInfo.collider);
                rb = grabbable != null ? grabbable.GetComponent<Rigidbody>() : null;

                if (rb != null && !rb.isKinematic)
                {
                    StickToRigidBody(rb);
                }
                else
                {
                    //m_rb.isKinematic = true;
                    if (shouldParent && hitInfo.collider.transform.localScale == Vector3.one)
                    {
                        transform.parent = hitInfo.collider.transform;
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }

                    else
                    {
                        thisRB.isKinematic = true;
                    }
                }
            }
        }

        //paste a arrow to a rb
        public void StickToRigidBody(Rigidbody rb)
        {
            
            joint = GetComponent<FixedJoint>();

            if(joint == null)
                joint = gameObject.AddComponent<FixedJoint>();

            joint.connectedBody = rb;
            joint.enableCollision = false;
            joint.breakForce = float.MaxValue;
            joint.breakTorque = float.MaxValue;
            joint.massScale = MASS_SCALE;

            thisCollider.enabled = false;

            //notify the connection, so we can track the numbers of connections per rigidbody and avoid jittering
            ArrowConnections.Add(rb , this);

            stickToRB = true;
        }


        private void OnGrabStateChange(GrabState state)
        {
            if ( !grabProcessed && (state == GrabState.Grab || state == GrabState.Flying) )
            {
                hitSomething = true;
                grabProcessed = true;

                //if we have a joint destroy it
                Joint j = GetComponent<Joint>();               

                if (j != null)
                {
                    ArrowConnections.Remove( j.connectedBody, this );
                    Destroy( j );
                }

                stickToRB = false;

            }
        }

        public override void Launch(ShootInfo info)
        {
            base.Launch( info );

            lastArrowHeadPosition = arrowHead.position;

            thisCollider.enabled = false;
            transform.parent = null;
            thisRB.isKinematic = false;
            transform.forward = info.dir;
            shootDir = info.dir;
            frameFix = true;
            thisRB.AddForce( info.dir * info.speed, ForceMode.VelocityChange );

            hitSomething = false;
            grabProcessed = false;

            StartCoroutine(DisableForOneFrame());

        }

        private IEnumerator DisableForOneFrame()
        {
            //SetVisivility(false);
            yield return new WaitForSeconds(0.2f);
           // SetVisivility( true );
        }

        private void SetVisivility(bool visibility)
        {
            MeshRenderer[] renderArray = gameObject.GetComponentsInChildren<MeshRenderer>();

            for (int n = 0; n < renderArray.Length; n++)
            {
                renderArray[n].enabled = visibility;
            }
        }

    }

}

