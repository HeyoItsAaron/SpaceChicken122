using UnityEngine;

namespace VRShooterKit.AI.SensorSystem
{
    public class VisualSensor : AIRangeSensor
    {
        public bool debugOn = false;
        #region INSPECTOR
        [SerializeField] [Range( 1.0f, 360.0f )] private float visualAngle = 1.0f;
        [SerializeField] private LayerMask visualLayer;
        #endregion


        private Ray visualRay
        {
            get
            {
                return new Ray(transform.position , transform.forward);
            }
        }


        //chekc if the collider is visible
        protected override float CalculateInRangeAmount(Stimulation stim)
        {

            Vector3 stimDirection = (stim.Collider.bounds.center - thisCollider.bounds.center).normalized;
            Vector3 thisForward = transform.forward;

            float angle = Vector2.Angle( new Vector2(stimDirection.x , stimDirection.z) , new Vector2( thisForward.x , thisForward.z ) );


            if( angle > visualAngle)
                return 0.0f;            

            RaycastHit[] hitInfo = Physics.RaycastAll( new Ray(thisCollider.bounds.center , stimDirection) , currentRange , visualLayer.value , QueryTriggerInteraction.Ignore );

            Collider closeCollider = null;
            float minDistance = float.MaxValue;


            for (int n = 0; n < hitInfo.Length; n++)
            {
                if (hitInfo[n].distance < minDistance)
                {
                    minDistance = hitInfo[n].distance;
                    closeCollider = hitInfo[n].collider;
                }
            }


            //Debug.Log(closeCollider.name);
            if (closeCollider == null)
            {
                return 0.0f;
            }

           
            if (closeCollider == stim.Collider)
            {
                float visibilityAmount = 0.0f;
                visibilityAmount += Mathf.Abs( ( ( Mathf.Min( Mathf.Abs( angle ), visualAngle ) / visualAngle ) / 2.0f ) - 0.5f );

               
                Vector3 targetPos = stim.Collider.bounds.center;
                Vector3 centerPos = thisCollider.bounds.center;

                float distance = Vector3.Distance( targetPos, centerPos );
                visibilityAmount += Mathf.Abs( ( ( Mathf.Min( distance, range ) / range ) / 2.0f ) - 0.5f );

                return visibilityAmount;
            }


            return 0.0f;
        }



        protected override void OnSensorUpdate()
        {

            if (owner == null)
                return;

            for (int n = 0; n < InterestList.Count; n++)
            {



                //we have this becase first we can no see stimlations of visual type none,
                //but i can no be 100% sure that the visual type of a stimulation of visualType none,
                //will be none always, so we will be checking this
                if (InterestList[n].stimulation.VisualType == VisualType.None)
                {
                    InterestList[n].inRange = false;

                }
                else
                {
                    if (InterestList[n].inRange)
                    {


                        InterestList[n].inRange = InterestList[n].stimulation.gameObject.activeInHierarchy && CheckIfStillInRange( InterestList[n].stimulation );
                        InterestList[n].UpdatePosition();
                        InterestList[n].inRangeAmount = CalculateInRangeAmount( InterestList[n].stimulation );
                    }
                    else
                    {

                        InterestList[n].inRange = InRange( InterestList[n].stimulation );
                    }
                }


                
            }
        }


        protected override bool CheckIfStillInRange(Stimulation stim)
        {


            float visibilityAmount = CalculateInRangeAmount( stim );

            if (visibilityAmount < MIN_EXIT_VALUE)
            {
                float blurryAmount = Mathf.Abs( visibilityAmount - 1.0f );


                return Mathf.Abs( ( blurryAmount - owner.Intelligence ) ) > MIN_EXIT_VALUE;
            }


            return true;
        }

        protected override bool InRange(Stimulation stim)
        {
            float visibilityAmount = CalculateInRangeAmount( stim );

           
            //if(debugOn)
            //   Debug.Log(visibilityAmount);


            //if it is smart that normal
            if (owner.Intelligence > 0.5f)
            {
                float v = owner.Intelligence / 2.0f;
                visibilityAmount = Mathf.Min( visibilityAmount + ( visibilityAmount * v ), 1.0f );               
            }
            //if it is dumb that normal
            else if (owner.Intelligence <= 0.5f)
            {
                float v = Mathf.Abs( ( owner.Intelligence - 0.5f ) / 2.0f);
                visibilityAmount = Mathf.Max( visibilityAmount - ( visibilityAmount * v ), 0.0f );
            }


            return visibilityAmount > MIN_ENTER_VALUE;

        }


    }
}

