using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    public class BowFlexiblePart : MonoBehaviour
    {
        [SerializeField] private float flexibleAngle = 30.0f;
        [SerializeField] private float lerpSpeed = 0.025f;        
        [SerializeField] private Vector3 rotDir = Vector3.zero;
      
        private Quaternion desireRotation = Quaternion.identity;
        private Quaternion startRotation = Quaternion.identity;
        private float desireValue = 0.0f;
               
        private void Awake()
        {
            startRotation = transform.localRotation;
            desireRotation = startRotation;          
        }

        private void Update()
        {
           transform.localRotation = Quaternion.Lerp( transform.localRotation, desireRotation, lerpSpeed  );
        }

        public void SetFlexibleValue(float v)
        {
            Mathf.Clamp(v , 0.0f, 1.0f);
                       
            if (v < 0.01f)
                v = 0.0f;

            float diff = Mathf.Abs(v - desireValue);

            //the change is to small dont take it 
            if (diff < 0.09f)
                return;

            desireValue = v;
            Quaternion rot = Quaternion.identity;
                        
            //calculate the desire value
            desireRotation = startRotation * Quaternion.Euler( rotDir * flexibleAngle * desireValue );
        }

    }

}

