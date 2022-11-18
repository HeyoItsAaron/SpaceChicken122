using UnityEngine;
using VRShooterKit.WeaponSystem;
using VRShooterKit.AI;

namespace VRShooterKit.DamageSystem
{

    public class DamageInfo
    {
        public float dmg = 0.0f;
        public Vector3 hitDir = Vector3.zero;
        public Vector3 hitPoint = Vector3.zero;
        public float explosionRadius = 0.0f;
        public float upwardsModifier = 0.0f;
        public float hitForce = 0.0f;
        public ForceMode forceMode = ForceMode.Impulse;
        public DamageType damageType = DamageType.Shoot;
        public bool canDismember = false;
        public GameObject sender = null;

        public DamageInfo() { }

        public DamageInfo(ShootInfo info, Projectile projectile)
        {
            dmg = info.dmg; ;
            hitDir = projectile.transform.forward;
            hitPoint = projectile.transform.position;
            hitForce = info.hitForce;
            sender = info.sender;
            canDismember = info.canDismember;
        }

        public DamageInfo(AIAttackInfo attackInfo)
        {
            dmg = attackInfo.dmg;
            hitForce = attackInfo.hitForce;
            forceMode = attackInfo.forceMode;
            damageType = attackInfo.damageType;
            canDismember = attackInfo.canDismember;
        }

        public void ApplyImpact(Rigidbody rb)
        {
            if (rb == null)
                return;

            if (damageType == DamageType.Explosion)
            {
                rb.AddExplosionForce( hitForce, hitPoint, explosionRadius, upwardsModifier, forceMode );
            }
            else if (damageType == DamageType.Shoot)
            {
                rb.AddForceAtPosition( hitForce * hitDir, hitPoint, forceMode );
            }
        }

    }
}

