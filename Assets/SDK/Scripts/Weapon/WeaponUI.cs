using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace VRShooterKit.WeaponSystem
{
    public class WeaponUI : MonoBehaviour
    {
        #region INSPECTOR
        [SerializeField] private VR_Weapon weapon = null;
        [SerializeField] private Text rightBulletCounter = null;
        [SerializeField] private Text leftBulletCounter = null;
        [SerializeField] private bool invertTextPosition = false;
        [SerializeField] private Image reloadBar = null;
        #endregion

        private VR_Grabbable weaponGrabbable = null;


        private void Start()
        {


            weaponGrabbable = weapon.GetComponent<VR_Grabbable>();
            weaponGrabbable.OnGrabStateChange.AddListener( OnWeaponGrabStateChange );

            if (weapon.ReloadMode == ReloadMode.UI)
            {
                string bullets = weapon.BulletsCounter.ToString();

                rightBulletCounter.text = bullets;
                leftBulletCounter.text = bullets;

                reloadBar.gameObject.SetActive( false );
            }

            rightBulletCounter.gameObject.SetActive( false );
            leftBulletCounter.gameObject.SetActive( false );
            reloadBar.gameObject.SetActive( false );


        }

        public void UpdateUI()
        {
            string bullets = "";

            if (weapon.ReloadMode == ReloadMode.Realistic)
                bullets = ( weapon.CurrentMagazine != null ? weapon.CurrentMagazine.Bullets : 0 ).ToString();
            else if (weapon.ReloadMode == ReloadMode.UI || weapon.ReloadMode == ReloadMode.Physics)
                bullets = weapon.BulletsCounter.ToString();

            rightBulletCounter.text = bullets;
            leftBulletCounter.text = bullets;
        }

        public void Reload(Action onComplete)
        {
            reloadBar.gameObject.SetActive( true );
            reloadBar.fillAmount = 0.0f;
            StartCoroutine( CO_Reload( weapon.ReloadTime, onComplete ) );
        }

        private IEnumerator CO_Reload(float t, Action onComplete)
        {
            float timer = 0.0f;

            while (timer < t)
            {
                reloadBar.fillAmount = timer / t;
                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            reloadBar.fillAmount = 1.0f;
            yield return new WaitForEndOfFrame();

            onComplete();
            reloadBar.gameObject.SetActive( false );
        }

        private void OnWeaponGrabStateChange(GrabState state)
        {
            if (state == GrabState.Grab)
            {               
                rightBulletCounter.gameObject.SetActive( weaponGrabbable.GrabController.ControllerType == VR_ControllerType.Right );
                leftBulletCounter.gameObject.SetActive( weaponGrabbable.GrabController.ControllerType == VR_ControllerType.Left );

                if (invertTextPosition)
                {
                    rightBulletCounter.gameObject.SetActive( !rightBulletCounter.gameObject.activeInHierarchy );
                    leftBulletCounter.gameObject.SetActive( !leftBulletCounter.gameObject.activeInHierarchy );
                }
                    
            }

            else if (state == GrabState.Drop)
            {
                rightBulletCounter.gameObject.SetActive( false );
                leftBulletCounter.gameObject.SetActive( false );
            }
        }


    }

}

