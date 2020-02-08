using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmScript : MonoBehaviour
{
    float countdownToLayerswitch = .3f;
    [HideInInspector]
    public Rigidbody2D myRb;
    [HideInInspector]
    public bool isReturning;
    private Quaternion armStartRot;
    private Transform player;
    private float returnSpeed = 10f;
    ThrowingMechanic throwingMechanic;
    public bool canCallBackArm = false;
    public bool armIsReturning = false;



    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
    }
    private void Start() {
        armStartRot = gameObject.transform.rotation;
    }
    private void Update()
    {
        countdownToLayerswitch -= Time.deltaTime;
        if(countdownToLayerswitch <= 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Axe");
        }
        if (canCallBackArm)
        {
            if (armIsReturning)
            {
                RecallAxe(15f, 1f);
            }
            
        }
    }
    
    IEnumerator DelayRigidBody(float time)
    {
        float elapsed = 0;
        float duration = time;
        while(elapsed < duration)
        {

            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        myRb.bodyType = RigidbodyType2D.Dynamic;
    }


    private void HorizontalRotation(Vector2 collisionObj)
    {
        //NEED LOTS OF POLISH!
        Vector3 leftRot = new Vector3(transform.rotation.x, transform.rotation.y, 90f);
        Vector3 rightRot = new Vector3(transform.rotation.x, transform.rotation.y, -90f);
        Vector3 upRot = new Vector3(transform.rotation.x, transform.rotation.y, 0);

        
        if (collisionObj.x > transform.position.x)
        {
            transform.DORotate(rightRot, 0.1f, RotateMode.Fast);
            //gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.localRotation, rightRot, 0.5f * Time.deltaTime);
        } else if(collisionObj.x < transform.position.x)
        {
            transform.DORotate(leftRot, 0.1f, RotateMode.Fast);
            //gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.localRotation, leftRot, 0.5f * Time.deltaTime);
        } 
    }
    public void RecallAxe(float axeSpeed, float maxDist)
    {
        myRb.bodyType = RigidbodyType2D.Dynamic;
        myRb.gravityScale = 0;
        myRb.velocity = Vector3.zero;
        Physics2D.IgnoreLayerCollision(11, 13);

        myRb.transform.position = Vector3.MoveTowards(myRb.transform.position, player.transform.position, (axeSpeed*1.5f) * Time.deltaTime);
        var distToPlayer = Vector3.Distance(myRb.transform.position, player.transform.position);
        //Debug.Break();
        if (distToPlayer < maxDist)
        {
            
            isReturning = false;
            canCallBackArm = false;
            //player.GetComponentInChildren<ThrowingMechanic>().projectileAmount = 1;
            //throwingMechanic.armIsDetached = false;
            Destroy(gameObject);
        }
    }
    public void TogglePhysics(bool isKinematic)
    {
        if(isKinematic)
        {
            myRb.bodyType = RigidbodyType2D.Kinematic;
            myRb.simulated = false;
        }
        else if(!isKinematic)
        {
            myRb.bodyType = RigidbodyType2D.Dynamic;
            myRb.simulated = true;
        }
    }
}
