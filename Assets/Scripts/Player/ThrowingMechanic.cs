using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowingMechanic : MonoBehaviour
{
    Vector2 aimDirection;
    public static ThrowingMechanic instance;
    public float throwingForce;
    public GameObject armObj;
    //SLOW DOWN TIME EFFECTS
    public TimeManager timeManager;
    public CameraEffects camFX;
    //AXE SETTINGS
    public Rigidbody2D axe;
    private Rigidbody2D axeClone;
    public Transform axeSpawnPoint;
    public int projectileAmount = 1;
    public float returnSpeed = 10f;
    
    private void Awake() {
        instance = this;
    }

    private void Update()
    {
        aimDirection = new Vector3(Input.GetAxisRaw("AimHorizontal"), Input.GetAxisRaw("AimVertical"),0);
        float rx = Input.GetAxis("AimHorizontal");
        float ry = Input.GetAxis("AimVertical");
        

        float angle = Mathf.Atan2(rx, ry) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (!LeftTrigger())
        {
            armObj.SetActive(false);
        }
        else if (LeftTrigger())
        {
            armObj.SetActive(true);
        }
        if (axeClone != null && axeClone.GetComponent<ArmScript>().canCallBackArm)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                axeClone.GetComponent<ArmScript>().armIsReturning = true;
            }
        }
        if (LeftTrigger() && Input.GetButtonDown("Fire2") && projectileAmount == 1)
        {
            projectileAmount -= 1;   
            // axeClone = Instantiate(axe, axeSpawnPoint.position, Quaternion.Euler(0,0, Random.Range(0,360f)));
            axeClone = Instantiate(axe, axeSpawnPoint.position, Quaternion.identity);
            axeClone.GetComponent<ArmScript>().canCallBackArm = true;
            //axeClone.gravityScale = 0;
            if(rx < 0)
            {
                axeClone.AddTorque(-50F);
            } else if(rx > 0)
            {
                axeClone.AddTorque(50f);
            }

            axeClone.AddForce(transform.up * throwingForce,ForceMode2D.Impulse);
        }
        
    }

    public bool LeftTrigger()
    {
        if(Input.GetAxisRaw("LeftTrigger") > 0)
        {
           // camFX.FadeInVignette();
           // timeManager.DoSlowMotion();
            return true;      
        } else 
        {
          //  camFX.FadeOutVignette();
           // timeManager.DoRealTime();
            return false;
        }    
    }

   
}
