using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    public class ParticleSystemMuzzleFlash : MuzzleFlash
    {
        public override void Fire()
        {
            base.Fire();
            ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

            for (int n = 0; n < particles.Length; n++)
            {
                particles[n].Play();
            }
        }
    }
}



