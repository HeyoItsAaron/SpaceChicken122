using UnityEngine;
using VRShooterKit.DamageSystem;

namespace VRShooterKit
{
    public class DamageScreenEffect : MonoBehaviour
    {
        [SerializeField] private DamageableManager damageableManager = null;        
        [SerializeField] private VR_ScreenFader screenFader = null;
        [SerializeField] private AnimationCurve animationCurve = null;
        [SerializeField] private float maxAlphaValue = 0.75f;

        private void Awake()
        {
            damageableManager.OnHPChangeEvent.AddListener( UpdateAlphaValue );
        }

        protected void Start()
        {
            UpdateAlphaValue( damageableManager.HP );
        }

        private void UpdateAlphaValue(float v)
        {
            float alpha = Mathf.Min( maxAlphaValue, maxAlphaValue * animationCurve.Evaluate( Mathf.Abs( ( v / damageableManager.MaxHP ) - 1.0f ) ) );
            screenFader.SetMaterialAlpha( alpha );
        }
    }
}

