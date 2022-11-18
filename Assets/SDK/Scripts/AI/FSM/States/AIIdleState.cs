using UnityEngine;

namespace VRShooterKit.AI
{
    public class AIIdleState : AIState
    {
        [SerializeField] private float minIdleTime = 5.0f;
        [SerializeField] private float maxIdleTime = 10.0f;

        private float idleTimer = 0.0f;
      
        public override int GetStateIndex()
        {
            return (int) AIStateType.Idle;
        }

        public override void OnEnterState()
        {
            //Debug.Log( "Entering state " + (AIStateType) GetStateIndex() );

            AI.NavAgent.enabled = false;
            AI.Obstacle.enabled = true;

            AI.SetAnimatorRootMotionControl( useRootMotionPosition, useRootMotionRotation );
            AI.SetNavAgentControl( updateNavAgentPosition, updateNavAgentRotation );
            
            AI.Speed = 0;
            idleTimer = Random.Range(minIdleTime , maxIdleTime);
                      
        }

        public override int OnUpdate()
        {
            if (AI.CurrentInterest != null && AI.CurrentInterest.inRange)
            {
                if (AI.RealDistanceToInterest < AI.MaxAttackRange)
                {
                    return (int) AIStateType.Attack;
                }

                if (AI.LastKonwDistanceToInterest > AI.NavAgent.stoppingDistance)
                {
                    return (int) AIStateType.Pursuit;
                }
            }

            

            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0.0f)
                return (int) AIStateType.Patrol;

            return GetStateIndex();
        }

    }

}

