using UnityEngine;
using VRShooterKit.DamageSystem;

namespace VRShooterKit
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform pockectsAnchorPoint = null;
        [SerializeField] private VR_ScreenFader gameOverScreenFader = null;
        [SerializeField] private DamageableManager damageableManager = null;

        public Transform PocketsAnchorPoint { get { return pockectsAnchorPoint; } }
        public VR_ScreenFader GameOverScreenFader { get { return gameOverScreenFader; } }
        public DamageableManager DamageableManager { get { return damageableManager; } }

    }
}

