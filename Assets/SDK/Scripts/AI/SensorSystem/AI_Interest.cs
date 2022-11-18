using UnityEngine;

namespace VRShooterKit.AI.SensorSystem
{
    [System.Serializable]
    public class AI_Interest
    {
        public Vector3 lastKnowPosition = new Vector3();
        public Stimulation stimulation = null;
        public bool inRange = false;
        public bool canBeHeared = false;
        public bool isAttackingMe = false;
        public float inRangeAmount = 0.0f;


        public AI_Interest(Stimulation stim, float inRangeAmount)
        {
            this.inRangeAmount = inRangeAmount;
            inRange = false;
            stimulation = stim;
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            lastKnowPosition = stimulation.Collider.bounds.center;
        }

        public float Distance(Vector3 point)
        {
            return Vector3.Distance( point, lastKnowPosition );
        }
    }
}

