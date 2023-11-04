using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;
    [SerializeField] private float velPower;
    private float xAxis;
    private float force = 0;

    [Header("Jumping")]
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float jumpReleaseMod;
    private bool isJumping;
    private bool isFalling;

    [Header("Ground Detection")]
    [SerializeField] private PolygonCollider2D leg;
    [SerializeField] private float circleRadius;
    [SerializeField] private float circleCastCorrectionValue;
    [SerializeField] private float raycastCorrectionValue;
    private bool isGrounded;
    private float facingDirection;

    [SerializeField] [Range(1, 180)]private int framerate;

    //constants can't be changed
    const string PlayerIdle = "Player_idle";
    const string PlayerWalk = "Player_walk";
    const string PlayerJump = "Player_jump";
    const string PlayerRun = "Player_run";
    const string PlayerAttack = "Player_attack";


    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "Player";
        damage = 0; //Note to future developers/self, this can be used for times when the player does deal contact damage to enemies. armor sets are an example. right now, it's useless.
        lifeMax = 100;
        life = lifeMax;
 
        leg = GameObject.Find("LeftLeg").GetComponent<PolygonCollider2D>();
        

        rb.velocity = new Vector2(rb.velocity.x, Vector2.zero.y);
    }
    public override void AI() //every frame (Update)
    {
        xAxis = Mathf.SmoothDamp(xAxis, Input.GetAxis("Horizontal"), ref xAxis, acceleration); //sets horizontal to -1 or 1 based on the player's input

        if (Input.GetButtonDown("Jump")) //if the jump button is being pressed...
        {
            if (isGrounded) //and the player is grounded...
            {
                StartCoroutine(JumpWithDelay()); //start the JumpWithDelay coroutine
            }
        }

        if (Input.GetButtonUp("Jump")) //if the jump button is released...
        {
            OnJumpUp(); //trigger the OnJumpUp method
        }

        //Debug.Log("IsJumping: " + isJumping + " IsFalling: " + isFalling);

        //Debug.Log(rb.velocity.y);

        /* if (isGrounded)
            isFalling = false;

        if (rb.velocity.y < -3f)
        {
            isFalling = true;
        }
        */
    }
    public void OnDisable()
    {
        GameObject.Find("WorldManager").SendMessage("Respawn"); //tell the worldmanager to respawn the player
    }
    private void Movement()
    {
        Bounds bounds = c2d.bounds;

        RaycastHit2D cast = Physics2D.CircleCast(bounds.center, circleRadius, Vector2.down, bounds.extents.y + circleCastCorrectionValue, groundLayer);

        isGrounded = Physics2D.Raycast(bounds.center, Vector2.down, bounds.extents.y + raycastCorrectionValue, groundLayer);

        float targetSpeed = xAxis * moveSpeed;

        Debug.DrawRay(cast.point, cast.normal);

        if(Mathf.Abs(cast.normal.y) > 0.01f && isGrounded && !isJumping )
        {    
            Vector2 normal = cast.normal;

            rb.velocity = targetSpeed * -Vector2.Perpendicular(normal);

            rb.AddForce(2.5f * 9.81f * -normal);
        }
        else
        {
            rb.velocity = new Vector2(targetSpeed, rb.velocity.y);

            rb.AddForce(2.5f * 9.81f * Vector2.down);
        }


        animator.speed = Mathf.Abs(targetSpeed / 10);

        if (isGrounded) //if the player is grounded and isn't attacking
        {
            if (Mathf.Abs(xAxis) > 0.01f) //if the player isn't still
            {
                ChangeAnimationState(PlayerWalk); //set the animation to walking
            }
            else
            {
                ChangeAnimationState(PlayerIdle); //set the animation to idle
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
    private void Jump()
    {
        isJumping = true; //set isjumping to true

        rb.velocity = new Vector2 (rb.velocity.x, Vector2.zero.y);

        rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse); //the jumping script
    }
    public void OnJumpUp()
    {
        if (isJumping) //if the player is jumping
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpReleaseMod), ForceMode2D.Impulse); //apply downward force to cut the player's jump
        }
    }
    private void FixedUpdate() //for physics
    {
        
        Application.targetFrameRate = framerate;

        Movement();
    }
    private IEnumerator JumpWithDelay() //this entire thing just triggers jump and waits until the player is falling to change the variable
    {
        Jump(); //trigger the jump method

        while (rb.velocity.y > 0) //while the player is still going up
        {
            yield return null; //return null (creates a loop until rb.velocity is less than ot equal to zero
        }

        isJumping = false; //set isjumping to false (doesn't trigger until the above loop is done)
    }
}
