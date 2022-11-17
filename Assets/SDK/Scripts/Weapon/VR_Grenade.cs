using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRShooterKit.DamageSystem;

namespace VRShooterKit.WeaponSystem
{
    public class VR_Grenade : MonoBehaviour
    {
        #region INSPECTOR        
        [SerializeField] private float explosionTime = 15.0f;       
        [SerializeField] private Text timeLabel = null;
        [SerializeField] protected Explosion explosionPrefab = null;
        #endregion

        protected bool explode = false;
        private float timer = 0.0f;

        private void OnEnable()
        {
            if (timeLabel != null)
                timeLabel.gameObject.SetActive( true );
        }

        private void OnDisable()
        {
            if (timeLabel != null)
                timeLabel.gameObject.SetActive( false );
        }

        private void Update()
        {
            if (explode)
                return;

            timer += Time.deltaTime;

            UpdateUI();

            if (timer >= explosionTime)
                Explode();

        }

        public virtual void Explode()
        {
            if (explosionPrefab != null)
                Instantiate( explosionPrefab, transform.position, Quaternion.identity );

            explode = true;

            Destroy(gameObject);

        }

        private void UpdateUI()
        {
            if (timeLabel != null)
                timeLabel.text = Mathf.CeilToInt( explosionTime - timer ).ToString();
        }

    }

}

