using UnityEngine.Events;
using VRShooterKit.DamageSystem.Dismember;

namespace VRShooterKit.Events
{   
    [System.Serializable]
    public class OnDismemberEvent : UnityEvent<DismemberPart> { }

}


