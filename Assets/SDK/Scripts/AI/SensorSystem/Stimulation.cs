using UnityEngine;
using VRShooterKit.DamageSystem;
using UnityEngine.AI;

namespace VRShooterKit.AI.SensorSystem
{
    public enum VisualType
    {
        None,
        Zombie,
        Player,
        DeadBody,
        Unknow
    }

    public enum AudioType
    {
        None,
        Enviroment,
        Explosion,
        Breathing,
        Unknow
    }

    public class Stimulation : MonoBehaviour
    {

        [SerializeField] private VisualType visualType = VisualType.None;
        [SerializeField] private AudioType audioType = AudioType.None;
        [SerializeField] private Collider thisCollider = null;
        [SerializeField] private DamageableManager damageableManager = null;
        [SerializeField] private Damageable damageable = null;
        [SerializeField] private NavMeshObstacle navObstacle = null;       
        


        private float range = 0.0f;
        private NavMeshObstacle navMeshObstacle = null;



        public VisualType VisualType { get { return visualType; } }
        public AudioType AudioType { get { return audioType; } }
        public Damageable Damageable { get { return damageable; } }
        public DamageableManager DamageableManager { get { return damageableManager; } }
        public ISpeed ISpeed { get; private set; }
        public Collider Collider { get { return thisCollider; } }
        public float Range { get { return range; } }
        public NavMeshObstacle NavMeshObstacle { get { return navMeshObstacle; } }



        protected virtual void Awake()
        {

            //get references            
            damageable = GetComponent<Damageable>();
            damageableManager = GetComponent<DamageableManager>();
            thisCollider = GetComponent<Collider>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            ISpeed = navObstacle.GetComponent<ISpeed>();
            
            
            if (DamageableManager != null)
            {
                DamageableManager.OnDie.AddListener( delegate
                 {
                     visualType = VisualType.DeadBody;
                 } ); 
            }

            range = ( Collider is SphereCollider ? ( (SphereCollider) Collider ).radius : 0.0f );

        }
    }

}

