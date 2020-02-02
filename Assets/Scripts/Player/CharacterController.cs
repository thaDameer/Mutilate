using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CharacterStates
    {
        Running, 
        Jumping,
        Idle,
    }

    public CharacterStates characterStates;

    //MOVEMENT
    [Header("Movement Variables")]
    public float moveSpeed = 10f;
    private float moveInput;
    private float verticalInput;
    //JUMPING
    [Header ("Jumping Variables")]
    public float jumpTime = 1f;
    public int jumpCount = 0;
    Vector2 jumpVector;
    public float jumpForce = 8;
    public LayerMask whatIsGround;
    public float checkRadius = 1f;
    public bool isGrounded;
    public bool isJumping;
    public float gravForce = 8f;
    public Material bodyDepleted;
    public Renderer body;
    Material standardMat;

    public Transform footPos;

    public Transform characterModel;

    //RIGIDBODY
    private Rigidbody2D myRb;

    //ANIMATIONS
    public Animator animator;
   
    void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
        characterStates = CharacterStates.Idle;
    }

    private void Start() {
        standardMat = body.material;
    }

    void Update()
    {
        Debug.Log(IsGrounded());
        //TEST
        jumpVector.y = jumpForce;
        RotateCharacter();
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        

        if(Input.GetButtonDown("Fire1") && jumpCount < 1)
        {
          StopCoroutine(Jump());
          StartCoroutine(Jump());
        }
        switch (characterStates)
        {
            case CharacterStates.Idle:
                //ANIMATIONS

                animator.SetBool("isIdle", true);
                animator.SetBool("isRunning", false);

                //CHANGE ANIMATION STATE
                if(moveInput != 0)
                {
                    characterStates = CharacterStates.Running;
                }

                
                break;
            case CharacterStates.Running:
                //ANIMATIONS
                animator.SetBool("isRunning", true);
                
                //CHANGE ANIMATION STATE
                if(moveInput == 0)
                {
                    characterStates = CharacterStates.Idle;
                }
                 if(Input.GetButtonDown("Fire1") && jumpCount < 1)
                {
                StopCoroutine(Jump());
                StartCoroutine(Jump());
                }
                break;
            case CharacterStates.Jumping:
              
                break;
        }


    }

    private void FixedUpdate()
    {
        switch (characterStates)
        {
            case CharacterStates.Idle:
                break;
            case CharacterStates.Running:
                myRb.velocity = new Vector2(moveInput * moveSpeed, myRb.velocity.y);
                break;
            case CharacterStates.Jumping:
                myRb.velocity = new Vector2(moveInput * moveSpeed, myRb.velocity.y);
                break;
        }
        
        
    }


    void RotateCharacter()
    {
        if (moveInput < 0)
        {
            var sprite = animator.GetComponent<SpriteRenderer>();
            characterModel.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveInput > 0)
        {
            characterModel.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator Jump()
    {
        jumpCount++;
        if(jumpCount > 0 && !IsGrounded())
        {
            PartyManager.partyInstance.InstantiateSmoke(footPos.position);
            body.material = bodyDepleted;
        }
        
        myRb.velocity = Vector2.zero;
        float timer = 0; 
        var downForce = new Vector2(0, -15);
        while(Input.GetButton("Fire1") && timer < jumpTime)
        {
            animator.SetBool("isJumping", true);
            myRb.gravityScale = 1;
            float jumpPercent = timer/ jumpTime;
            Vector2 thisJumpVector = Vector2.Lerp(jumpVector, Vector2.zero,jumpPercent);
            float thisForce = Mathf.Lerp(jumpForce, 0, jumpPercent);
           // thisJumpVector.x = myRb.velocity.x;
           myRb.AddForce(Vector2.up * thisForce, ForceMode2D.Force);
            
            //myRb.velocity = thisJumpVector;
            myRb.velocity = new Vector2(myRb.velocity.x, thisForce);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        myRb.gravityScale = gravForce;
        animator.SetBool("isJumping", false);
        
        isJumping = false;
    }

    public bool IsGrounded()
    {
       if(Physics2D.OverlapCircle(footPos.position, checkRadius, whatIsGround))
       {
           jumpCount = 0;
           isGrounded = true;
           body.material = standardMat;
           return true;
       }
       else
       {
           isGrounded = false;
           return false;
       } 
           
           
    }
 
}
