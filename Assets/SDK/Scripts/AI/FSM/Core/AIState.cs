using UnityEngine;

namespace VRShooterKit.AI
{
    public abstract class AIState : State
    {       
        [SerializeField] protected bool useRootMotionRotation = false;
        [SerializeField] protected bool useRootMotionPosition = false;
        [SerializeField] protected bool updateNavAgentPosition = false;
        [SerializeField] protected bool updateNavAgentRotation = false;
               

        protected AIEntity AI { get { return (AIEntity) fsm; } }           
    }

}

