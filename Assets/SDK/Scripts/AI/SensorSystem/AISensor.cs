using System.Collections.Generic;
using UnityEngine;

namespace VRShooterKit.AI.SensorSystem
{
    public abstract class AISensor : MonoBehaviour
    {
        protected AIEntity owner = null;

        public List<AI_Interest> InterestList = null;
        //public List<GameObject> InterestListGO = new List<GameObject>();

        protected virtual void Awake()
        {
            InterestList = new List<AI_Interest>();
        }

        private void Update()
        {
            RemoveEmpty();
            OnSensorUpdate();

        }

        protected abstract void OnSensorUpdate();


        /// <summary>
        /// Set the AI owner for this sensor
        /// </summary>
        /// <param name="fsm"></param>
        public virtual void SetOwner(AIEntity owner)
        {
            this.owner = owner;
        }

        private void RemoveEmpty()
        {
            for (int n = 0; n < InterestList.Count; n++)
            {
                if (InterestList[n] == null || InterestList[n].stimulation == null)
                {
                    InterestList.RemoveAt( n );
                    n--;
                }
            }
        }
    }
}

