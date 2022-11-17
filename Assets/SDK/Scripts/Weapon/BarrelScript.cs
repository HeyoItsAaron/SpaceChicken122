using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VRShooterKit.WeaponSystem
{
    //this script is being use in the revolver for the physics reload system
    public class BarrelScript : MonoBehaviour
    {

        [SerializeField] private Shell shellPrefab = null;
        [SerializeField] private Transform barrelCylinder = null;
        [SerializeField] private float barrelRotSpeed = 0.5f;        
        [SerializeField] private float slideThreshold = 0.1f;
        [SerializeField] private float slideRotateTime = 0.1f;        
        [SerializeField] private float barrelOpenTime = 0.2f;
        [SerializeField] private float barrelOpenAngle = 35.0f;
        [SerializeField] private float barrelTorque = 50.0f;
        [SerializeField] private float angularDrag = 0.5f;          
        [SerializeField] private Transform[] shellSlots = null;
      
        private List<Shell> activeBullets = new List<Shell>();
        private Vector3 barrelOpenDir = Vector3.zero;
        private Vector3 barrelRotDir = Vector3.zero;
        private Vector3 spinBarrelDir = Vector3.zero;      
        private Quaternion thisInitialRotation = Quaternion.identity;       
        private Quaternion barrelOriginalRotation = Quaternion.identity;        
        private float currentBarrelTorque = 0.0f;
        private Quaternion barrelTargetRotation = Quaternion.identity;        
        private bool playingReloadAnimation = false;
        private int usedBulletsCounter = 0;
        private int barrelRotIndex = 0;
       

        public bool IsReady { get { return !playingReloadAnimation; } }
        public bool HasBullets
        {
            get
            {               
                return usedBulletsCounter != activeBullets.Count;
            }
        }

        public int BulletsCounter { get { return activeBullets.Count - usedBulletsCounter; } }

        private void Awake()
        {
            thisInitialRotation = transform.localRotation;           
            barrelOriginalRotation = barrelCylinder.localRotation;
            barrelTargetRotation = Quaternion.identity;

            CreateShells();
        }

        //create new shells after reloading
        private void CreateShells()
        {
            
            for (int n = 0; n < shellSlots.Length; n++)
            {               
                if ( IsShellSlotEmpty( shellSlots[n] ) )
                {
                    Shell clone = CreateShellAtPoint( shellSlots[n] );
                    activeBullets.Add( clone );
                }               
            }
        }

        private bool IsShellSlotEmpty(Transform slot)
        {
            return slot.childCount == 0;
        }

        private Shell CreateShellAtPoint(Transform p)
        {            
            Shell clone = Instantiate(shellPrefab , p);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = Vector3.one;

            return clone;
        }
       
        private void Update()
        {
            if ( !ShouldRotateBarrel() )
                return;

            barrelCylinder.localRotation = Quaternion.Lerp( barrelCylinder.localRotation , CalculateBarrelTargetRotation() , barrelRotSpeed );              
                
        }

        private bool ShouldRotateBarrel()
        {
            return !playingReloadAnimation;
        }

        private Quaternion CalculateBarrelTargetRotation()
        {
            //add some offset
            return barrelTargetRotation * barrelOriginalRotation;
        }
       
        
        public void Shoot()
        {           
            //increase the barrel rot index
            barrelRotIndex++;

            barrelTargetRotation = CalculateBarrelRotationAtIndex(barrelRotIndex);
            
            if (barrelRotIndex > shellSlots.Length - 1)
                barrelRotIndex = 0;

            if ( usedBulletsCounter < shellSlots.Length )
                usedBulletsCounter++;
          
        }

        private Quaternion CalculateBarrelRotationAtIndex(int index)
        {
            return Quaternion.AngleAxis( 360.0f * ( (float) index / (float) shellSlots.Length ), Vector3.right );
        }

        public void Reload(float dir , Action onComplete)
        {
            if (playingReloadAnimation)
                return;

            CalculateDirections(dir);
            StartCoroutine( ReloadRoutine( onComplete ) );
        }
        
        private void CalculateDirections(float dir)
        {
            barrelOpenDir = Vector3.forward * -1.0f * dir;
            barrelRotDir = Vector3.right * dir;           
            spinBarrelDir = barrelRotDir * dir;
        }

        private IEnumerator ReloadRoutine(Action onComplete)
        {
            playingReloadAnimation = true;

            //animation code
            yield return StartCoroutine( OpenBarrel() );
            StartCoroutine( SpinBarrel() );
            yield return StartCoroutine( RotateBarrelOpen() );
            yield return StartCoroutine( EjectBullets() );            
            yield return StartCoroutine( RotateBarrelClose() );
            CreateShells();
            StartCoroutine( CloseBarrel() );
            currentBarrelTorque = 0.0f;
            yield return StartCoroutine( ResetBarrelRotation() );

            barrelTargetRotation = Quaternion.identity;
            usedBulletsCounter = 0;
            barrelRotIndex = 0;

            if (onComplete != null)
                onComplete();

            playingReloadAnimation = false;
        }

        private IEnumerator OpenBarrel()
        {
            float startTime = Time.time;
            Vector3 startPosition = transform.localPosition;
            Vector3 targetPosition = transform.localPosition  + ( barrelOpenDir * slideThreshold );

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                progress = ( Time.time - startTime ) / barrelOpenTime;
                transform.localPosition = Vector3.Lerp( startPosition , targetPosition , progress );

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator SpinBarrel()
        {
            currentBarrelTorque = barrelTorque;

            while (currentBarrelTorque > 0.0f)
            {
                barrelCylinder.Rotate( spinBarrelDir , currentBarrelTorque * Time.deltaTime);

                //simulate angular drag
                currentBarrelTorque -= angularDrag * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator RotateBarrelOpen()
        {
            float rotateStartTime = Time.time;
            
            Quaternion startRotation = transform.localRotation;
            Quaternion targetRotation = transform.localRotation * ( Quaternion.Euler( barrelRotDir * -1.0f * barrelOpenAngle ) );

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                progress = ( Time.time - rotateStartTime ) / slideRotateTime;
                transform.localRotation = Quaternion.Slerp(startRotation , targetRotation, progress );

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator EjectBullets()
        {

            if (usedBulletsCounter == 0)
            {
                yield return new WaitForSeconds( 0.35f );
            }

            else
            {
                for (int n = 0; n < usedBulletsCounter ; n++)
                {

                    activeBullets[0].Eject();
                    activeBullets.RemoveAt( 0 );
                    
                    yield return new WaitForSeconds( 0.2f );
                }
            }
                   
        }

        private IEnumerator RotateBarrelClose()
        {
            float rotateStartTime = Time.time;

            Quaternion startRotation = transform.localRotation;
            Quaternion targetRotation = thisInitialRotation;

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                progress = ( Time.time - rotateStartTime ) / slideRotateTime;
                transform.localRotation = Quaternion.Slerp( startRotation, targetRotation, progress );

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator CloseBarrel()
        {
            float startTime = Time.time;
            Vector3 startPosition = transform.localPosition;
            Vector3 targetPosition = transform.localPosition + ( barrelOpenDir * -slideThreshold );

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                progress = ( Time.time - startTime ) / barrelOpenTime;
                transform.localPosition = Vector3.Lerp( startPosition, targetPosition, progress );

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ResetBarrelRotation()
        {
            float rotateStartTime = Time.time;

            Quaternion startRotation = barrelCylinder.localRotation;
            Quaternion targetRotation = barrelOriginalRotation;

            float progress = 0.0f;

            while (progress < 1.0f)
            {
                progress = ( Time.time - rotateStartTime ) / 0.175f;
                barrelCylinder.localRotation = Quaternion.Slerp( startRotation, targetRotation , progress );

                yield return new WaitForEndOfFrame();
            }
        }     


    }

}

