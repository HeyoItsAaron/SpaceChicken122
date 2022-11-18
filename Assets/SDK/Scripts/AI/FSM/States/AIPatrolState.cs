using UnityEngine;
using UnityEngine.AI;

namespace VRShooterKit.AI
{
    public class AIPatrolState : AIState
    {
        [SerializeField] private AISpeed speed = AISpeed.SlowWalk;
        [SerializeField] [Range( 0.1f, 10.0f )] private float slerpSpeed = 3.0f;
        [SerializeField] private float slerpSpeedDistanceFactor = 10.0f;
        [SerializeField] private float minPatrolDistance = 5.0f;
        [SerializeField] private float stoppingDistance = 0.25f; 

        private bool pathComplete = false;
                
        public override int GetStateIndex()
        {
            return (int) AIStateType.Patrol;
        }

        public override void OnEnterState()
        {
            //Debug.Log( "Entering state " + (AIStateType) GetStateIndex() );

            base.OnEnterState();



            AI.Obstacle.enabled = false;
            AI.NavAgent.enabled = true;            

            pathComplete = false;

            AI.SetAnimatorRootMotionControl( useRootMotionPosition, useRootMotionRotation );
            AI.SetNavAgentControl( updateNavAgentPosition, updateNavAgentRotation );

            //set path listeners
            AI.PathGenerator.OnPathComplete += OnGenerateRandomPatrolPathComplete;
            AI.PathGenerator.OnPathFailure += OnGenerateRandomPatrolPathFailure;

            GenerateRandomPatrolPath();
        }

        private void GenerateRandomPatrolPath()
        {
            pathComplete = false;

            //Vector3 position = NavMeshExtension.CalculateRandomPointInsideNavMesh( AI.transform.position, patrolRadius, 1 );
            Vector3 position = SamplePatrolPointPosition( AI.PatrolPoinstProvider.GetRandomPatrolPoint().position );

            AI.PathGenerator.CalculatePath(position);
        }

        public Vector3 SamplePatrolPointPosition(Vector3 point)
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition( point, out hit, 1.0f, 1 ))
            {
                return hit.position;
            }

            return point;
        }

        private void OnGenerateRandomPatrolPathComplete()
        {

            if ( !PathIsValid() )
            {
                GenerateRandomPatrolPath();
                return;
            }

            AI.Speed = AISpeed.Stopped;
            pathComplete = true;

        }

        private bool PathIsValid()
        {
            return CalculateDistanceToPathEnd() > minPatrolDistance;
        }

        private void OnGenerateRandomPatrolPathFailure()
        {
            GenerateRandomPatrolPath();
        }

        public override int OnUpdate()
        {
            if (AI.CurrentInterest != null && AI.CurrentInterest.inRange && AI.LastKonwDistanceToInterest > AI.NavAgent.stoppingDistance)
                return (int) AIStateType.Pursuit;

            if (!pathComplete)
                return GetStateIndex();


            float distance = CalculateDistanceToPathEnd();

            //rotate slowly to desire point
            if (NavAgentVelocityIsValid() && !AI.NavAgent.pathPending)
            {
                if (AI.Speed != speed)
                    AI.Speed = speed;

                Quaternion newRotation = Quaternion.LookRotation( AI.NavAgent.desiredVelocity );
                AI.transform.rotation = Quaternion.Slerp( AI.transform.rotation, newRotation, Time.deltaTime * slerpSpeed * ( slerpSpeedDistanceFactor / distance ) );
            }
            else if (AI.Speed != AISpeed.Stopped)
            {
                AI.Speed = AISpeed.Stopped;
            }
          
            if (ReachTargetPosition())
            {
                return (int) AIStateType.Idle;
            }

           
            return GetStateIndex();
        }

        private bool NavAgentVelocityIsValid()
        {
            return AI.NavAgent.desiredVelocity != Vector3.zero;
        }

        private bool ReachTargetPosition()
        {
            return CalculateDistanceToPathEnd() < stoppingDistance;
        }

        private float CalculateDistanceToPathEnd()
        {
            return Vector3.Distance( AI.transform.position, AI.NavAgent.pathEndPosition );
        }

        public override void OnExitState()
        {
            base.OnExitState();

            //set path listeners
            AI.PathGenerator.OnPathComplete -= OnGenerateRandomPatrolPathComplete;
            AI.PathGenerator.OnPathFailure -= OnGenerateRandomPatrolPathFailure;

            AI.PathGenerator.CancelPathGenerationProcess();
        }
    }
}

