using UnityEngine;
using UnityEngine.AI;
using VRShooterKit.AI.SensorSystem;
using VRShooterKit.AI.Tools;
using VRShooterKit.DamageSystem;


namespace VRShooterKit.AI
{
    //the AIStates
    public enum AIStateType
    {
        Patrol,
        Pursuit,
        Attack,
        Idle,
        Die
    }

    //the Behaviour of this AI
    public enum Behaviour
    {               
        Neutral, // neven start a attack, but he can respond if get attacked
        Normal,  //just attack other entities that are diferent form him
        Aggresive //attack all
    }

    public enum AISpeed
    {
        Stopped = 0,
        SlowWalk = 1,
        Walk = 2,
        Run = 3,
        Sprint = 4
    }

    public class AIEntity : FSM
    {
        [SerializeField] private Behaviour behaviour = Behaviour.Normal;
        [SerializeField] [Range( 0.0f, 1.0f )] private float intelligence = 0.25f;
        [SerializeField] private float minAttackRange = 1.0f;
        [SerializeField] private float maxAttackRange = 2.0f;       
        [SerializeField] private AISensor[] sensors = null;
        [SerializeField] private Stimulation thisStimulation = null;
       

        public Collider Collider { get; private set; }
        public PathGenerator PathGenerator { get; private set; }
        public NavMeshAgent NavAgent { get; private set; }
        public NavMeshObstacle Obstacle { get; private set; }
        public Animator Animator { get; private set; }
        public AnimatorHelper AnimatorHelper { get { return animatorHelper; } }
        public PatrolPointsProvider PatrolPoinstProvider { get { return patrolPointsProvider; } }
        public AI_Interest CurrentInterest { get { return currentInterest; } }
        public AISensor[] SensorArray { get { return sensors; } }
        public AISpeed Speed
        {
            set
            {
                Animator.SetInteger( speedHash, (int)value );
            }

            get
            {
                return (AISpeed)Animator.GetInteger(speedHash);
            }
        } 
        public float Intelligence { get { return intelligence; } }
        public float MinAttackRange { get { return minAttackRange; } }
        public float MaxAttackRange { get { return maxAttackRange; } }
        public bool IsPlayingAttackAnimation { get { return animatorHelper.IsPlayingOrTransitionToState( ATTACK_LAYER , ATTACK_STATE_NAME ); } }
        public float InterestObstacleSize
        {
            get
            {
                if (currentInterest == null || currentInterest.stimulation == null || currentInterest.stimulation.NavMeshObstacle == null)
                    return 0.0f;

                Vector3 size = currentInterest.stimulation.NavMeshObstacle.size;

                return (( size.x + size.y + size.z ) / 3.0f) ;
            }
        }
        public float InterestSpeed
        {
            get
            {
                if (currentInterest == null || currentInterest.stimulation == null || currentInterest.stimulation.ISpeed == null)
                    return 0.0f;

                return currentInterest.stimulation.ISpeed.Speed;
            }
        }
        public float RealDistanceToInterest
        {
            get
            {
                if (currentInterest == null)
                    return 0.0f;

                return Vector3.Distance(currentInterest.stimulation.transform.position , transform.position);
            }
        }
        public float LastKonwDistanceToInterest
        {
            get
            {
                if (currentInterest == null)
                    return 0.0f;

                return CalculateLastKnowDistanceToInterest(currentInterest);
            }
        }

      
        private bool useRootRotation = false;
        private bool useRootPosition = false;
        private DamageableManager damageableManager = null;
        private RagdollHelper ragdoll = null;
        private AnimatorHelper animatorHelper = null;
        private PatrolPointsProvider patrolPointsProvider = null;
        private int speedHash = Animator.StringToHash( "Speed" );
        private float dieTime = 10.0f;        

        [SerializeField] protected AI_Interest currentInterest = null;

        #region LAYER_NAMES
        public const int BASE_LAYER = 0;
        public const int ATTACK_LAYER = 1;
        #endregion

        public const string ATTACK_STATE_NAME = "Attack";

        protected virtual void Awake()
        {
            GetAllComponets();
            SetupDamageableLimbs();
            RandomizeAnimator();
            SetAllSensorOwner();

            currentInterest = null;
            patrolPointsProvider = FindObjectOfType<PatrolPointsProvider>();
        }
       

        private void GetAllComponets()
        {
            PathGenerator = GetComponent<PathGenerator>();
            NavAgent = GetComponent<NavMeshAgent>();
            Obstacle = GetComponent<NavMeshObstacle>();
            Animator = GetComponent<Animator>();
            damageableManager = GetComponent<DamageableManager>();
            ragdoll = GetComponent<RagdollHelper>();
            animatorHelper = GetComponent<AnimatorHelper>();
        }

        private void RandomizeAnimator()
        {
            Animator.speed = 1.0f + ( Random.Range( 0.10f, 0.15f ) * ( Random.value > 0.5f ? 1.0f : -1.0f ) );
        }

        private void SetAllSensorOwner()
        {
            for (int n = 0; n < sensors.Length; n++)
            {
                sensors[n].SetOwner( this );
            }
        }

        protected override void Update()
        {
            base.Update();
            UpdateCurrentInterest();
        }

        public void SetCurrentInterest(AI_Interest interest)
        {
            currentInterest = interest;
        }

        protected virtual void UpdateCurrentInterest()
        {           
            if ( !CurrentInterestIsValid() || InterestIsDead(currentInterest) || (CurrentInterestIsValid() && !currentInterest.inRange))
            {
                currentInterest = null;
            }       

            for (int n = 0; n < sensors.Length; n++)
            {
                ProcessSensor(sensors[n]);
            }

        }

        private bool CurrentInterestIsValid()
        {
            return currentInterest != null && currentInterest.stimulation != null;
        }

        private bool InterestIsDead(AI_Interest interest)
        {
            return interest != null && interest.stimulation.VisualType == VisualType.DeadBody;
        }

        private void ProcessSensor(AISensor sensor)
        {
            for (int n = 0; n < sensor.InterestList.Count; n++)
            {
                //we are no interested in things that we can no see or hear
                if (sensor.InterestList[n].inRange)
                {
                    if (ShouldUpdateCurrentInterest( sensor.InterestList[n] ))
                    {
                        currentInterest = sensor.InterestList[n];
                    }
                }
            }
        }

        protected bool ShouldUpdateCurrentInterest(AI_Interest interest)
        {
            //the new interest is no valid
            if (!InterestIsValid( interest ) || InterestIsDead( interest ) || !CanEngage(interest))
            {
                return false;
            }

            //if we dont have a interest
            if (!HaveInterest())
            {
                return true;
            }

            //if the current interest is no the player and the new one is the player
            if (currentInterest.stimulation.VisualType != VisualType.Player && interest.stimulation.VisualType == VisualType.Player)
            {
                return true;
            }

            //if the current interest is the player and the new one no
            if (currentInterest.stimulation.VisualType == VisualType.Player && interest.stimulation.VisualType != VisualType.Player)
            {
                return false;
            }            

            /*
            float distanceToCurrentInterest = CalculateLastKnowSquareDistanceToInterest( currentInterest );
            float distanceToNewInterest = CalculateLastKnowSquareDistanceToInterest( interest );

            float distanceDiff = Mathf.Abs( distanceToCurrentInterest - distanceToNewInterest );

            if (distanceDiff > 0.25f * 0.25f && distanceToNewInterest < distanceToCurrentInterest)
            {               
                return true;
            }*/

            return currentInterest.inRangeAmount + 0.15f < interest.inRangeAmount;

        }

        private bool InterestIsValid(AI_Interest interest)
        {
            return interest != null && interest.stimulation != null;
        }

        private bool HaveInterest()
        {
            return currentInterest != null;
        }

        private float CalculateLastKnowDistanceToInterest(AI_Interest interest)
        {
            return Vector3.Distance( interest.lastKnowPosition, transform.position );
        }

        private float CalculateLastKnowSquareDistanceToInterest(AI_Interest interest)
        {
            return (interest.lastKnowPosition - transform.position).magnitude;
        }

        protected virtual bool CanEngage(AI_Interest interest)
        {
            if (CurrentInterestIsValid() && currentInterest.stimulation == interest.stimulation)
                return true;

            if (interest.isAttackingMe)
            {
                return true;
            }

            if (behaviour == Behaviour.Neutral && CurrentInterestIsValid() && currentInterest.isAttackingMe && interest.stimulation == currentInterest.stimulation)
            {
                return true;
            }
           
            //flee behaivour never start a engage
            if (behaviour == Behaviour.Neutral)
                return false;
            //aggresive attack all
            if (behaviour == Behaviour.Aggresive)
                return true;
            if (behaviour == Behaviour.Normal && interest.stimulation.VisualType != thisStimulation.VisualType)
                return true;

            return false;
        }

        public void RemoveCurrentInterestFromSensors()
        {

            for (int n = 0; n < sensors.Length; n++)
            {
                if (sensors[n].InterestList.Contains( currentInterest ))
                    sensors[n].InterestList.Remove( currentInterest );
            }

            currentInterest = null;
        }
        


        private void SetupDamageableLimbs()
        {

            //set callback for die
            damageableManager.OnDie.AddListener( delegate
            {               
                //disable all nav componets
                NavAgent.enabled = false;
                Animator.enabled = false;

                ragdoll.EnableRagdoll();

                //go to die state
                TryTransitionTo( (int) AIStateType.Die );

                //set a destroy time
                Destroy( gameObject, dieTime );
            } );
        }

        /// <summary>
        /// Set animator root motion control
        /// </summary>
        /// <param name="updatePosition"></param>
        /// <param name="updateRotation"></param>
        public void SetAnimatorRootMotionControl(bool updatePosition, bool updateRotation)
        {
            useRootPosition = updatePosition;
            useRootRotation = updateRotation;
        }

        /// <summary>
        /// set nav agent control
        /// </summary>
        /// <param name="updatePosition"></param>
        /// <param name="updateRotation"></param>
        public void SetNavAgentControl(bool updatePosition, bool updateRotation)
        {
            NavAgent.updatePosition = updatePosition;
            NavAgent.updateRotation = updateRotation;
        }

        //this method is called by Unity like the Update method, 
        //this is where unity apply the movement and rotation from animations
        //we override it in case we want to the NavAgent take control over rotation and movement
        //you can read more here:
        //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAnimatorMove.html
        private void OnAnimatorMove()
        {


            if (useRootPosition && Animator.deltaPosition != Vector3.zero)
                NavAgent.velocity = Animator.deltaPosition / Time.deltaTime;
            if (useRootRotation)
                transform.rotation = Animator.rootRotation;
        }
    }
}
