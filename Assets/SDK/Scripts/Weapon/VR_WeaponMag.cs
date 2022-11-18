using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    //this script controls the weapons magazine, 
    //use in weapons that use realistic reload mode
    [RequireComponent( typeof( VR_Grabbable ) )]
    public class VR_WeaponMag : MonoBehaviour
    {
        #region INSPECTOR       
        [SerializeField] private int bullets = 30;
        #endregion

        #region PRIVATE        
        private new Collider collider = null;
        private Rigidbody rb = null;
        private VR_Weapon owner = null;
        #endregion

        #region PUBLIC
        public int Bullets { get { return bullets; } set { bullets = value; } }       
        #endregion

        private void Awake()
        {           
            collider = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
        }
       
        public void SetOwner(VR_Weapon owner)
        {
            this.owner = owner;

            if (collider != null)
                collider.enabled = false;

            if (rb != null)
                rb.isKinematic = true;
        }

    }

}

