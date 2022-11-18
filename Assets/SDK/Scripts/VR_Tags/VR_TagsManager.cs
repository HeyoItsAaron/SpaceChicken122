using UnityEngine;
using System.Collections.Generic;

namespace VRShooterKit
{
    //the real implementation is on the editor code, in the I_VR_TagsManager.cs
    [CreateAssetMenu( fileName = "TagManager", menuName = "VRShooterKit/Create Tag Manager" )]
    public class VR_TagsManager : ScriptableObject
    {
        public List<string> TagList = null;
    }

}

