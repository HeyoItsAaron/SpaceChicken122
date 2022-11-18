using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    public enum BowControlMode
    {
        Realistic,
        Unrealistic
    }


    public class BowController : MonoBehaviour
    {
        #region INSPECTOR
        [SerializeField] private BowControlMode bowControlMode = BowControlMode.Realistic;
        [SerializeField] private VR_GrabbableZone arrowGrabZone = null;
        [SerializeField] private Arrow arrow = null;
        [SerializeField] private BowStringController bowStringController = null;
        [SerializeField] private Transform arrowLookAtPoint = null;
        [SerializeField] private LayerMask hitLayerMask;
        [SerializeField] private float dmg = 0.0f;
        [SerializeField] private float arrowMaxSpeed = 200.0f;
        [SerializeField] private float arrowMinSpeed = 10.0f;
        [SerializeField] private float hitForce = 80.0f;
        [SerializeField] private float speedFactor = 10.0f;
        [SerializeField] private float bowMaxDistanceResistance = 1.2f;
        [SerializeField] private BowStringController stringController = null;
        [SerializeField] private BowFlexiblePart[] bowFlexiblePartArray = null;
        #endregion

        #region PRIVATE
        private VR_Grabbable thisGrabbable = null;
        private VR_Controller leftController = null;
        private VR_Controller rightController = null;
        private VR_Controller arrowController = null;
        private Arrow currentArrow = null;
        private float stringGrabDistance = 0.0f;
        private bool shootIntent = false;
        private Vector3 lastArrowDirection = Vector3.zero;
        private Transform player = null;

        public bool HasArrow { get { return currentArrow != null; } }
        #endregion

        

        private void Start()
        {
            thisGrabbable = GetComponent<VR_Grabbable>();
            GameObject go = GameObject.FindGameObjectWithTag("Player");

            if (go != null) player = go.transform;

            if(stringController == null)
                stringController = transform.GetComponentInChildren<BowStringController>();

            bowStringController.Grabbable.OnGrabStateChange.AddListener( OnStringGrabStateChange );
            thisGrabbable.OnGrabStateChange.AddListener( OnThisGrabStateChange );

            leftController = VR_Manager.instance.Player.LeftController;
            rightController = VR_Manager.instance.Player.RightController;

            //try to get the grabzone
            if (arrowGrabZone == null && bowControlMode == BowControlMode.Realistic)
            {
                arrowGrabZone = FindArrowGrabbableZone();

                if (arrowGrabZone == null)
                {
                    Debug.LogError("Realistic bow needs a arrow grabzone in order to work!");
                }
            }

            if(arrowGrabZone != null)
                arrowGrabZone.enabled = false;
            
            bowStringController.Grabbable.enabled = false;
            stringGrabDistance = bowStringController.Grabbable.GrabDistance;
        }

        private VR_GrabbableZone FindArrowGrabbableZone()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                return player.GetComponentInChildren<VR_GrabbableZone>();
            }

            return null;

        }

        private void Update()
        {            
           
            if (thisGrabbable.GrabController == null)
                return;

            UpdateControlPositionMode();            
            UpdateFlexibleParts();
            CheckIfCanUseGrabbeArrow();
        }
      

        private void UpdateControlPositionMode()
        {
            if (currentArrow != null)
            {
                if (thisGrabbable.CurrentGrabState != GrabState.Grab)
                {
                    SetControlPositionMode( bowStringController.Grabbable.GrabController, MotionControlMode.Engine );
                    return;
                }

                if (!IsOverCorrectPosition())
                {
                    SetControlPositionMode( bowStringController.Grabbable.GrabController, MotionControlMode.Free );
                    bowStringController.Grabbable.GrabController.transform.parent = player;
                }
                else
                {
                    SetControlPositionMode( bowStringController.Grabbable.GrabController, MotionControlMode.Engine );
                }

            }
        }

        private bool IsOverCorrectPosition()
        {            
            float distance = Vector3.Distance( arrowLookAtPoint.position, bowStringController.Grabbable.GrabController.OriginalParent.position );
            return distance < currentArrow.Size;
        }

        private void SetControlPositionMode(VR_Controller controller, MotionControlMode mode)
        {
            if (controller.ControlPositionMode != mode)
            {
                controller.SetPositionControlMode( mode );
                controller.SetRotationControlMode( mode );
            }
        }

        private void UpdateFlexibleParts()
        {
            float d = Vector3.Distance( bowStringController.Middle.position, bowStringController.MiddleStart.position );
            float stringStress = Mathf.Min( d / bowMaxDistanceResistance, 1.0f );

            //avoid jittering in the stirng
            if (bowStringController.Grabbable.CurrentGrabState != GrabState.Grab && stringStress < 0.2f)
            {
                stringStress = 0.0f;
            }

            for (int n = 0; n < bowFlexiblePartArray.Length; n++)
            {
                bowFlexiblePartArray[n].SetFlexibleValue( stringStress );
            }
        }

        private void CheckIfCanUseGrabbeArrow()
        {
            if (bowControlMode == BowControlMode.Realistic && currentArrow == null && CanUseArrow())
            {
                VR_Controller controller = thisGrabbable.GrabController == rightController ? leftController : rightController;
                GameObject go = controller.CurrentGrab.gameObject;
                controller.CurrentGrab.ForceDrop();
                controller.ForceGrab( bowStringController.Grabbable );
                UseArrow( go.GetComponent<Arrow>() );
            }
        }

        private float GetDistanceToController()
        {
            VR_Controller targetController = thisGrabbable.GrabController == rightController ? leftController : rightController;
            return Vector3.Distance( bowStringController.Middle.position , targetController.GrabPoint.position );            
        }

        private bool CanUseArrow()
        {
            VR_Controller targetController = thisGrabbable.GrabController == rightController ? leftController : rightController;

            if (targetController.CurrentGrab == null)
                return false;

            if (GetDistanceToController() > stringGrabDistance)
                return false;

            Arrow arrow = targetController.CurrentGrab.GetComponent<Arrow>();

            return arrow != null;

        }  

        private void LateUpdate()
        {

            if (shootIntent)
            {
                shootIntent = false;
                Shoot();
            }

            if (currentArrow != null)
            {
                if (currentArrow.transform.parent != player)
                    currentArrow.transform.parent = player;

                if (bowStringController.Grabbable.GrabController.ControlPositionMode == MotionControlMode.Free)
                {
                    float d = currentArrow.Size;
                    Vector3 dir = ( bowStringController.Grabbable.GrabController.OriginalParent.position - arrowLookAtPoint.position).normalized;
                    

                    bowStringController.Grabbable.GrabController.transform.position = arrowLookAtPoint.position + ( dir * d );
                }

                currentArrow.transform.position = bowStringController.Grabbable.GrabController.transform.position;
                SetArrowRotation();
            }

            stringController.Render();
        }

       
        private void SetArrowRotation()
        {
            Vector3 dir = arrowLookAtPoint.position - bowStringController.Middle.transform.position;
            currentArrow.transform.rotation = Quaternion.LookRotation( dir );
            lastArrowDirection = dir;
        }

        private void OnStringGrabStateChange(GrabState state)
        {
            if (state == GrabState.Grab && currentArrow == null)
            {
                if (bowControlMode == BowControlMode.Unrealistic)
                {
                    CreateArrow();
                    SetArrowRotation();
                }

                arrowController = bowStringController.Grabbable.GrabController;
                
            }
            else if (state == GrabState.Drop)
            {

                if (currentArrow == null)
                {
                    SetControlPositionMode( arrowController, MotionControlMode.Engine );                   
                }
                else
                {
                    shootIntent = true;
                }

              
            }
        }

        private void Shoot()
        {

            ShootInfo info = new ShootInfo();
            float d = Vector3.Distance( bowStringController.Middle.position, bowStringController.MiddleStart.position );
            info.speed = Mathf.Min( (currentArrow.Size / (d < 1.0f ? 1.0f : d) ) * speedFactor, arrowMaxSpeed );            
            info.dir = lastArrowDirection;
            info.hitForce = hitForce * ( info.speed / arrowMaxSpeed );
            info.dmg = dmg * ( info.speed / arrowMaxSpeed );
            info.hitLayer = hitLayerMask;
            info.sender = player.gameObject;

            currentArrow.Launch( info );
            currentArrow = null;

            SetControlPositionMode( arrowController, MotionControlMode.Engine );
           
        }
       
        private void CreateArrow()
        {
            currentArrow = Instantiate( arrow );
            currentArrow.transform.parent = bowStringController.Middle;
            currentArrow.transform.localPosition = Vector3.zero;
            Rigidbody rb = currentArrow.GetComponent<Rigidbody>();

            if (rb != null)
                rb.isKinematic = true;
        }

        private void UseArrow(Arrow arrow)
        {
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            currentArrow = arrow;
            currentArrow.transform.parent = bowStringController.Middle;
            currentArrow.transform.localPosition = Vector3.zero;
            SetArrowRotation();
        }

        private void OnThisGrabStateChange(GrabState state)
        {
            if (state == GrabState.Grab && !bowStringController.Grabbable.enabled)
            {
                bowStringController.Grabbable.enabled = true;
                ChangeArrowGrabZoneEnable(true);
            }

            else if (state == GrabState.Drop && bowStringController.Grabbable.enabled)
            {
                bowStringController.Grabbable.enabled = false;

                if(bowStringController.Grabbable.CurrentGrabState != GrabState.UnGrab)
                    bowStringController.Grabbable.ForceDrop();

                ChangeArrowGrabZoneEnable(false);
            }

        }

        private void ChangeArrowGrabZoneEnable(bool value)
        {

            if (arrowGrabZone == null)
                return;

            arrowGrabZone.enabled = value;
        }

    }

}

