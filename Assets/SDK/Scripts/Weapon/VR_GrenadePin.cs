using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    //this script controls the VR Grenade, when this is remove from the object the grenade is activated

    [RequireComponent( typeof( VR_Grabbable ) )]
    public class VR_GrenadePin : MonoBehaviour
    {
        #region INSPECTOR
        [SerializeField] private VR_Grabbable granadeGrabbable = null;
        [SerializeField] private VR_Grenade grenade = null;
        #endregion

        private VR_Grabbable thisGrabbable = null;
        private Collider thisCollider = null;
        private Rigidbody rb = null;
        private bool pinWasUsed = false;

        private void Start()
        {
            //disable
            grenade.enabled = false;

            rb = GetComponent<Rigidbody>();
            thisCollider = GetComponent<Collider>();
            thisGrabbable = GetComponent<VR_Grabbable>();
            thisGrabbable.OnGrabStateChange.AddListener( OnThisGrabStateChange );
            thisGrabbable.enabled = false;

            granadeGrabbable.OnGrabStateChange.AddListener( OnGrenadeGrabStateChange );

            if (thisCollider != null)
            {
                granadeGrabbable.IgnoreCollider( thisCollider );
                thisCollider.enabled = false;
            }

            if (rb != null)
            {
                rb.isKinematic = true;
            }           


        }

        /// <summary>
        /// Called when the grenade grab state change
        /// </summary>
        /// <param name="grabState"></param>
        private void OnGrenadeGrabStateChange(GrabState grabState)
        {
            if (grabState == GrabState.Grab)
            {
                thisGrabbable.enabled = true;
            }

            else if (!pinWasUsed && grabState == GrabState.Drop && thisGrabbable.CurrentGrabState != GrabState.Grab)
            {
                thisGrabbable.enabled = false;
            }
        }

        /// <summary>
        /// Called when this grab state change
        /// </summary>
        /// <param name="grabState"></param>
        private void OnThisGrabStateChange(GrabState grabState)
        {
            if (grabState == GrabState.Grab && grenade != null && !grenade.enabled)
            {
                grenade.enabled = true;
                pinWasUsed = true;
            }

            if (grabState == GrabState.Drop)
            {
                transform.parent = null;

                if (thisCollider != null)
                {
                    thisCollider.enabled = true;
                }

                if (rb != null)
                {
                    rb.isKinematic = false;
                }
                
            }
        }
    }

}
