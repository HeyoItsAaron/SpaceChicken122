using UnityEngine;
using UnityEditor;
using VRShooterKit.DamageSystem;

namespace VRShooterKit.EditorCode
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof( DamageableManager ) )]
    public class I_DamagableManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //create a button in the inspector for automatically 
            //assign damageable limbs to all colliders in a character
            if (GUILayout.Button( "Setup DamageableLimbs" ))
            {
                ( (DamageableManager) target ).SetupDamageableLimbs();
                EditorUtility.SetDirty( target );
            }
        }
    }
}

