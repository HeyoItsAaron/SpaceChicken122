using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VRShooterKit.WeaponSystem
{
   
    public enum ReloadMode
    {
        Physics,
        Realistic,
        UI,
        InfiniteBullets,
        PumpActionInfiniteBullets,
        PumpActionRealistic,
        Launcher
    }

    //this scripta controlls all the fire weapons
    public class VR_Weapon : MonoBehaviour
    {
        #region INSPECTOR        
        [SerializeField] private ReloadMode reloadMode = ReloadMode.InfiniteBullets;
        [SerializeField] private BarrelScript barrelScript = null;
        [SerializeField] private float reloadAngle = 40.0f;
        [SerializeField] private Bullet bulletPrefab = null;
        [SerializeField] private WeaponUI weaponUI = null;
        [SerializeField] private WeaponHammer weaponHammer = null;
        [SerializeField] private ShellEjector shellEjector = null;
        [SerializeField] private VR_DropZone magazineDropZone = null;
        [SerializeField] protected GameObject hitEffect = null;
        [SerializeField] protected float bulletSpeed = 200.0f;
        [SerializeField] private int clipSize = 1;
        [SerializeField] private float reloadTime = 1.0f;
        [SerializeField] private bool isAutomatic = false;
        [SerializeField] protected float dmg = 10.0f;
        [SerializeField] protected bool canDismember = false;
        [SerializeField] protected float minHitForce = 0.0f;
        [SerializeField] protected float maxHitForce = 0.0f;
        [SerializeField] protected float range = 250.0f;
        [SerializeField] private VR_InputButton fireButton = VR_InputButton.Trigger;
        [SerializeField] protected Transform shootPoint = null;
        [SerializeField] private float shootRate = 0.02f;
        [SerializeField] private float minRecoilPositionForce = 0.0f;
        [SerializeField] private float maxRecoilPositionForce = 0.0f;
        [SerializeField] private float recoilPositionLimit = 0.0f;
        [SerializeField] private float minRecoilRotationForce = 0.0f;
        [SerializeField] private float maxRecoilRotationForce = 0.0f;
        [SerializeField] private float recoilAngleLimit = 40.0f;
        [SerializeField] [Range( 0.0f, 5.0f )] private float positionLerpSpeed = 0.05f;
        [SerializeField] [Range( 0.0f, 70.0f )] private float rotationLerpSpeed = 0.0f;       
        [SerializeField] private bool useSpread = false;
        [SerializeField] private int minSpreadCount = 0;
        [SerializeField] private int maxSpreadCount = 0;
        [SerializeField] private float minSpreadAngle = 0;
        [SerializeField] private float maxSpreadAngle = 0;
        [SerializeField] private AudioClip shootSound = null;
        [SerializeField] protected LayerMask hitLayer = new LayerMask();
        [SerializeField] protected int maxBulletBounceCount = 0;
        [SerializeField] private VR_Slider slider = null;
        [SerializeField] private VR_DropZone bulletInsertPoint = null;
        [SerializeField] protected MuzzleFlash m_muzzleFlash = null; 
        #endregion

#if UNITY_EDITOR
        [SerializeField] private VR_WeaponEditorPart m_editorPart = null;
        public VR_WeaponEditorPart EditorPart => m_editorPart;
#endif

        #region PRIVATE
        private int bulletsCounter = 0;
        private bool reloading = false;
        private bool wasButtonPressed = false;
        private float timer = float.MaxValue;
        private VR_WeaponMag currentWeaponMag = null;
        protected VR_Grabbable grabbable = null;
        protected VR_TwoHandGrabbable twoHandGrabbable = null;
        private AudioSource audioSource = null;
        private VR_ControllerGesture gestureScript = null;
        private VR_Haptics hapticsController = null;
        private Bullet insertedBullet = null;
        protected ShootInfo cacheShootInfo;
        private float recoilRotationTarget = 0.0f;
        private bool hasShellObstruction = false;
        private const float HAPTICS_MULTIPLIER = 5.0F;
        private float positionRecoilModifier = 1.0f;
        private float rotationRecoilModifier = 1.0f;
        #endregion

        public VR_WeaponMag CurrentMagazine
        {
            get => currentWeaponMag;
            set
            {
                currentWeaponMag = value;

                if (weaponUI != null) weaponUI.UpdateUI();
            }
        }
        public VR_DropZone MagazineDropZone => magazineDropZone;

        public ReloadMode ReloadMode => reloadMode;
        public LayerMask HitLayer 
        { 
            set => hitLayer = value;
            get => hitLayer;
        }
        
        public int BulletsCounter
        {
            get
            {
                if (reloadMode == ReloadMode.Physics) return barrelScript.BulletsCounter;
                return bulletsCounter;
            }
        }
        public float ReloadTime => reloadTime;


        private void Awake()
        {
            if (reloadMode == ReloadMode.UI) bulletsCounter = clipSize;
        }

        private void Start()
        {
            Initialize();            
            SetupMuzzleFlash();
            IgnoreShellColliders();
            SetupWeaponCallbacks();
        }

        private void Initialize()
        {
            grabbable = GetComponent<VR_Grabbable>();
            audioSource = GetComponent<AudioSource>();
            hapticsController = GetComponent<VR_Haptics>();
            twoHandGrabbable = transform.GetComponentInChildren<VR_TwoHandGrabbable>();

            if (twoHandGrabbable != null)
            {
                twoHandGrabbable.SetApplyTwoHandTransformManually(true);
            }

            if (reloadMode == ReloadMode.Realistic)
            {
                if (magazineDropZone != null && magazineDropZone.StartingDrop != null)
                {
                    CurrentMagazine = magazineDropZone.StartingDrop.GetComponent<VR_WeaponMag>();

                    if (CurrentMagazine != null)
                    {
                        CurrentMagazine.SetOwner( this );
                        //say to the grabbable that ignore disabling and enabling the mag collider
                        grabbable.IgnoreCollider( CurrentMagazine.GetComponent<Collider>() );
                    }

                }

                magazineDropZone.OnDrop.AddListener( OnMagazineDropStateChange );
                magazineDropZone.OnUnDrop.AddListener( OnMagazineUnDropStateChange );
            }

        }

        private void OnMagazineDropStateChange(VR_Grabbable dropGrabbable)
        {
            if (dropGrabbable != null)
            {
               
                VR_WeaponMag mag = dropGrabbable.GetComponent<VR_WeaponMag>();

                if (mag != null)
                {
                    mag.SetOwner( this );
                    CurrentMagazine = mag;
                }
            }
        }

        private void OnMagazineUnDropStateChange(VR_Grabbable undropGrabbable)
        {
            if (undropGrabbable != null)
            {
                
                VR_WeaponMag mag = undropGrabbable.GetComponent<VR_WeaponMag>();

                if (mag != null)
                {
                    mag.SetOwner( null );
                    CurrentMagazine = null;
                }
            }
        }


        private void SetupMuzzleFlash()
        {
            if (m_muzzleFlash == null) return;
            m_muzzleFlash.Initialize(shootPoint);
        }

        private void IgnoreShellColliders()
        {
            //if we have shells inside this weapons they have colliders so, say to the grabbable to ignore those colliders so it dont enable or disable it when object gets grabbed/dropped
            Shell[] shellArray = FindObjectsOfType<Shell>();

            for (int n = 0; n < shellArray.Length; n++)
            {
                grabbable.IgnoreCollider( shellArray[n].GetComponent<Collider>() );
            }
        }

       

        private void SetupWeaponCallbacks()
        {
            if ( (reloadMode == ReloadMode.PumpActionRealistic || reloadMode == ReloadMode.Launcher) && bulletInsertPoint != null)
            {
                bulletInsertPoint.OnDrop.AddListener( OnBulletInserted );
                bulletInsertPoint.OnUnDrop.AddListener( delegate { OnBulletInserted( null ); } );
            }

            if (reloadMode == ReloadMode.Physics)
            {
                grabbable.OnGrabStateChange.AddListener( OnGrabStateChange );
            }
        }

        private void OnBulletInserted(VR_Grabbable bullet)
        {
            if (bullet == null && reloadMode == ReloadMode.Launcher)
            {
                insertedBullet = null;
                return;
            }
                

            if (reloadMode == ReloadMode.Launcher)
            {
                insertedBullet = bullet.GetComponent<Bullet>();
            }

            bulletsCounter++;

            UpdateUI();

            if (reloadMode != ReloadMode.Launcher)
            {
                Destroy( bullet.gameObject );
            }
                

            if (bulletsCounter >= clipSize && reloadMode != ReloadMode.Launcher)
            {
                bulletInsertPoint.enabled = false;
            }
        }

        private void UpdateUI()
        {
            if (weaponUI != null)
                weaponUI.UpdateUI();
        }

        private void OnGrabStateChange(GrabState newState)
        {
            if (newState == GrabState.Grab)
            {
                //cache the gesture script
                gestureScript = grabbable.GrabController.GestureScript;

                if (gestureScript != null)
                {
                    gestureScript.ListenForRotationGesture( OnRotationGesture );
                }
                else
                {
                    Debug.LogWarning( "Grabbing hand dont has a rotation gesture controller" );
                }
            }

            else if (newState == GrabState.Drop)
            {

                if (gestureScript != null)
                {
                    gestureScript.RemoveRotationGestureListener( OnRotationGesture );
                }
                else
                {
                    Debug.LogWarning( "Grabbing hand dont has a rotation gesture controller" );
                }

                //empty the gesture script
                gestureScript = null;
            }
        }

        private void OnRotationGesture(RotationGestureInfo info)
        {           

            VR_Controller controller = grabbable.GrabController;

            float angle = Vector2.Angle( controller.transform.up, Vector2.up );

            //no enought rotation
            if (angle < reloadAngle)
                return;

            float dir = ( controller.ControllerType == VR_ControllerType.Right ? 1.0f : -1.0f );
            angle = Vector3.Angle( controller.transform.right * dir, Vector3.up );

            //is the weapon facing the wrong direction?
            if (angle > 40.0f)
                return;

            barrelScript.Reload( dir, delegate
            {
                UpdateUI();
            } );

        }      
        
        protected virtual void LateUpdate()
        {
            if (grabbable == null) return;
               

            if (twoHandGrabbable != null)
            {
                twoHandGrabbable.TwoHandUpdate();
            }

            bool shootThisFrame = false;
            timer += Time.deltaTime;

            if ( CanShoot() )
            {
                Fire();
                shootThisFrame = true;
            }
            
            UpdateMuzzleFlash(shootThisFrame);
            UpdateRecoil();
            UpdateWasButtonPressed();
        }

        protected virtual void UpdateMuzzleFlash(bool shootThisFrame)
        {
            if (m_muzzleFlash == null) return;
            m_muzzleFlash.InternalUpdate(shootThisFrame);
        }

        private bool CanShoot()
        {
            if (!grabbable.IsGrabbed) return false;

            return isAutomatic ? grabbable.GrabController.Input.GetButton(fireButton) : grabbable.GrabController.Input.GetButtonDown(fireButton);
        }

        public virtual void UpdateRecoil()
        {
            grabbable.RotationOffset = Quaternion.RotateTowards( grabbable.RotationOffset, Quaternion.identity, rotationLerpSpeed * Time.deltaTime );
            grabbable.PositionOffset = Vector3.MoveTowards( grabbable.PositionOffset, Vector3.zero, positionLerpSpeed * Time.deltaTime );

            recoilRotationTarget = Mathf.MoveTowards( recoilRotationTarget, 0.0f, rotationLerpSpeed * Time.deltaTime );
        }

        private void UpdateWasButtonPressed()
        {
            if (wasButtonPressed && grabbable != null && grabbable.GrabController != null)
            {
                wasButtonPressed = grabbable.GrabController.Input.GetButton( fireButton );
            }
        }
        
        protected virtual void Fire()
        {
            //in launcher mode we need to insert a bullet object
            if (reloadMode == ReloadMode.Launcher && insertedBullet == null) return;

            //we still have a shell obstruction we need to eject it
            if (HasShellObstruction()) return;

            if (timer < shootRate || reloading)
            {
                if (reloading)
                {
                    HideMuzzleFlashWhileNoShooting();
                }

                return;
            }

            UpdateWeaponComponents();
           

            if ( IsPhysicsReload() )
            {
                bool success = HandlePhysicsReloadShoot();

                if (!success)
                    return;
            }
            

            wasButtonPressed = true;

            if (IsRealisticReload() )
            {
                bool success = HandleRealisticReloadShoot();

                if (!success) return;
            }
            else if (IsUIReload() )
            {
                HandleUIReloadShoot();
            }
            //you need to insert the bullets manually
            else if ( IsPumpActionRealisticReload() )
            {
                bool success = HandlePumpActionRealisticReloadShoot();

                if (!success)
                    return;
            }

            UpdateUI();
            PlayShootSound();         

            timer = 0.0f;

            Shoot();
            SpawnMuzzleFlash();
            Recoil();
            CrateShellObstructionForPumpActionReload();
            Haptics();
        }

        private void HideMuzzleFlashWhileNoShooting()
        {
            if (m_muzzleFlash != null)
            {
                m_muzzleFlash.SetVisibility(false);
            }
        }

        private bool HasShellObstruction()
        {
            return IsPumpActionReload() && hasShellObstruction;
        }

        private bool IsPumpActionReload()
        {
            return reloadMode == ReloadMode.PumpActionInfiniteBullets || reloadMode == ReloadMode.PumpActionRealistic;
        }

        private void UpdateWeaponComponents()
        {
            if (weaponHammer != null)
                weaponHammer.Shoot();

            if (shellEjector != null && !IsPumpActionReload())
                shellEjector.Eject();
        }

        private bool IsPhysicsReload()
        {
            return reloadMode == ReloadMode.Physics && barrelScript != null;
        }

        private bool HandlePhysicsReloadShoot()
        {
            //reset timer
            timer = 0.0f;
            wasButtonPressed = true;

            if (!barrelScript.IsReady)
                return false;

            //cache before calling shoot and increased used bullets counter
            bool hasBullets = barrelScript.HasBullets;

            barrelScript.Shoot();

            if (!hasBullets)
            {
                HideMuzzleFlashWhileNoShooting();

                return false;
            }

            return true;
        }

        private bool IsRealisticReload()
        {
            return reloadMode == ReloadMode.Realistic;
        }

        private bool HandleRealisticReloadShoot()
        {
            if (CurrentMagazine == null || CurrentMagazine.Bullets <= 0)
            {
                HideMuzzleFlashWhileNoShooting();
                return false;
            }

            else
            {
                CurrentMagazine.Bullets--;
            }

            return true;
        }

        private bool IsUIReload()
        {
            return reloadMode == ReloadMode.UI;
        }

        private void HandleUIReloadShoot()
        {
            bulletsCounter--;

            if (bulletsCounter <= 0)
            {
                reloading = true;
                weaponUI.Reload( delegate
                {
                    bulletsCounter = clipSize;
                    reloading = false;

                    UpdateUI();
                } );
            }
        }

        private bool IsPumpActionRealisticReload()
        {
            return reloadMode == ReloadMode.PumpActionRealistic;
        }

        private bool HandlePumpActionRealisticReloadShoot()
        {
            bulletsCounter--;

            if (bulletsCounter < 0)
            {
                bulletsCounter = 0;
                return false;
            }

            if (!bulletInsertPoint.enabled)
                bulletInsertPoint.enabled = true;

            return true;
        }

        private void PlayShootSound()
        {
            if (audioSource == null) return;
            
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.clip = shootSound;
            audioSource.Play();
        }     

        private void Shoot()
        {
            ShootInfo data = CreateShootInfo();

            if (useSpread)
            {
                int spreadCount = Random.Range( minSpreadCount, maxSpreadCount );

                data.dmg = data.dmg / spreadCount;
                data.hitForce = data.hitForce / spreadCount;

                for (int n = 0; n < spreadCount; n++)
                {
                    ShootInfo clone = (ShootInfo) data.Clone();
                    Vector3 dir = GetRandomSpreadDirection( clone.dir );
                    clone.dir = dir;

                    Bullet bullet = InstantiateBullet(shootPoint.position, Quaternion.LookRotation(dir));
                    bullet.Launch( clone );
                }
            }
            else
            {
                Bullet bullet = null;

                if (reloadMode == ReloadMode.Launcher)
                {
                   
                    GameObject go = Instantiate( insertedBullet.gameObject, shootPoint.position, shootPoint.rotation );
                    data.dir = shootPoint.forward;
                    go.transform.localScale = insertedBullet.transform.lossyScale;
                    bullet = go.GetComponent<Bullet>();
                    Destroy( insertedBullet.gameObject );

                }
                else
                {
                    bullet = InstantiateBullet(shootPoint.position, shootPoint.rotation );                   
                }

                
                bullet.Launch( data );
                OnShootComplete(bullet, data.dir);
            }
            
            
        }

        protected virtual Bullet InstantiateBullet(Vector3 position, Quaternion rotation)
        {
            return Instantiate( bulletPrefab, position, rotation );  
        }

        protected virtual void OnShootComplete(Bullet bullet, Vector3 dir) { }


        private void SpawnMuzzleFlash()
        {
            if (m_muzzleFlash == null) return;
            m_muzzleFlash.Fire();
        }

        protected virtual ShootInfo CreateShootInfo()
        {            
            cacheShootInfo.dir = shootPoint.forward;
            cacheShootInfo.dmg = dmg;
            cacheShootInfo.canDismember = canDismember;
            cacheShootInfo.range = range;
            cacheShootInfo.speed = bulletSpeed;
            cacheShootInfo.maxBounceCount = maxBulletBounceCount;
            cacheShootInfo.hitForce = Random.Range( minHitForce, maxHitForce );
            cacheShootInfo.hitLayer = hitLayer;
            cacheShootInfo.hitEffect = hitEffect;
            cacheShootInfo.sender = grabbable.GrabController.transform.root.gameObject;

            return cacheShootInfo;
        }

        private Vector3 GetRandomSpreadDirection(Vector3 forward)
        {
            return ( Quaternion.Euler( new Vector3( Random.Range( -1.0f, 1.0f ), Random.Range( -1.0f, 1.0f ), Random.Range( -1.0f, 1.0f ) ) * Random.Range( minSpreadAngle, maxSpreadAngle ) ) * forward ).normalized;
        }

        protected  virtual void Recoil()
        {
            Vector3 offset = Vector3.zero;

            #if SDK_STEAM_VR
            offset = new Vector3(45.0f , 0.0f , 0.0f);
            #endif

            grabbable.PositionOffset += Random.Range( minRecoilPositionForce * positionRecoilModifier , maxRecoilPositionForce * positionRecoilModifier ) * ( Quaternion.Euler( offset ) *  Vector3.forward  ) * -1.0f;

            //clamp the distance to the limit
            float d = grabbable.PositionOffset.magnitude;

            if (d > recoilPositionLimit)
            {
                grabbable.PositionOffset = grabbable.PositionOffset.normalized * recoilPositionLimit;
            }

            float recoilRotation = Random.Range( minRecoilRotationForce * rotationRecoilModifier, maxRecoilRotationForce * rotationRecoilModifier );
            recoilRotationTarget += recoilRotation;
            grabbable.RotationOffset *= Quaternion.AngleAxis( recoilRotation, Vector3.right * -1.0f );


            //clamp the angle
            if (recoilRotationTarget > recoilAngleLimit)
            {
                recoilRotationTarget = recoilAngleLimit;
                grabbable.RotationOffset = Quaternion.AngleAxis( recoilRotationTarget, Vector3.right * -1.0f );
            }
        }

        private void Haptics()
        {
            //haptics feedback
            if (hapticsController != null)
            {
                float haptics = ( grabbable.PositionOffset.magnitude ) + ( ( grabbable.RotationOffset.eulerAngles / recoilAngleLimit ).magnitude ) * HAPTICS_MULTIPLIER;

                if (haptics > 0.1f)
                    hapticsController.SetHaptics( haptics, grabbable.GrabController );
            }
        }

        private void CrateShellObstructionForPumpActionReload()
        {
            //after shooting we have a shell obstruction
            if (IsPumpActionReload())
            {
                hasShellObstruction = true;
                slider.OnValueChange.AddListener( OnWeaponSliderValueChange );
            }
        }

        private void OnWeaponSliderValueChange(float v)
        {
            if (v > 0.95f)
            {
                shellEjector.Eject();
                hasShellObstruction = false;
                slider.OnValueChange.RemoveListener( OnWeaponSliderValueChange );
            }
                
        }

        private void OnDrawGizmosSelected()
        {
            DrawRangeGizmo();
        }

        private void DrawRangeGizmo()
        {
            if (shootPoint == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine( shootPoint.position, shootPoint.position + ( shootPoint.forward * range ) );
        }

        public void SetRecoilModifierValue(float position , float rotation)
        {
            positionRecoilModifier = position;
            rotationRecoilModifier = rotation;
        }
    }

    [System.Serializable]
    public class TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 dir;

        public TransformInfo(Transform t)
        {
            position = t.position;
            rotation = t.rotation;
            dir = t.forward;
        }
    }

}

