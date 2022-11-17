using System.Collections.Generic;
using UnityEngine;

namespace VRShooterKit
{
    public class VR_TagFilter : MonoBehaviour
    {
        [SerializeField] private List<VR_TagsEnum> acceptedTags = null;

        public bool Check(VR_TagsEnum tag)
        {
            return acceptedTags.Contains(tag);
        }
        
    }

}

