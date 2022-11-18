using System.Collections.Generic;
using UnityEngine;

namespace VRShooterKit.AI
{
    //this is just a handy implementation of a Finite State Machine
    public class FSM : MonoBehaviour
    {
        [SerializeField] private AIStateType initialState = AIStateType.Idle;

        private Dictionary<int, State> states = new Dictionary<int, State>();
        protected State currentState = null;
        private State enterState = null;       

        private void Start()
        {
            //get all the AI states
            State[] statesArray = GetComponents<State>();

            //add states to the dictionary
            for (int n = 0; n < statesArray.Length; n++)
            {
                //if the dictionary is no null and no exist already in the list add it to the dictionary
                if (statesArray[n] != null && !states.ContainsKey( statesArray[n].GetStateIndex() ))
                {
                    states[statesArray[n].GetStateIndex()] = statesArray[n];
                    statesArray[n].SetStateMachine( this );
                }
            }

            State initialState = GetInitialState();

            if (initialState != null)
            {
                initialState.OnEnterState();
                currentState = initialState;
                enterState = initialState;
            }
            else
            {
                Debug.LogError("Can no find Initial state type " + initialState);
            }

            /*
            //call the initial state,in our case the Idle
            for (int n = 0; n < statesArray.Length; n++)
            {
                if (statesArray[n].IsInitialState)
                {
                    statesArray[n].OnEnterState();
                    currentState = statesArray[n];
                    enterState = currentState;
                }

            }*/

            if (currentState == null)
            {
                Debug.LogError( "Error initial state not found!" );
            }

        }

        private State GetInitialState()
        {
            State state = null;

            if (states.TryGetValue( (int) initialState, out state ))
            {               
                return state;
            }

            return null;
        }


        protected virtual void Update()
        {
            if ( !HasValidState() )
                return;

            //update the current state
            int newStateIndex = currentState.OnUpdate();

            //check if want to transition to other state
            if (newStateIndex != currentState.GetStateIndex())
            {
                bool success = TryTransitionTo( newStateIndex );

                if (!success)
                {
                    Debug.LogWarning("State " + (AIStateType) newStateIndex + " can no be found! , transiting to the default state" );
                    success = TryTransitionTo( enterState.GetStateIndex() );

                    if (!success)
                    {
                        Debug.LogError("Fail transition to default state!");
                    }
                }          

            }
        }

        private bool HasValidState()
        {
            return currentState != null;
        }

        protected bool TryTransitionTo(int stateIndex)
        {
            State newState = null;

            if (states.TryGetValue( stateIndex, out newState ))
            {
                currentState.OnExitState();
                newState.OnEnterState();
                currentState = newState;

                return true;
            }

            return false;
        }


    }

}

