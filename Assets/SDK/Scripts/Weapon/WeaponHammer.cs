using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    public class WeaponHammer : MonoBehaviour
    {
        [SerializeField] private Vector3 axis = Vector3.zero;
        [SerializeField] private float startRotation = 0.0f;
        [SerializeField] private float endRotation = 0.0f;

        private bool isAnimating = false;
        private bool isReturningToStart = false;
       
        void Update()
        {
            if (isAnimating)
            {
                float targetAngle = isReturningToStart ? startRotation : endRotation;
                Quaternion target = Quaternion.AngleAxis( targetAngle, axis );

                transform.localRotation = Quaternion.Lerp( transform.localRotation, target, isReturningToStart ? 0.1f : 0.25f );
                float angle = Quaternion.Angle( transform.localRotation, target );

                if (angle < 1.0f)
                {
                    if (isReturningToStart)
                    {
                        isAnimating = false;
                    }
                    else
                    {
                        isReturningToStart = true;
                    }
                }
            }
        }

        public void Shoot()
        {
            isAnimating = true;
            isReturningToStart = false;
        }
    }
}

