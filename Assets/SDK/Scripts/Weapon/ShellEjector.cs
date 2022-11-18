using UnityEngine;

namespace VRShooterKit.WeaponSystem
{
    //this script is used by the weapon system, will create shells ejections when needed
    public class ShellEjector : MonoBehaviour
    {
        [SerializeField] private GameObject shellPrefab = null;
        [SerializeField] private Transform shellEjectPoint = null;
        [SerializeField] private Vector3 startRotation = Vector3.zero;
        [SerializeField] private float minEjectForce = 0.0f;
        [SerializeField] private float maxEjectForce = 0.0f;
        [SerializeField] private float minEjectTorque = 0.0f;
        [SerializeField] private float maxEjectTorque = 0.0f;
              

        public void Eject()
        {
            Rigidbody rb = Instantiate(shellPrefab , shellEjectPoint.position , Quaternion.Euler(startRotation)).GetComponent<Rigidbody>();

            rb.AddForce( shellEjectPoint.forward * Random.Range( minEjectForce , maxEjectForce ) , ForceMode.VelocityChange  );            
            rb.AddTorque( transform.up * Random.Range(minEjectTorque , maxEjectTorque) , ForceMode.VelocityChange);
           
        }
       

    }
}

