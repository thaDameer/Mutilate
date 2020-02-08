using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmHandler : MonoBehaviour
{
   public enum WhatArm
   {
       rightArm,
       leftArm
   }
    public WhatArm whatArm;
    public static bool haveRightArm = true;
    public static bool haveLeftArm = true;
    public ThrowingMechanic leftArmScript, rightArmScript;


    private void Update() {
       
    }

    public void ToggleArm(string whatArm, bool isActive)
    {
        if(whatArm == "leftArm")
        {
            if(isActive)
            {
                haveRightArm = true;
               
            }
            else
            {
                haveRightArm = false;
            }
            
        } 
        else if(whatArm == "rightArm")
        {
           if(isActive)
           {
               haveRightArm = true;
           }
           else
           {
               haveRightArm = false;
           }
        }
    }
    void ToggerHierarchy(bool controlRight, bool controlLeft)
    {
        if(controlRight)
        {
            rightArmScript.isActive = true;
            leftArmScript.isActive = false;
        } else if(controlLeft)
        {
            rightArmScript.isActive = false;
            leftArmScript.isActive = true;
        }
    }
}
