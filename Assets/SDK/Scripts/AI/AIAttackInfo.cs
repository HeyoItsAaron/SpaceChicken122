using UnityEngine;
using VRShooterKit.DamageSystem;

namespace VRShooterKit.AI
{
    [CreateAssetMenu( fileName = "SimpleAttackInfo", menuName = "VRShooterKit/Attack Info/Simple Attack Info" )]
    public class AIAttackInfo : ScriptableObject
    {       
        public float dmg = 10.0f;
        public float range = 1.5f;
        public float hitForce = 10.0f;
        public bool multipleTargets = false;
        public bool canDismember = false;
        public ForceMode forceMode = ForceMode.Force;
        public DamageType damageType = DamageType.Physical;       
    }
}

