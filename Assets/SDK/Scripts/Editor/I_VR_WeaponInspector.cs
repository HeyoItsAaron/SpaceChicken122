    using Platinio.SDK.EditorTools;
    using UnityEngine;
using UnityEditor;
    using UnityEditor.EditorTools;
    using VRShooterKit.WeaponSystem;
using UnityEditor.SceneManagement;

namespace VRShooterKit.EditorCode
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VR_Weapon))]
    public class I_VR_WeaponInspector : Editor
    {

        private SerializedProperty reloadMode = null;
        private SerializedProperty barrelScript = null;
        private SerializedProperty reloadAngle = null;
        private SerializedProperty bulletPrefab = null;
        private SerializedProperty weaponUI = null;
        private SerializedProperty magazineDropZone = null;
        private SerializedProperty muzzleFlash = null;
        private SerializedProperty bulletSpeed = null;
        private SerializedProperty dmg = null;
        private SerializedProperty canDismember = null;
        private SerializedProperty minHitForce = null;
        private SerializedProperty maxHitForce = null;
        private SerializedProperty range = null;
        private SerializedProperty fireButton = null;
        private SerializedProperty shootPoint = null;
        private SerializedProperty shootRate = null;
        private SerializedProperty hitLayer = null;
        private SerializedProperty minRecoilPositionForce = null;
        private SerializedProperty maxRecoilPositionForce = null;
        private SerializedProperty recoilPositionLimit = null;
        private SerializedProperty minRecoilRotationForce = null;
        private SerializedProperty maxRecoilRotationForce = null;
        private SerializedProperty recoilAngleLimit = null;
        private SerializedProperty positionLerpSpeed = null;
        private SerializedProperty rotationLerpSpeed = null;
        private SerializedProperty shootSound = null;
        private SerializedProperty clipSize = null;
        private SerializedProperty reloadTime = null;
        private SerializedProperty useSpread = null;
        private SerializedProperty minSpreadCount = null;
        private SerializedProperty maxSpreadCount = null;
        private SerializedProperty minSpreadAngle = null;
        private SerializedProperty maxSpreadAngle = null;
        private SerializedProperty isAutomatic = null;
        private SerializedProperty maxBulletBounceCount = null;
        private SerializedProperty weaponHammer = null;
        private SerializedProperty shellEjector = null;
        private SerializedProperty m_muzzleFlash = null;
        private SerializedProperty slider = null;
        private SerializedProperty bulletInsertPoint = null;
        private SerializedProperty hitEffect = null;
    

        private const float MAX_HIT_FORCE = 1500.0f;
        private const float MIN_HIT_FORCE = 0.0f;
        private const float MAX_POSITION_FORCE = 1.0f;
        private const float MIN_POSITION_FORCE = 0.0f;
        private const float MAX_ROTATION_FORCE = 90.0f;
        private const float MIN_ROTATION_FORCE = 0.0f;

        private Texture reloadIcon = null;
        private Texture shootIcon = null;
        private Texture damageIcon = null;
        private Texture recoilIcon = null;

        private VR_Weapon weapon = null;
        private VR_WeaponEditorPart editorPart = null;

        private void OnEnable()
        {
            LoadIcons();
            GetSerializeProperties();

            weapon = (VR_Weapon)target;
            editorPart = weapon.EditorPart;

        }

        private void LoadIcons()
        {
            reloadIcon = PlatinioEditorGUILayout.LoadIcon("reload");
            shootIcon = PlatinioEditorGUILayout.LoadIcon("target");
            damageIcon = PlatinioEditorGUILayout.LoadIcon("explosion");
            recoilIcon = PlatinioEditorGUILayout.LoadIcon("weapon");
        }

        private void GetSerializeProperties()
        {
            reloadMode = serializedObject.FindProperty("reloadMode");
            barrelScript = serializedObject.FindProperty("barrelScript");
            reloadAngle = serializedObject.FindProperty("reloadAngle");
            bulletPrefab = serializedObject.FindProperty("bulletPrefab");
            weaponUI = serializedObject.FindProperty("weaponUI");
            magazineDropZone = serializedObject.FindProperty("magazineDropZone");
            muzzleFlash = serializedObject.FindProperty("m_muzzleFlash");
            bulletSpeed = serializedObject.FindProperty("bulletSpeed");
            dmg = serializedObject.FindProperty("dmg");
            canDismember = serializedObject.FindProperty("canDismember");
            minHitForce = serializedObject.FindProperty("minHitForce");
            maxHitForce = serializedObject.FindProperty("maxHitForce");
            range = serializedObject.FindProperty("range");
            fireButton = serializedObject.FindProperty("fireButton");
            shootPoint = serializedObject.FindProperty("shootPoint");
            hitLayer = serializedObject.FindProperty("hitLayer");
            shootRate = serializedObject.FindProperty("shootRate");
            minRecoilPositionForce = serializedObject.FindProperty("minRecoilPositionForce");
            maxRecoilPositionForce = serializedObject.FindProperty("maxRecoilPositionForce");
            recoilAngleLimit = serializedObject.FindProperty("recoilAngleLimit");
            minRecoilRotationForce = serializedObject.FindProperty("minRecoilRotationForce");
            maxRecoilRotationForce = serializedObject.FindProperty("maxRecoilRotationForce");
            recoilPositionLimit = serializedObject.FindProperty("recoilPositionLimit");
            positionLerpSpeed = serializedObject.FindProperty("positionLerpSpeed");
            rotationLerpSpeed = serializedObject.FindProperty("rotationLerpSpeed");
            shootSound = serializedObject.FindProperty("shootSound");
            clipSize = serializedObject.FindProperty("clipSize");
            reloadTime = serializedObject.FindProperty("reloadTime");
            useSpread = serializedObject.FindProperty("useSpread");
            minSpreadCount = serializedObject.FindProperty("minSpreadCount");
            maxSpreadCount = serializedObject.FindProperty("maxSpreadCount");
            minSpreadAngle = serializedObject.FindProperty("minSpreadAngle");
            maxSpreadAngle = serializedObject.FindProperty("maxSpreadAngle");
            isAutomatic = serializedObject.FindProperty("isAutomatic");
            maxBulletBounceCount = serializedObject.FindProperty("maxBulletBounceCount");
            weaponHammer = serializedObject.FindProperty("weaponHammer");
            slider = serializedObject.FindProperty("slider");
            bulletInsertPoint = serializedObject.FindProperty("bulletInsertPoint");
            hitEffect = serializedObject.FindProperty("hitEffect");
            m_muzzleFlash = serializedObject.FindProperty("m_muzzleFlash");
            shellEjector = serializedObject.FindProperty("shellEjector");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(10);

            editorPart.selectedMenu = (WeaponSelectionMenu)PlatinioEditorGUILayout.DrawGridButtons((int)editorPart.selectedMenu, 2,
                new GUIContent(" Reload", reloadIcon),
                new GUIContent(" Shoot", shootIcon),
                new GUIContent(" Damage", damageIcon),
                new GUIContent(" Recoil", recoilIcon)
                );

            EditorGUILayout.Space(30);

            if (editorPart.selectedMenu == WeaponSelectionMenu.Reload)
            {
                DrawReloadSettings();
            }
            else if (editorPart.selectedMenu == WeaponSelectionMenu.Shoot)
            {
                DrawShootSettings();
            }
            else if (editorPart.selectedMenu == WeaponSelectionMenu.Damage)
            {
                DrawDamageSettings();
            }
            else if (editorPart.selectedMenu == WeaponSelectionMenu.Recoil)
            {
                DrawRecoilSettings();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawReloadSettings()
        {
            DrawReloadSettingsHeader();
            EditorGUILayout.Space(30);
            EditorGUILayout.BeginVertical("Box");
            DrawReloadSettingsBody();
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawReloadSettingsHeader()
        {
            PlatinioEditorGUILayout.DrawTooltipBox(reloadIcon, "Reload Settings",
                "Here you can configure your desire reload method, you have plenty of options, and small description for each option.");
        }

        protected virtual void DrawReloadSettingsBody()
        {
            GUIContent content = new GUIContent("Reload Mode", "Select your reload mode");
            reloadMode.enumValueIndex = (int)(ReloadMode)EditorGUILayout.EnumPopup(content, (ReloadMode)reloadMode.enumValueIndex);

            DrawReloadModeInfo();

            if (reloadMode.enumValueIndex == (int)ReloadMode.Realistic)
            {
                content = new GUIContent("Magazine Dropzone", "The component use to insert and remove magazines");
                magazineDropZone.objectReferenceValue = EditorGUILayout.ObjectField(content, magazineDropZone.objectReferenceValue, typeof(VR_DropZone), true);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.UI)
            {
                PlatinioEditorGUILayout.PropertyField(clipSize);
                PlatinioEditorGUILayout.PropertyField(reloadTime);


                if (clipSize.intValue <= 0)
                    clipSize.intValue = 1;

                if (reloadTime.floatValue < 0.0f)
                    clipSize.floatValue = 0.001f;
            }
            else if ((ReloadMode)reloadMode.enumValueIndex == ReloadMode.Physics)
            {
                PlatinioEditorGUILayout.PropertyField(barrelScript);
                PlatinioEditorGUILayout.PropertyField(reloadAngle);
            }
            if ((ReloadMode)reloadMode.enumValueIndex == ReloadMode.PumpActionInfiniteBullets || (ReloadMode)reloadMode.enumValueIndex == ReloadMode.PumpActionRealistic)
            {
                PlatinioEditorGUILayout.PropertyField(slider);
            }

            if ((ReloadMode)reloadMode.enumValueIndex == ReloadMode.PumpActionRealistic)
            {
                PlatinioEditorGUILayout.PropertyField(clipSize);
                PlatinioEditorGUILayout.PropertyField(bulletInsertPoint);
            }

            if ((ReloadMode)reloadMode.enumValueIndex == ReloadMode.Launcher)
            {
                PlatinioEditorGUILayout.PropertyField(bulletInsertPoint);
            }

        }

        private void DrawReloadModeInfo()
        {
            if (reloadMode.enumValueIndex == (int)ReloadMode.Physics)
            {
                EditorGUILayout.HelpBox("<b>Physics:</b> in physics reload mode you are require to reload using a gesture, rotate your hand fast " +
                "in some angle amount, an example of this can be the revolver in the example scene.", MessageType.Info);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.Realistic)
            {
                EditorGUILayout.HelpBox("<b>Realistic:</b> in realistic reload mode you are require to use magazines and once a magazine goes empty" +
                    " you will need to remove it and insert a new one, this make use of <b>VR_Dropzones</b> in order to insert and remove magazines.", MessageType.Info);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.UI)
            {
                EditorGUILayout.HelpBox("<b>UI:</b> in UI reload mode you have infinite bullets, but once a magazine gets empty, a small bar will be" +
                    " show indicating reload time, this is perfect if you want to stop your players from shooting like crazies and at the same time" +
                    " have a simple reload mechanic.", MessageType.Info);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.InfiniteBullets)
            {
                EditorGUILayout.HelpBox("<b>Inifinite Bullets:</b> Just like that infinite bullets your players can shoot as much as they want.", MessageType.Info);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.PumpActionInfiniteBullets)
            {
                EditorGUILayout.HelpBox("<b>Pump Action Inifinite Bullets:</b> a weapon with a pumping action is that weapon where the " +
                    "handguard has to be slid back and forth to eject the fired cartridge and insert a new cartridge into its chamber, but this version" +
                    "is a simplified version you don't need to insert new cartridges players have infinite bullets, but they are require to slid the" +
                    "handguard between shoots, uses " +
                    "a <b>VR_Slider</b> component in order to simulate the handguard", MessageType.Info);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.PumpActionRealistic)
            {
                EditorGUILayout.HelpBox("<b>Pump Action Realistics:</b> a weapon with a pumping action is that weapon where the " +
                    "handguard has to be slid back and forth to eject the fired cartridge and insert a new cartridge into its chamber, uses " +
                    "a <b>VR_Slider</b> component in order to simulate the handguard", MessageType.Info);
            }
            else if (reloadMode.enumValueIndex == (int)ReloadMode.Launcher)
            {
                EditorGUILayout.HelpBox("<b>Launcher:</b> A launcher just require to insert a gameobject containing the component <b>Bullet</b>" +
                    " in the <b>VR_Dropzone</b> and that object will be launched, an example can be the rocket launcher from teh example scene", MessageType.Info);
            }
        }

        private void DrawShootSettings()
        {
            DrawShootSettingsHeader();
            EditorGUILayout.Space(30);            
            DrawShootSettingsBody();
            
        }

        protected virtual void DrawShootSettingsHeader()
        {
            PlatinioEditorGUILayout.DrawTooltipBox(shootIcon, "Shoot Settings",
                "Here you can configure your all your shooting settings.");
        }

        protected virtual void DrawShootSettingsBody()
        {
            GUIContent  content = new GUIContent("Basic", "");            
            PlatinioEditorGUILayout.FoldoutInspector(content, ref editorPart.foldoutBasicShootSettings, 1, DrawBasicShootSettings);

            EditorGUILayout.Space(10);

            PlatinioEditorGUILayout.PropertyField(m_muzzleFlash);

            EditorGUILayout.Space(10);

            content = new GUIContent("Optional", "");            
            PlatinioEditorGUILayout.FoldoutInspector(content, ref editorPart.foldoutOptionalShootSettings, 1, DrawOptionalShootSettings);

            EditorGUILayout.Space(10);
        }

        private void DrawBasicShootSettings()
        {
            EditorGUILayout.BeginVertical("Box");
            PlatinioEditorGUILayout.PropertyField(fireButton);
            GUIContent content = new GUIContent("Shoot Point", "The point where the bullet will be spanned, and the forward " +
                "of this point defines the direction");
            shootPoint.objectReferenceValue = EditorGUILayout.ObjectField(content, shootPoint.objectReferenceValue, typeof(Transform), true);

            if ((ReloadMode)reloadMode.enumValueIndex != ReloadMode.Launcher)
                PlatinioEditorGUILayout.PropertyField(bulletPrefab);

            PlatinioEditorGUILayout.PropertyField(shootRate);

            content = new GUIContent("Is Automatic? ", "is this weapon automatic?");
            isAutomatic.boolValue = PlatinioEditorGUILayout.DrawBoolEnum(content, isAutomatic.boolValue);

            PlatinioEditorGUILayout.PropertyField(bulletSpeed);

            content = new GUIContent("Max Bounce ", "Bullets can bounce on surfaces, by a limited amount of times, you can define here that amount" +
                " or just 0 if you don't want your bullets to bounce at all, if you want your bullets to bounce on some surfaces you can use the" +
                " SurfaceDetails component in order to define a surface properties like if that surface allow bounce");
            maxBulletBounceCount.intValue = EditorGUILayout.IntField(content , maxBulletBounceCount.intValue);

            PlatinioEditorGUILayout.PropertyField(range);
            
            PlatinioEditorGUILayout.DrawTittle( "Hit Force" );
            PlatinioEditorGUILayout.MinMaxFloatSlider( minHitForce, maxHitForce, MIN_HIT_FORCE, MAX_HIT_FORCE );
            
            PlatinioEditorGUILayout.PropertyField(shootSound);            
            

            EditorGUILayout.HelpBox("Weapon RPM: " + 60.0f / shootRate.floatValue, MessageType.Info);
            EditorGUILayout.EndVertical();
        }
        

        private void DrawOptionalShootSettings()
        {
            EditorGUILayout.BeginVertical("Box");
            GUIContent content = new GUIContent("Hit Effect", "(Optional) A GameObject spanned when the bullet hits something, can be maybe an explosion effect?");
            hitEffect.objectReferenceValue = EditorGUILayout.ObjectField(content, hitEffect.objectReferenceValue, typeof(GameObject), true);

            content = new GUIContent("Weapon Hammer", "(Optional) Hammer component of the gun, useful for creating revolver weapons");
            weaponHammer.objectReferenceValue = EditorGUILayout.ObjectField(content, weaponHammer.objectReferenceValue, typeof(WeaponHammer), true);

            content = new GUIContent("Shell Ejector", "(Optional) Shell Ejector component of the gun, you can eject shells while shooting");
            shellEjector.objectReferenceValue = EditorGUILayout.ObjectField(content, shellEjector.objectReferenceValue, typeof(ShellEjector), true);
            
            if ((ReloadMode) reloadMode.enumValueIndex != ReloadMode.InfiniteBullets )
            {
                PlatinioEditorGUILayout.PropertyField( weaponUI );
            }
            
            EditorGUILayout.EndVertical();
        }

        private void DrawDamageSettings()
        {
            DrawDamageSettingsHeader();
            EditorGUILayout.Space(30);            
            DrawDamageSettingsBody();            
        }

        protected virtual void DrawDamageSettingsHeader()
        {
            PlatinioEditorGUILayout.DrawTooltipBox(damageIcon, "Damage Settings",
                "Here you can configure the weapon damage settings, hit force and spread.");
        }

        protected virtual void DrawDamageSettingsBody()
        {
            EditorGUILayout.BeginVertical("Box");
            PlatinioEditorGUILayout.PropertyField(dmg , "Damage");

            GUIContent content = new GUIContent("Can Dismember?", "Can this weapon dismember enemies, (only if they have a properly setup " +
                "dismember system)");
            canDismember.boolValue = PlatinioEditorGUILayout.DrawBoolEnum(content, canDismember.boolValue);

            PlatinioEditorGUILayout.PropertyField(hitLayer);
            EditorGUILayout.EndVertical();
            content = new GUIContent("Spread", "");
            PlatinioEditorGUILayout.FoldoutInspector(content, ref editorPart.foldoutSpreadDamageSettings, 0, DrawSpreadDamageSettings);

        }

        private void DrawSpreadDamageSettings()
        {
            EditorGUILayout.BeginVertical("Box");
            GUIContent content = new GUIContent("Use Spread?", "The bullets of weapons like shotguns explode in the air creating small bullet fragments traveling " +
                "in random directions, use this if you wanna imitate this behaviour");

            useSpread.boolValue = PlatinioEditorGUILayout.DrawBoolEnum(content, useSpread.boolValue);

            if (useSpread.boolValue)
            {
                PlatinioEditorGUILayout.PropertyField(minSpreadCount);
                PlatinioEditorGUILayout.PropertyField(maxSpreadCount);

                PlatinioEditorGUILayout.DrawTittle("Spread Angle");
                PlatinioEditorGUILayout.MinMaxFloatSlider(minSpreadAngle , maxSpreadAngle , 1 , 180.0f);
                               
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawRecoilSettings()
        {
            DrawRecoilSettingsHeader();
            EditorGUILayout.Space(30);
            DrawRecoilSettingsBody();
        }

        protected virtual void DrawRecoilSettingsHeader()
        {
            PlatinioEditorGUILayout.DrawTooltipBox(recoilIcon, "Recoil Settings",
                "Here you can configure weapon recoil settings");
        }

        protected virtual void DrawRecoilSettingsBody()
        {
            EditorGUILayout.BeginVertical("Box");
            PlatinioEditorGUILayout.DrawTittle("Recoil Position");
            PlatinioEditorGUILayout.MinMaxFloatSlider(minRecoilPositionForce, maxRecoilPositionForce, MIN_POSITION_FORCE, MAX_POSITION_FORCE);
            PlatinioEditorGUILayout.PropertyField(recoilPositionLimit);

            PlatinioEditorGUILayout.DrawTittle("Recoil Rotation");
            PlatinioEditorGUILayout.MinMaxFloatSlider(minRecoilRotationForce, maxRecoilRotationForce, MIN_ROTATION_FORCE, MAX_ROTATION_FORCE);
            PlatinioEditorGUILayout.PropertyField(recoilAngleLimit);
                       
            

            PlatinioEditorGUILayout.DrawTittle("Recoil Lerp");
            PlatinioEditorGUILayout.PropertyField(positionLerpSpeed);
            PlatinioEditorGUILayout.PropertyField(rotationLerpSpeed);
            EditorGUILayout.HelpBox("Position and Rotation Lerp Speed define the speed use by this weapon after a recoil in order to comeback," +
                                    " to his starting position and rotation", MessageType.Info);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }


    }

}

