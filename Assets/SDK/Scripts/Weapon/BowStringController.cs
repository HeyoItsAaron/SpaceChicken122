using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    //this script controll the bow string, it uses just a line render for now
    public class BowStringController : MonoBehaviour
    {
        [SerializeField] private Transform start = null;
        [SerializeField] private Transform middle = null;
        [SerializeField] private Transform end = null;
        [SerializeField] private VR_Grabbable grabbable = null;
        [SerializeField] private float stringFlyTime = 0.3f;
        [SerializeField] private TubeRenderer tubeRender = null;

        private Transform middleStartPoint = null;
        private float dropStartTime = 0.0f;
        private Vector3 dropStartPosition = Vector3.zero;
        private bool isLerping = false;
     

        public Transform Middle { get { return middle; } }
        public Transform MiddleStart { get { return middleStartPoint; } }
        public VR_Grabbable Grabbable { get { return grabbable; } }

        private void Awake()
        {           
            middleStartPoint = new GameObject( "MiddleStart" ).transform;
            middleStartPoint.parent = middle.parent;
            middleStartPoint.localPosition = middle.localPosition;


            grabbable.OnGrabStateChange.AddListener( OnStrigGrabStateChange );
        }


        private void OnStrigGrabStateChange(GrabState state)
        {
            if (state == GrabState.Drop)
            {                
                dropStartTime = Time.time;
                dropStartPosition = middle.position;
                isLerping = true;

                middle.parent = transform;
            }
        }


        public void Render()
        {

            if (grabbable.CurrentGrabState != GrabState.Grab)
            {


                float progress = ( Time.time - dropStartTime ) / stringFlyTime;
                if (progress < 1 && Time.time > stringFlyTime)
                {

                    middle.position = Vector3.Lerp( dropStartPosition, middleStartPoint.position, progress );
                                       

                    tubeRender.SetPositions( new Vector3[] { start.position, middle.position, end.position } );

                    return;
                }

                if (isLerping)
                    isLerping = false;


                //wait to call drop event
                if (grabbable.CurrentGrabState == GrabState.Drop)
                    return;


                middle.position = middleStartPoint.position;

              
                tubeRender.SetPositions( new Vector3[] { start.position, end.position } );

                return;

            }
                      

            tubeRender.SetPositions( new Vector3[] { start.position, middle.position, end.position } );


        }



    }

}

