using UnityEngine;

namespace VRShooterKit.AI.SensorSystem
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AIRangeSensor : AISensor
    {

        [SerializeField] [Range( 0.1f, 80.0f )] protected float range = 1.0f;
        protected SphereCollider thisCollider = null;

        protected float currentRange = 0.0f;

        protected readonly float MIN_ENTER_VALUE = 0.65f;
        protected readonly float MIN_EXIT_VALUE = 0.45f;

        protected override void Awake()
        {
            base.Awake();


            thisCollider = GetComponent<SphereCollider>();
            thisCollider.isTrigger = true;

            currentRange = range;
            thisCollider.radius = currentRange;
        }


        protected virtual void OnTriggerEnter(Collider other)
        {

            Stimulation stim = other.GetComponent<Stimulation>();            

            if (stim != null)
            {              
                InterestList.Add( new AI_Interest( stim, CalculateInRangeAmount( stim ) ) );
            }
        }


        protected virtual void OnTriggerExit(Collider other)
        {

            for (int n = 0; n < InterestList.Count; n++)
            {
                if (InterestList[n] == null)
                {
                    InterestList.RemoveAt( n );
                    n--;
                }
                
                else if (InterestList[n].stimulation == null || InterestList[n].stimulation.gameObject == other.gameObject)
                {
                    InterestList[n].inRange = false;
                    InterestList.RemoveAt( n );
                    n--;
                }
            }
        }

        protected abstract float CalculateInRangeAmount(Stimulation stim);
        protected abstract bool InRange(Stimulation stim);
        protected abstract bool CheckIfStillInRange(Stimulation stim);

       
        public void SetSensorFocus(float f)
        {
            currentRange = range * f;
            thisCollider.radius = currentRange;
        }

    }
}

