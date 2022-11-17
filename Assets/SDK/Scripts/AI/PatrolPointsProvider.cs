using UnityEngine;

namespace VRShooterKit
{
    public class PatrolPointsProvider : MonoBehaviour
    {
        public Transform GetRandomPatrolPoint()
        {
            return transform.GetChild( Random.Range(0 , transform.childCount) );
        }
    }
}

