using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowingMechanic : MonoBehaviour
{
    Vector2 aimDirection;
    public Input fireButton;
    private bool canShoot;
    private bool armIsDetached;
    public float throwingForce;
    public ArmScript armToShoot;
    public GameObject armToHide;
    //SLOW DOWN TIME EFFECTS
    public TimeManager timeManager;
    public CameraEffects camFX;
    //AXE SETTINGS
    public Rigidbody2D axe;
    private Rigidbody2D axeClone;
    public Transform axeSpawnPoint;
    public int projectileAmount = 1;
    public float returnSpeed = 10f;
    
   
    private void Update()
    {
        aimDirection = new Vector3(Input.GetAxisRaw("AimHorizontal"), Input.GetAxisRaw("AimVertical"),0);
        float rx = Input.GetAxis("AimHorizontal");
        float ry = Input.GetAxis("AimVertical");
        

        float angle = Mathf.Atan2(rx, ry) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (angle == 0)
        {
            HideAnimatedArm(false);
        }
        else if (angle != 0)
        {
            HideAnimatedArm(true);
        }
        if (axeClone != null && axeClone.GetComponent<ArmScript>().canCallBackArm)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("PRESSED");
                
                axeClone.GetComponent<ArmScript>().armIsReturning = true;
            }
        }
        if (canShoot && Input.GetButtonDown("Fire2") && projectileAmount == 1)
        {
            StartCoroutine(ChargedShot(15,45));
            /*projectileAmount -= 1;   
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

            axeClone.AddForce(transform.up * throwingForce,ForceMode2D.Impulse);*/
        } 
        
    }

    IEnumerator ChargedShot(float startForce, float endForce)
    {

        float elapsed = 0;
        float duration = 2f;
        while(elapsed < duration && Input.GetButton("Fire2"))
        {
            throwingForce = Mathf.SmoothStep(startForce, endForce,elapsed);
            //Debug.Log(throwingForce);
            Debug.Log(elapsed);
            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        projectileAmount -= 1;    
        axeClone = Instantiate(axe, axeSpawnPoint.position, Quaternion.identity);
        axeClone.GetComponent<ArmScript>().canCallBackArm = true;
        axeClone.AddForce(transform.up * throwingForce,ForceMode2D.Impulse);
    }
    void HideAnimatedArm(bool isHidden)
    {
        if(isHidden)
        {
            armToHide.SetActive(false);
            armToShoot.gameObject.SetActive(true);
        } else if(!isHidden)
        {
            armToHide.SetActive(true);
            armToShoot.gameObject.SetActive(false);
        }
    }
}
