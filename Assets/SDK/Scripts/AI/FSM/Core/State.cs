using UnityEngine;

namespace VRShooterKit.AI
{
    /// <summary>
    /// Basic class for all FSM States
    /// </summary>
    public abstract class State : MonoBehaviour
    {       
        protected FSM fsm = null;

        //abstrac methods
        public abstract int GetStateIndex();
        public abstract int OnUpdate();
                
        public virtual void SetStateMachine(FSM fsm)
        {
            this.fsm = fsm;
        }

        //default handlers
        public virtual void OnEnterState() { }
        public virtual void OnExitState() { }
        
    }

}

