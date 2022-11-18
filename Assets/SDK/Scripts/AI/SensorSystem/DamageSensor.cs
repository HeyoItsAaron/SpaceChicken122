using UnityEngine;
using VRShooterKit.DamageSystem;

namespace VRShooterKit.AI.SensorSystem
{
    public class DamageSensor : AISensor
    {
        [SerializeField] private DamageableManager damageableManager = null;

        protected override void Awake()
        {
            base.Awake();

            if(damageableManager == null)
            {
                damageableManager = GetComponent<DamageableManager>();            
            }

            damageableManager.OnDamage.AddListener(OnDamage);

        }

        protected override void OnSensorUpdate()
        {
            
            if (owner.CurrentInterest != null)
            {
                AI_Interest interest = GetInterestIfExits(owner.CurrentInterest.stimulation);

                if(interest != null)
                    InterestList.Remove(owner.CurrentInterest);
            }
        }

        private void OnDamage(DamageInfo info , DamageablePart damageablePart)
        {
            Stimulation stim = null;

            if (info.sender != null)
            {
                stim = info.sender.GetComponentInChildren<Stimulation>();
            }

            if (owner != null && stim != null && (owner.CurrentInterest == null || owner.CurrentInterest.stimulation == null || owner.CurrentInterest.stimulation != stim))
            {

                AI_Interest interest = GetInterestIfExits(stim);

                if (interest != null)
                {
                    interest.lastKnowPosition = info.sender.transform.position;
                }

                else
                {
                    interest = new AI_Interest( stim, 0.35f );
                    interest.lastKnowPosition = info.sender.transform.position;
                    interest.inRange = true;
                    interest.isAttackingMe = true;

                    InterestList.Add( interest );
                }                
            }           

        }

        private bool StimulationExitsInInterestList(Stimulation stim)
        {
            for (int n = 0 ; n < InterestList.Count; n++)
            {
                if (InterestList[n].stimulation == stim)
                {
                    return true;
                }
            }

            return false;
        }

        private AI_Interest GetInterestIfExits(Stimulation stim)
        {
            for (int n = 0; n < InterestList.Count; n++)
            {
                if (InterestList[n].stimulation == stim)
                {
                    return InterestList[n];
                }
            }

            return null;
        }

    }
}

