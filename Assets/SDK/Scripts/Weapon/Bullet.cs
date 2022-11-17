using UnityEngine;
using System.Collections;

namespace VRShooterKit.WeaponSystem
{
    public class Bullet : Projectile
    {
        [SerializeField] private TrailRenderer trailRender = null;
        [SerializeField] private Gradient invisibleGrandient;

        private float distanceTraveled = 0.0f;
        protected bool shouldHit = false;
        protected int bounceCount = 0;
        private Gradient originalGradient;
        private float inmeadiatleHitDistance = 0.5f;

        protected override void Awake()
        {
            base.Awake();

            if (trailRender != null)
            {
                originalGradient = trailRender.colorGradient;
            }

        }

        private void Update()
        {
            if (!launched)
                return;
            
            if (shouldHit)
            {
                
                if (shootInfo.hitEffect != null)
                    Instantiate(shootInfo.hitEffect , transform.position , Quaternion.identity);

                Destroy( gameObject );           
                return;
            }

                      
            float travelDistance = Time.deltaTime * shootInfo.speed;
            distanceTraveled += travelDistance;

           

            RaycastHit hit;
            bool hitSomething = HitSomething(transform.position, transform.forward, travelDistance, out hit);

            if (hitSomething && hit.collider != playerCollider)
            {                
                HandleHit(hit);
            }
            else
            {
                transform.position += shootInfo.dir * travelDistance;

                if (distanceTraveled >= shootInfo.range)
                {
                    Destroy( gameObject );
                }
                   
            }
        }

        protected bool HitSomething(Vector3 from, Vector3 dir, float travelDistance, out RaycastHit hit)
        {
            return  Physics.Raycast( transform.position, dir, out hit, travelDistance, shootInfo.hitLayer.value );
        }

        protected void HandleHit(RaycastHit hit)
        {
            transform.position += transform.forward * hit.distance;                
            bool handleByDamageSystem = TryDoDamage(hit.collider);

            //if we hit a object that dont process the damage in his on way using Damageable components, add just the impact force in a default way
            if (!handleByDamageSystem)
            {
                ApplyImpactForce( hit.rigidbody, hit.point );                    
            }
                    

            SurfaceDetails surface = hit.collider.GetComponent<SurfaceDetails>();

            if (surface != null)
            {
                if (surface.BulletsCanBounce && bounceCount < shootInfo.maxBounceCount)
                {
                    BounceOnSurface( hit , surface );
                }
                else
                {
                    shouldHit = true;
                }
            }
            else if (bounceCount < shootInfo.maxBounceCount)
            {
                BounceOnSurface( hit , null );
            }
            else
            {
                shouldHit = true;
            }
        }

        protected void BounceOnSurface(RaycastHit hit , SurfaceDetails surface = null)
        {
            bounceCount++;
            //reflect the bullet on the surface
            Vector3 newDir = Vector3.Reflect( transform.forward, hit.normal );
            transform.forward = newDir;
            shootInfo.dir = newDir;

            float speedLose = 0.20f;

            if (surface != null)
                speedLose = surface.BulletsSpeedLoseOnBounce;

            shootInfo.speed -= speedLose * shootInfo.speed;
        }

        private int RandomSign()
        {
            return Random.value < .5 ? 1 : -1;
        }

        public override void Launch(ShootInfo info)
        {
            HandleBulletLaunch(info);
        }

        protected virtual void HandleBulletLaunch(ShootInfo info)
        {
            ApplyPlayerVelocity();
            StartCoroutine( SetInvisibleForTwoFrames() );
            base.Launch( info );
            CheckForImmediatelyHit();
            MoveForward(info);
        }

        protected void CheckForImmediatelyHit()
        {
            bool hitSomething = HitSomething(transform.position, transform.forward, inmeadiatleHitDistance, out RaycastHit hit);

            if (hitSomething && hit.collider != playerCollider)
            {                
                HandleHit(hit);
            }
        }

        private void ApplyPlayerVelocity()
        {
            Vector3 playerVelocity = VR_Manager.instance.Player.CharacterController.velocity;
            playerVelocity.y = 0.0f;
            transform.position += playerVelocity * Time.deltaTime * 3.0f;
        }

        private void MoveForward(ShootInfo info)
        {
            float originalSpeed = info.speed;
            info.speed = 0.15f;

            Update();
            info.speed = originalSpeed;
        }

        private IEnumerator SetInvisibleForTwoFrames()
        {
            if(trailRender != null)
                trailRender.colorGradient = invisibleGrandient;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (trailRender != null)
                trailRender.colorGradient = originalGradient;
                       

            launched = true;
        }


    }

}

