using UnityEngine;
using VRShooterKit.DamageSystem;
using System.Collections.Generic;

namespace VRShooterKit.AI
{
    public class AIAttackHandler : MonoBehaviour
    {
        [SerializeField] private Transform forwardAttackDirection = null;
        [SerializeField] private LayerMask hitLayer;

        private DamageableManagerAI damageableManager = null;

        private Ray AttackRay { get { return new Ray( forwardAttackDirection.position , forwardAttackDirection.forward ); } }

        private void Awake()
        {
            
            damageableManager = GetComponent<DamageableManagerAI>();
        }

        public void DoDamageAnimationEvent(AnimationEvent animationEvent)
        {           
            AIAttackInfo attackInfo = animationEvent.objectReferenceParameter as AIAttackInfo;
            DamageInfo damageInfo = new DamageInfo(attackInfo);
            damageInfo.sender = gameObject;

            List<Damageable> damageableList = AttackRaycast( attackInfo);
            SendDamageEvent(damageableList , damageInfo);
            
        }


        private List<Damageable> AttackRaycast(AIAttackInfo attackInfo)
        {
            RaycastHit[] hitArray = Physics.RaycastAll( AttackRay, attackInfo.range , hitLayer.value , QueryTriggerInteraction.Ignore );

            if (attackInfo.multipleTargets)
            {
                return HandleMultipleTargetsRaycast(hitArray);
            }
            else
            {
                return HandleSingleTargetRaycast(hitArray);
            }

        }

        private List<Damageable> HandleMultipleTargetsRaycast(RaycastHit[] hitArray)
        {
            List<Damageable> damageableList = new List<Damageable>();

            for (int n = 0; n < hitArray.Length; n++)
            {
                Damageable damageable = null;

                damageable = hitArray[n].collider.GetComponent<Damageable>();

                if (damageable != null && !ThisDamageableIsMine(damageable))
                {
                    damageableList.Add(damageable);
                }

            }

            return damageableList;
        }

        private List<Damageable> HandleSingleTargetRaycast(RaycastHit[] hitArray)
        {
            List<Damageable> damageableList = new List<Damageable>();

            Damageable closerDamageable = null;
            float minDistance = float.MaxValue;

            for (int n = 0; n < hitArray.Length; n++)
            {

                Damageable damageable = null;

                damageable = hitArray[n].collider.GetComponent<Damageable>();

               
                if (damageable != null && !ThisDamageableIsMine(damageable) && hitArray[n].distance < minDistance)
                {
                    closerDamageable = damageable;
                    minDistance = hitArray[n].distance;
                }

            }

            if(closerDamageable != null)
            {
                damageableList.Add(closerDamageable);
            }

            return damageableList;
        }

        private void SendDamageEvent(List<Damageable> damageableList , DamageInfo damageInfo)
        {
            for (int n = 0; n < damageableList.Count; n++)
            {

                damageableList[n].DoDamage(damageInfo);
            }
        }

        private bool ThisDamageableIsMine(Damageable damageable)
        {
            if (damageableManager == null)
                return false;

            if (!( damageable is DamageablePart ))
                return false;

            DamageablePart damageablePart = damageable as DamageablePart;

            return damageablePart.Owner == damageableManager;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(AttackRay.origin , AttackRay.origin + ( AttackRay.direction * 2.0f ));
        }
        

    }
}


