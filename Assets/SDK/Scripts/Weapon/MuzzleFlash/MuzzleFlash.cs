using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    public class MuzzleFlash : MonoBehaviour
    {
        #region MuzzleFlash
        [SerializeField] protected Vector3 m_positionOffset = Vector3.zero;
        [SerializeField] protected bool m_parent = false;
        [SerializeField] protected bool m_disableWhileNoShooting = true;
        [SerializeField] protected float m_visibleTime = 0.2f;
        #endregion

        public bool IsVisible => gameObject.activeInHierarchy;

        protected Transform m_muzzleFlashPivot = null;
        private float m_muzzleExpireTime = 0.0f;

        public virtual void Initialize(Transform pivot)
        {
            m_muzzleFlashPivot = pivot;

            if (!m_parent)
            {
                transform.parent = null;
            }

            SetVisibility(false);
        }

        public virtual void Fire()
        {
            if (!m_parent && m_muzzleFlashPivot != null)
            {
                transform.position = m_muzzleFlashPivot.position + m_positionOffset;
                transform.rotation = m_muzzleFlashPivot.rotation;
            }

            SetVisibility(true);
            m_muzzleExpireTime = Time.time + m_visibleTime;
        }

        public virtual void InternalUpdate(bool shoot)
        {
            if (!shoot && m_disableWhileNoShooting )
            {
                SetVisibility(false);
            }

            if (MuzzleFlashExpireTimePassed())
            {
                SetVisibility(false);
            }
        }
        
        public void HideMuzzleFlashWhileNoShooting()
        {
            if (m_disableWhileNoShooting) SetVisibility(false);
        }

        public virtual void SetVisibility(bool visibility)
        {
            gameObject.SetActive(visibility);
        }
        
        private bool MuzzleFlashExpireTimePassed()
        {
            return m_muzzleExpireTime < Time.time;
        }

    }

}

