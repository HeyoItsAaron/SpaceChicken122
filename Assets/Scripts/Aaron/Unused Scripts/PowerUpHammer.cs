using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PowerUpHammer : MonoBehaviour
{
    public GameObject powerUpIcon; // this is the thing we crush to activate the hammer
    public GameObject hammerPrefab; // this chooses what to spawn
    private GameObject playerHammer; // this is the instance of hammer created on activatePowerUp()
    public GameObject targetRay;
    private Rigidbody hammerRigidBody;
    public InputActionProperty rightGrabPull;
    public WristHealthBar wristUI;

    //public RaycastHit SelectedTargetLocation;

    public Vector3 targetLocation; // 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSelectEntered()
    {

    }

    public void activatePowerUp()
    {
        // playerHammer = Instantiate(hammerPrefab, spawner.transform.position, spawner.transform.rotation);
        powerUpIcon.SetActive(false);
        //targetRay.SetActive(true);

    }
    public void spawnHammer()
    {
        //targetLocation = targetRay.transform.position;
        //targetRay.SetActive(false);
        wristUI.AddPowerUp();
        playerHammer = Instantiate(hammerPrefab, new Vector3(targetLocation.x, targetLocation.y + 300, targetLocation.x) , Quaternion.Euler( new Vector3(0, 0, 180) ) );
        hammerRigidBody = playerHammer.GetComponent<Rigidbody>();
        hammerRigidBody.velocity = Vector3.down * 100;

        Destroy(powerUpIcon);
        StartCoroutine(destroyAfterThisManySeconds());
    }
    void setHammerTime()
    {

    }
    IEnumerator destroyAfterThisManySeconds()
    {
        yield return new WaitForSeconds(60);
        Destroy(playerHammer);
    }
}
