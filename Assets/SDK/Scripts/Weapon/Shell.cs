using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    //this script is being used in the revolver for the physics reload mode,
    //eject the bullets
    public class Shell : MonoBehaviour
    {
        [SerializeField] private float ejectForce = 50.0f;

        private Rigidbody rb = null;
        private Collider thisCollider = null;
        private bool ejected = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            thisCollider = GetComponent<Collider>();
            thisCollider.enabled = false;
#if UNITY_2019_1_OR_NEWER
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
#endif
            rb.isKinematic = true;
        }

        public void Eject()
        {
            if (ejected)
                return;

            float ejectAngle = Vector2.Angle(new Vector2(transform.up.x , transform.up.y) , Vector2.up);
            float currentEjectForce = ( ejectAngle / 180.0f ) * ejectForce;
            

            transform.parent = null;
            ejected = true;
            rb.isKinematic = false;
            rb.AddForce(transform.up * currentEjectForce * -1.0f );

            thisCollider.enabled = true;

        }

      

    }

}

