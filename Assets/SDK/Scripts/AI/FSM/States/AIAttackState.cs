using UnityEngine;

namespace VRShooterKit.AI
{
    //currently the attack state is juts a placeholder state
    public class AIAttackState : AIState
    {
        [SerializeField] private AISpeed movingSpeed = AISpeed.Walk;
        [SerializeField] private float attackRate = 2.0f;
        [SerializeField] private float attackExitRange = 5.0f;
        [SerializeField] private float slerpSpeed = 10.0f;
        [SerializeField] private float repathDistanceMultiplier = 0.035f;
        [SerializeField] private float minRepathTime = 0.0f;
        [SerializeField] private float maxRepathTime = 0.0f;


        private float timer = 0.0f;
        private float repathTimer = 0.0f;
        private bool attackLayerIsEnable = false;
        private float distanceToTarget;
        private float targetSpeed;



        public override void OnEnterState()
        {
            //Debug.Log( "Entering state " + (AIStateType) GetStateIndex() );

            AI.NavAgent.enabled = true;
            AI.Obstacle.enabled = false;


            AI.SetAnimatorRootMotionControl(useRootMotionPosition, useRootMotionRotation);
            AI.SetNavAgentControl(updateNavAgentPosition, updateNavAgentRotation);
        }

        public override void OnExitState()
        {
            DisableAttackAnimationLayer();
        }

        private void EnableAttackAnimationLayer()
        {
            if (!attackLayerIsEnable)
            {
                AI.AnimatorHelper.EnableLayer( AIEntity.ATTACK_LAYER );
                attackLayerIsEnable = true;
            }
        }

        private void DisableAttackAnimationLayer()
        {
            if (attackLayerIsEnable)
            {
                AI.AnimatorHelper.DisableLayer( AIEntity.ATTACK_LAYER );
                attackLayerIsEnable = false;
            }
        }


        public override int GetStateIndex()
        {
            return (int) AIStateType.Attack;
        }

        public override int OnUpdate()
        {

            if (AI.CurrentInterest == null || AI.CurrentInterest.stimulation == null)
                return (int) AIStateType.Idle;

            distanceToTarget = AI.RealDistanceToInterest;
            targetSpeed = AI.InterestSpeed;

            UpdateCurrentSpeed();


            if (!AI.IsPlayingAttackAnimation)
                timer += Time.deltaTime;


            if (timer >= attackRate && distanceToTarget < AI.MaxAttackRange)
            {
                EnableAttackAnimationLayer();
                AI.Animator.SetTrigger( "Attack" );
                timer = 0.0f;
            }
            else if (attackLayerIsEnable && !AI.IsPlayingAttackAnimation)
            {
                DisableAttackAnimationLayer();
            }       


            Vector3 targetPos = AI.CurrentInterest.stimulation.transform.position;
            targetPos.y = AI.transform.position.y;
            Quaternion newRot = Quaternion.LookRotation( targetPos - AI.transform.position );


            AI.transform.rotation = Quaternion.Slerp( AI.transform.rotation, newRot, Time.deltaTime * slerpSpeed );


            //repath
            //update time
            repathTimer += Time.deltaTime;
            if (distanceToTarget > AI.MinAttackRange && Mathf.Clamp( AI.CurrentInterest.Distance(AI.transform.position) * repathDistanceMultiplier, minRepathTime, maxRepathTime ) < repathTimer)
            {
                //Debug.Log("new path");
                //repath
                if (AI.NavAgent.enabled)
                    AI.NavAgent.SetDestination( AI.CurrentInterest.lastKnowPosition);
                repathTimer = 0.0f;
            }

            if (distanceToTarget > attackExitRange)
                return (int) AIStateType.Pursuit;


            return GetStateIndex();
        }

        private void UpdateCurrentSpeed()
        {/*
            if ( ShouldMove() )
            {

                AISpeed speed = AISpeed.Stopped;


                if (targetSpeed > 4.0f)
                {
                    speed = ShouldAttack() || AI.IsPlayingAttackAnimation ? AISpeed.Sprint : AISpeed.Run;

                    if (distanceToTarget <= AI.InterestObstacleSize)
                    {
                        speed = AISpeed.Walk;
                    }

                }
                else
                {
                    speed = ShouldAttack() || AI.IsPlayingAttackAnimation ? AISpeed.Run : AISpeed.Walk;

                    if (speed == AISpeed.Run && distanceToTarget < AI.MinAttackRange)
                    {
                        speed = AISpeed.Walk;
                    }

                    if (distanceToTarget <= AI.InterestObstacleSize)
                    {
                        speed = AISpeed.SlowWalk;
                    }

                }

                AI.Speed = speed;
            }
            else if (targetSpeed < 0.1f || distanceToTarget <= AI.MinAttackRange + 0.1f)
            {
                AI.Speed = AISpeed.Stopped;
            }*/

           
            AI.Speed = ShouldMove() ? movingSpeed : AISpeed.Stopped;

        }

        private bool ShouldMove()
        {
            return distanceToTarget > AI.InterestObstacleSize && (targetSpeed > 0.0f || distanceToTarget > AI.MinAttackRange);
        }

        private bool ShouldAttack()
        {
            return timer >= attackRate;
        }
        
    }

}

