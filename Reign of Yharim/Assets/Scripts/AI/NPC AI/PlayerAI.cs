using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    [Header("Movement")]    
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] [Range(0, 1)] private float acceleration;
    [SerializeField] private float slopeDetectorLength;
    private float velY;
    private float xAxis;
    private Vector2 placeHolder = Vector2.zero;

    [Header("Jumping")]
    [SerializeField] private float jumpingSpeed = 9f;
    [SerializeField] private int jumpingTime = 8;
    [SerializeField] private float jumpReleaseMod;
    private bool isJumping;
    private bool isFalling;

    [Header("Flying")]
    [SerializeField] private float wingAcceleration;
    [SerializeField] private float wingSpeed;
    [SerializeField] private float flightTime;

    private bool isFlying;

    [Header("Ground Detection")]
    [SerializeField] private CapsuleCollider2D cc2d;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private float rayHeight;
    [SerializeField] private float rideHeight;
    [SerializeField] private float rideSpringStrength;
    [SerializeField] private float rideSpringDamper;
    private Vector2 bottomPoint;
    private bool isGrounded;
    private float facingDirection;

    //constants can't be changed
    const string PlayerIdle = "Player_idle";
    const string PlayerWalk = "Player_walk";
    const string PlayerJump = "Player_jump";
    const string PlayerRun = "Player_run";
    const string PlayerAttack = "Player_attack";


    public override void SetDefaults()
    {
        NPCName = "Player";
        damage = 0; //Note to future developers/self, this can be used for times when the player does deal contact damage to enemies. armor sets are an example. right now, it's useless.
        lifeMax = 100;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);

        rb = GetComponent<Rigidbody2D>(); //PlayerAI.rb equals the rigidbody2d of the player
        cc2d = GetComponentInChildren<CapsuleCollider2D>();
        spr = GetComponent<SpriteRenderer>();

        rb.velocity = new Vector2(rb.velocity.x, Vector2.zero.y);
    }

    IEnumerator Jump()
    {
        isJumping = true;

        for(int i = 0; i<jumpingTime; i++)//jump
        {
            if(Input.GetKey(KeyCode.Space)){rb.velocity = new Vector2(rb.velocity.x, jumpingSpeed);}

            yield return new WaitForFixedUpdate();
        }

        isJumping = false;

        isFlying = true;

        for(int i = 0; i<flightTime; i++)//flight
        {
            if(Input.GetKey(KeyCode.Space))
            {
                rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(rb.velocity.x, wingSpeed), ref placeHolder, wingAcceleration); 

                rb.AddForce(Vector2.up * 2.5f * 9.81f);//Remove gravity. It just works
            }

            else{i -= 1;}//flightTime is not spend

            if(isGrounded){break;}

            yield return new WaitForFixedUpdate();
        }

        isFlying = false;
    }

    public override void OnKill()
    {
        GameObject.Find("WorldManager").SendMessage("Respawn");
    }
    private void Movement()
    {
        
        //Upward slope
        Vector2 normal = Physics2D.Raycast(bottomPoint, new Vector2(facingDirection, 0), slopeDetectorLength, 1 << LayerMask.NameToLayer("Ground")).normal;

        if(isGrounded && !isJumping && normal.x != 0)
        {
            velY = moveSpeed * Mathf.Abs(normal.y/normal.x * rb.velocity.x/moveSpeed);
        } 

        else if(isGrounded && !isJumping)
        {
            velY = -moveSpeed;
        }

        else{velY = rb.velocity.y;}

        float targetSpeed = xAxis * moveSpeed;

        rb.velocity = new Vector2(targetSpeed,  velY);

        animator.speed = Mathf.Abs(targetSpeed / 10);

        if (isGrounded)
        {
            if (xAxis != 0) //if the player isn't still
            {
                ChangeAnimationState(PlayerWalk);
            }
            else
            {
                ChangeAnimationState(PlayerIdle);
            }
        }
        if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, 1); //facing right
            facingDirection = 1f;
        }
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, 1); //facing left
            facingDirection = -1f;
        }
    }
   
    private void FixedUpdate() //for physics
    {
        #region GroundDetection
        Color rayCol;
        RaycastHit2D hit = Physics2D.Raycast(bottomPoint, Vector2.down, rayHeight, groundLayer);

        Debug.DrawRay(bottomPoint, Vector2.down * rayHeight);

        if (hit)
        {
            rayCol = Color.green;
            isGrounded = true;
        }
        else
        {
            rayCol = Color.red;
            isGrounded = false;
        }
        #endregion

        Movement();
    }

    public override void AI() //every frame (Update)
    {

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("WalkThroughNPCPlayer"), LayerMask.NameToLayer("NPCs")); //notice that child objects don't use the same layer

        xAxis = Mathf.MoveTowards(xAxis, Input.GetAxis("Horizontal"), acceleration);

        if (Input.GetButtonDown("Jump") && !isJumping && !isFlying && isGrounded)
        {
            StartCoroutine(Jump());
        }

        bottomPoint = new Vector2(cc2d.bounds.center.x, cc2d.bounds.min.y); //the bottompoint variable equals the bottommost y point and center x point of the capsule collider

        //Debug.Log("IsJumping: " + isJumping + " IsFalling: " + isFalling);

        //Debug.Log(rb.velocity.y);

        /* if (isGrounded)
            isFalling = false;

        if (rb.velocity.y < -3f)
        {
            isFalling = true;
        }
        */


        /* if (isGrounded)
            isFalling = false;

        if (rb.velocity.y < -3f)
        {
            isFalling = true;
        }
        */


        /* if (isGrounded)
            isFalling = false;

        if (rb.velocity.y < -3f)
        {
            isFalling = true;
        }
        */
    }
}
