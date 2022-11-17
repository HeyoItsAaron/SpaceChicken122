using UnityEngine;
using UnityEngine.AI;

namespace VRShooterKit.AI
{
    public class AIPursuitState : AIState
    {  
        [SerializeField] private AISpeed speed = AISpeed.Sprint;        
        [SerializeField] private float slerpSpeed = 5.0f;
        [SerializeField] private float repathDistanceMultiplier = 0.035f;
        [SerializeField] private float minRepathTime = 0.0f;
        [SerializeField] private float maxRepathTime = 0.0f;

        private float repathTimer = 0.0f;

        public override void OnEnterState()
        {
            //Debug.Log("Entering state " + (AIStateType) GetStateIndex());

            AI.Obstacle.enabled = false;
            AI.NavAgent.enabled = true;
            

            AI.SetAnimatorRootMotionControl(useRootMotionPosition, useRootMotionRotation);
            AI.SetNavAgentControl(updateNavAgentPosition, updateNavAgentRotation);
           

            AI.Speed = 0;
            repathTimer = 0.0f;

            //caluclate path
            AI.NavAgent.SetDestination(AI.CurrentInterest.lastKnowPosition);
            AI.NavAgent.isStopped = false;

        }

        public override void OnExitState()
        {
            
        }

        public override int OnUpdate()
        {
            if (AI.CurrentInterest == null || AI.CurrentInterest.stimulation == null)
                return (int) AIStateType.Idle;

            if (!AI.NavAgent.enabled)
                return  GetStateIndex();

           
            if ( !CurrentPathIsValid() )
            {
                AI.NavAgent.SetDestination( AI.CurrentInterest.lastKnowPosition );
                return GetStateIndex();
            }


            
            //if path is being computed
            if (PathIsBeingComputed())
            {
                return GetStateIndex();
            }
            else
            {
                if (AI.Speed != speed)
                {
                    AI.Speed = speed;
                }

                //float distanceToPathEnd = Vector3.Distance( AI.transform.position, AI.NavAgent.pathEndPosition );
                float distanceToPathEnd = ( AI.transform.position - AI.NavAgent.pathEndPosition ).magnitude;

                if (distanceToPathEnd < AI.NavAgent.stoppingDistance * AI.NavAgent.stoppingDistance)
                {
                    AI.RemoveCurrentInterestFromSensors();
                    return (int) AIStateType.Idle;
                }

                //update time
                repathTimer += Time.deltaTime;
                
                
                float distanceToTarget = 0.0f;

                if (AI.NavAgent.pathStatus == NavMeshPathStatus.PathPartial || AI.CurrentInterest == null)
                {
                    //distanceToTarget = Vector3.Distance( AI.transform.position, AI.NavAgent.destination );
                    distanceToTarget = distanceToPathEnd;
                }

                else
                {
                    //distanceToTarget = Vector3.Distance( AI.transform.position, AI.CurrentInterest.lastKnowPosition );
                    distanceToTarget = ( AI.transform.position - AI.CurrentInterest.lastKnowPosition ).magnitude;
                }


                if (distanceToTarget <= AI.MaxAttackRange * AI.MaxAttackRange)
                {
                    if (AI.NavAgent.pathStatus == NavMeshPathStatus.PathPartial || AI.CurrentInterest == null  || !AI.CurrentInterest.inRange)
                        return (int)AIStateType.Idle;
                    if (AI.RealDistanceToInterest < AI.MaxAttackRange && AI.CurrentInterest.inRange && AI.NavAgent.pathStatus == NavMeshPathStatus.PathComplete)
                        return (int) AIStateType.Attack;
                }

                //rotate slowly to desire point
                if (AI.NavAgent.desiredVelocity != Vector3.zero)
                {
                    Quaternion newRotation = Quaternion.LookRotation(AI.NavAgent.desiredVelocity);
                    AI.transform.rotation = Quaternion.Slerp( AI.transform.rotation, newRotation, Time.deltaTime * slerpSpeed);
                }

                if (AI.CurrentInterest != null && Mathf.Clamp(distanceToTarget * repathDistanceMultiplier, minRepathTime, maxRepathTime) < repathTimer)
                {
                    //repath
                    AI.NavAgent.SetDestination( AI.CurrentInterest.lastKnowPosition );
                    repathTimer = 0.0f;
                }

                

            }

            return GetStateIndex();
        }

        public override int GetStateIndex()
        {
            return (int) AIStateType.Pursuit;
        }

        private bool CurrentPathIsValid()
        {
            return AI.NavAgent.pathStatus != NavMeshPathStatus.PathInvalid;
        }

        private bool PathIsBeingComputed()
        {
            return AI.NavAgent.pathPending;
        }

    }

}

