using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmHandler : MonoBehaviour
{
   
    public static bool haveRightArm = true;
    public static bool haveLeftArm = true;

    private void Update() {
        Debug.Log(haveRightArm);
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
   
}
