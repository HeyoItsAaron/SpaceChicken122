using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace VRShooterKit.AI.Tools
{
    public class PathGenerator : MonoBehaviour
    {
        [SerializeField] private float maxProcessingTime = 1.0f;

        private NavMeshAgent navAgent = null;
        private NavMeshObstacle navObstacle = null;
        private bool isProcessing = false;
        private Coroutine calculatePathCoroutine = null;
        private float timer = 0.0f;

        public bool IsProcessing { get { return isProcessing; } }
        public Action OnPathComplete { get; set; }
        public Action OnPathFailure { get; set; }


        private void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            navObstacle = GetComponent<NavMeshObstacle>();
        }


        /// <summary>
        /// Calculate a new path
        /// </summary>
        /// <param name="position">destination</param>
        /// <param name="onComplete">called when path is complete</param>
        public void CalculatePath(Vector3 position)
        {            
            
            CalculatePath(position , 0.0f);
        }

        private void CalculatePath(Vector3 position, float currentTime)
        {
            StopCurrentPathCalculationProcess();
            calculatePathCoroutine = StartCoroutine( CalculatePathRoutine( position, currentTime) );
        }

        private void StopCurrentPathCalculationProcess()
        {
            if (calculatePathCoroutine != null)
                StopCoroutine( calculatePathCoroutine );
        }

        private IEnumerator CalculatePathRoutine(Vector3 position , float currentTime = 0.0f)
        {           
            isProcessing = true;
            timer = currentTime;

            navAgent.Warp(transform.position);
            yield return new WaitForEndOfFrame();

            //if the obstacle is enable disable it and give a frame so the navmesh can be update and hole can be filled
            if (navObstacle.enabled)
            {
                navObstacle.enabled = false;
                yield return new WaitForEndOfFrame();               
            }

            navAgent.SetDestination( position );
           
            
            yield return new WaitForEndOfFrame();

            while (( !navAgent.hasPath || navAgent.pathStatus != NavMeshPathStatus.PathComplete || navAgent.desiredVelocity == Vector3.zero || navAgent.pathPending ))
            {
                if (!navAgent.enabled || timer > maxProcessingTime)
                {
                    CancelPathGenerationProcess();

                    if(OnPathFailure != null)
                        OnPathFailure();
                    yield break;
                }                

                yield return new WaitForEndOfFrame();

                timer += Time.deltaTime;
            }

            //wait a extra frame
            yield return new WaitForEndOfFrame();

            //something goes wrong generating the path generate a new one
            if (navAgent.enabled && ( !navAgent.hasPath || navAgent.desiredVelocity == Vector3.zero ))
            {
                //clean the path
                navAgent.path = new NavMeshPath();
                CalculatePath(position , timer);
                yield break;
            }

            else if (navAgent.enabled)
            {
                if(OnPathComplete != null)
                    OnPathComplete();
            }

            isProcessing = false;            
        }

        public void CancelPathGenerationProcess()
        {
            StopCurrentPathCalculationProcess();
            isProcessing = false;
        }

       
    }
}

