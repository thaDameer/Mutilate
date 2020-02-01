using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TimeManager : MonoBehaviour
{


    //TIME ADJUSTMENTS
    public float slowDownFactor = 0.4f;
    bool timeIsSlowedDown;


   


    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        timeIsSlowedDown = true;
    }
    public void DoRealTime()
    {
        Time.timeScale = 1f;
        timeIsSlowedDown = false;
    }

    public bool TimeIsSlowedDown()
    {
        if (timeIsSlowedDown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void VignetteEffect()
    {
        
    }

    public IEnumerator AirSlashFreeze(Rigidbody2D myRB)
    {
        var grav = myRB.gravityScale;
        var moveSpeed = myRB.GetComponent<CharacterController>().moveSpeed;
        //myRB.bodyType = RigidbodyType2D.Static;
        myRB.velocity = Vector3.zero;
       myRB.GetComponent<CharacterController>().moveSpeed = moveSpeed / 2;
        myRB.gravityScale = 0;
        yield return new WaitForSeconds(0.1f);
        myRB.gravityScale = grav;
        myRB.GetComponent<CharacterController>().moveSpeed = moveSpeed;
        //myRB.bodyType = RigidbodyType2D.Dynamic;   
    }
}
