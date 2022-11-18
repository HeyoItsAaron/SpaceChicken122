using UnityEngine;

namespace VRShooterKit
{
    [RequireComponent( typeof( TubeRenderer) )]
    public class LaserSight : MonoBehaviour
    {
        [SerializeField] private float depth = 1000.0f;
        [SerializeField] private LayerMask layerMask;        
        [SerializeField] private Transform laserPoint = null;
        [SerializeField] private GameObject tip = null;

        private TubeRenderer tubeRenderer = null;
        private Vector3[] positions = new Vector3[2];
        private bool laserSightIsEnable = false;

        private void Awake()
        {
            tubeRenderer = GetComponent<TubeRenderer>();
        }

        private void Update()
        {
            positions[0] = laserPoint.position;
            tip.gameObject.SetActive(false);

            if (laserSightIsEnable)
            {
                RaycastHit hitInfo;

                if (Physics.Raycast(laserPoint.position, laserPoint.forward, out hitInfo, depth, layerMask.value, QueryTriggerInteraction.Ignore))
                {
                    positions[1] = hitInfo.point;
                    tip.gameObject.SetActive(true);
                    tip.transform.position = hitInfo.point;
                }

            }
            else 
            {
                
                positions[1] = laserPoint.position;
            }

            tubeRenderer.SetPositions( positions );
        }

        public void OnEnableLaserSight()
        {
            laserSightIsEnable = true;
        }

        public void OnDisableLaserSight()
        {
            laserSightIsEnable = false;
        }

    }
}

