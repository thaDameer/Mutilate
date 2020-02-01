using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    private Transform player;
    public float maxDistToPlayer = 10f;
    public float moveSpeed = 5f;
    bool isMoving;
    Rigidbody2D myRB;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Animator animator;
    public bool isFlying;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        myRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Animations();
        var dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist < maxDistToPlayer)
        {
            isMoving = true;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving && player.transform.position.x < transform.position.x)
        {
            myRB.velocity = Vector2.Lerp(transform.position, -Vector2.right, moveSpeed);
            spriteRenderer.flipX = true;
        } else if(isMoving && player.transform.position.x > transform.position.x)
        {
            myRB.velocity = Vector2.Lerp(transform.position, Vector2.right, moveSpeed);
            spriteRenderer.flipX = false;
        }
    }

    void Animations()
    {
        if (isMoving)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
        }
        else if (!isMoving)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
        }
    }
}
