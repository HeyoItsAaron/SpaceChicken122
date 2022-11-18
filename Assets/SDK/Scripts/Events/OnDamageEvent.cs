using UnityEngine.Events;
using VRShooterKit.DamageSystem;

namespace VRShooterKit.Events
{
    /// <summary>
    /// Event that gets called when a objects get some damage, we extend from UnityEvent<GrabState> so we can see this onn the inspector and add some listeners by hand if we want
    /// </summary>
    [System.Serializable]
    public class OnDamageEvent : UnityEvent<DamageInfo , DamageablePart> { }

}


