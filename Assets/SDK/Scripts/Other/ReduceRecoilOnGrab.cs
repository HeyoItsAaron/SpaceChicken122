using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    public class ReduceRecoilOnGrab : MonoBehaviour
    {
        [SerializeField] private VR_Weapon target = null;
        [SerializeField] private float positionRecoilReduce = 0.5f;
        [SerializeField] private float rotationRecoildReduce = 0.5f;

        private void Awake()
        {
            GetComponent<VR_Grabbable>().OnGrabStateChange.AddListener( OnGrabStateChange );
        }

        private void OnGrabStateChange(GrabState grabState)
        {
            if (grabState == GrabState.Grab)
            {
                target.SetRecoilModifierValue(positionRecoilReduce, rotationRecoildReduce);
            }
            else
            {
                target.SetRecoilModifierValue(1.0f, 1.0f);
            }
        }


    }

}

