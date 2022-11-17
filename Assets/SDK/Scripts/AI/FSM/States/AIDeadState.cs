using UnityEngine;

namespace VRShooterKit.AI
{
    //in this state we will just wait for the destroy call
    public class AIDeadState : AIState
    {      

        public override int GetStateIndex()
        {
            return (int) AIStateType.Die;
        }

        public override void OnEnterState()
        {
            AI.NavAgent.enabled = false;
            AI.Obstacle.enabled = false;
        }


        public override int OnUpdate()
        {
            return GetStateIndex();
        }
    }

}

