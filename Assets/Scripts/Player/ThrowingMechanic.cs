using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowingMechanic : ArmHandler
{
    public enum Arms
    {
        right,
        left
    }
    public Arms arms;
    public enum ArmState 
    {
        ArmAttached,
        ArmDetached,
        ArmIsDeactive
    }
    public ArmState armState;
    Vector2 aimDirection;
    public Input fireButton;
    public bool armIsDetactivated;
    private bool canShoot;
    [Header("Shooting Settings")]
    public float minSpeed = 15;
    public float maxSpeed = 45;
    private float throwingForce;
    [Header("Arm Settings")]
    public Transform aimingArm;
    public GameObject armToHide;
    public ArmScript armToInstantiate;
    public ArmScript armClone;
    public int projectileAmount = 1;
    float scale;
    public float maxScale = 1.8f;
    public bool isActive;
    public CharacterController player;

   
    private void Update()
    {
        switch (armState)
        {
            case ArmState.ArmAttached:
               if(isActive)
               {
                aimDirection = new Vector3(Input.GetAxisRaw("AimHorizontal"), Input.GetAxisRaw("AimVertical"),0);
                float rx = Input.GetAxis("AimHorizontal");
                float ry = Input.GetAxis("AimVertical");
                float angle = Mathf.Atan2(rx, ry) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                if  (ry == 0 && rx == 0)
                {
                canShoot = false;
                HideAnimatedArm(false);
                }
                else if (rx != 0 || ry != 0)
                {
                canShoot = true;
                HideAnimatedArm(true);
                }
                if (armClone != null && armClone.canCallBackArm)
                {
                if (Input.GetButtonDown("Fire2"))
                {
                    armClone.GetComponent<ArmScript>().armIsReturning = true;
                }
                }
                if (canShoot && Input.GetButtonDown("Fire2") && projectileAmount == 1)
                {
                StartCoroutine(ChargedShot(minSpeed,maxSpeed));
                } 
               }
                
            break;

            case ArmState.ArmDetached:
                HideAnimatedArm(true);
                if (Input.GetButtonDown("Fire2"))
                {
                    
                    armClone.GetComponent<ArmScript>().armIsReturning = true;
                }
                if(armClone == null)
                {
                   
                    projectileAmount = 1;
                    //DO ARM TOGGLE FUNCTION
                    player.WhatArmIsActive(arms, true);
                    HideAnimatedArm(false);
                    armState = ArmState.ArmAttached;
                }

            break;
        }
    }

    IEnumerator ChargedShot(float startForce, float endForce)
    {
        float elapsed = 0;
        float duration = 1.5f;
        Vector3 scaleVector;
        
        while(elapsed < duration && Input.GetButton("Fire2"))
        {
            throwingForce = Mathf.SmoothStep(startForce, endForce,elapsed);
            scale = Mathf.SmoothStep(1, maxScale, elapsed);
            scaleVector = new Vector3(aimingArm.localScale.x, scale, aimingArm.localScale.z);
            aimingArm.transform.localScale = scaleVector;
            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
       
        aimingArm.localScale = Vector3.one;
        projectileAmount -= 1;    
        armClone = Instantiate((ArmScript)armToInstantiate, transform.position, transform.rotation);
        scaleVector = new Vector3(armClone.transform.localScale.z,scale, armClone.transform.localScale.z);
        armClone.transform.localScale = scaleVector;
        armClone.canCallBackArm = true;
        armClone.myRb.AddForce(transform.up * throwingForce, ForceMode2D.Impulse); 
        armState = ArmState.ArmDetached;
        player.WhatArmIsActive(arms, false);
    }
    public void HideAnimatedArm(bool isHidden)
    {
        //Hide In Attached Arm State
        switch (armState)
        {
            case ArmState.ArmAttached:

            if(isHidden)
            {
            armToHide.SetActive(false);
            aimingArm.gameObject.SetActive(true);
            } 
            else if(!isHidden)
            {
            armToHide.SetActive(true);
            aimingArm.gameObject.SetActive(false);
            }
            break;

            case ArmState.ArmDetached:
            armToHide.gameObject.SetActive(false);
            aimingArm.gameObject.SetActive(false);

            break;
        }  
        // HIDE IN DETACHED STATE
    }
}
