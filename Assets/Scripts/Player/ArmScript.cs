﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmScript : MonoBehaviour
{
    public enum ArmBehaviour
    {
        recall,
        pickup
    }
    public ArmBehaviour armBehaviour;
    float countdownToLayerswitch = .3f;
    [HideInInspector]
    public Rigidbody2D myRb;
    [HideInInspector]
    public bool isReturning;
    private Quaternion armStartRot;
    private Transform player;
    private float returnSpeed = 10f;
    ThrowingMechanic throwingMechanic;
    public BoxCollider2D triggerCol;
    public bool canCallBackArm = false;
    public bool armIsReturning = false;
    public LayerMask enemyLayer;
    public float maxDetectionRadius;
    Collider2D[] colliderResult = new Collider2D[10];
    ContactFilter2D enemy;

    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
//        player = GameObject.Find("Player").transform;
    }
    private void OnEnable() 
    {
        if(armBehaviour == ArmBehaviour.pickup)
        {
           // TriggerDelay(1f);
        }    
    }
    private void Start() 
    {
        armStartRot = gameObject.transform.rotation;
        triggerCol.gameObject.SetActive(true);
        StartCoroutine(TriggerDelayAndShrinkBack(1f));
    }
    
    IEnumerator TriggerDelayAndShrinkBack(float time)
    {
        float elapsed = 0;
        float duration = time;
        Vector3 prevScale = gameObject.transform.localScale;
        
        while(elapsed < duration)
        {
            //SHRINKS ARM BACK TO START SIZE
            gameObject.transform.localScale = Vector3.Lerp(prevScale, Vector3.one,elapsed);
            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        triggerCol.isTrigger = true;
    }

    IEnumerator HitSlowmotion(float time, float timeScale)
    {
        float scale = timeScale;
        float elapsed = 0;
        float duration = time;
        while(elapsed < duration)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            elapsed = Mathf.Min(duration, elapsed + Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
        triggerCol.isTrigger = true;
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
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isReturning = false;
            canCallBackArm = false;
            Destroy(gameObject);
        }
       
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            StartCoroutine(HitSlowmotion(0.5f, 0.4f));
        }    
    }
/*
        public void CheckForEnemyInRange()
        {   
            ContactFilter2D cont; 
            if(Physics2D.CircleCast(transform.position,5,transform.forward,0,enemyLayer))
            {
                
            }
            else
            {
            
            }
        }*/
}
