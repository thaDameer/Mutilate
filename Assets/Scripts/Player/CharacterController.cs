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
    public int maxJump = 2;
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
    public float MovementSpeed()
    {
        if(characterStates == CharacterStates.Jumping)
        {
            var speed = moveSpeed;
            speed = Mathf.Lerp(moveSpeed, 10, 2f * Time.deltaTime);
            return 10;
        } else
        {
            return moveSpeed;
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(footPos.position, checkRadius, whatIsGround);
        jumpVector.y = jumpForce;
        RotateCharacter();
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if(isGrounded)
        {
            jumpCount=0;
            body.material = standardMat;
            moveSpeed = 13f;
        }

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
                if(Input.GetButtonDown("Fire1") && jumpCount < maxJump )
                {
                StopCoroutine(Jump());
                StartCoroutine(Jump());
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

                if(Input.GetButtonDown("Fire1") && jumpCount < maxJump )
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
                //myRb.velocity = new Vector2(moveInput * MovementSpeed(), myRb.velocity.y);
                break;
            case CharacterStates.Jumping:
                myRb.velocity = new Vector2(moveInput * moveSpeed, myRb.velocity.y);
                 //myRb.velocity = new Vector2(moveInput * MovementSpeed(), myRb.velocity.y);
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
        characterStates = CharacterStates.Jumping;
        jumpCount++;
        if(jumpCount == maxJump)
        {
            PartyManager.partyInstance.InstantiateSmoke(footPos.position);
            body.material = bodyDepleted;
        }
        
        myRb.velocity = Vector2.zero;
        float timer = 0; 
        var storedSpeed = this.moveSpeed;
        while(Input.GetButton("Fire1") && timer < jumpTime)
        {
            animator.SetBool("isJumping", true);
            myRb.gravityScale = 1;
            float jumpPercent = timer/ jumpTime;
            Vector2 thisJumpVector = Vector2.Lerp(jumpVector, Vector2.zero,jumpPercent);
            moveSpeed = Mathf.Lerp(moveSpeed, 12f, jumpPercent);
            float thisForce = Mathf.Lerp(jumpForce, 0, jumpPercent);
           // thisJumpVector.x = myRb.velocity.x;
            myRb.AddForce(Vector2.up * thisForce, ForceMode2D.Force);
            
            //myRb.velocity = thisJumpVector;
            myRb.velocity = Vector2.up * thisForce;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        moveSpeed = storedSpeed;
        myRb.gravityScale = gravForce;
        animator.SetBool("isJumping", false);
        isJumping = false;
       
        characterStates = CharacterStates.Running;
    }
}
