using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    private float xAxis;
    private float jumpingPower = 16f;
    private float hitAngle;

    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float jumpReleaseMod = 2;
    [SerializeField] private float moveSpeed = 8f;

    private bool isGrounded = false;
    private bool isOnSlope;
    private bool oldIsOnSlope;
    private bool jumpPressed;
    private bool jumpReleased;
    private bool attackPressed;
    private bool isAttacking;

    private string isFacing;
    private string slopeStatus;


    [SerializeField] private Transform raycastFront;

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
        bc2d = GetComponent<BoxCollider2D>();
    }

    public override void AI() //every frame (Update)
    {
        Physics2D.IgnoreLayerCollision(10, 3); //Layer 10 (WalkThroughNPCSPlayer) will ignore collisions with layer 3 (NPCS) the child gameobjects don't use layer 10, so they can still detect collisions

        xAxis = Input.GetAxisRaw("Horizontal"); //sets horizontal to -1 or 1 based on the player's input

        if (Input.GetButtonDown("Jump")) //if the jump button is pressed
        {
            if (isGrounded)
                jumpPressed = true; //set the bool jumpPressed to true
        }
        if (Input.GetButtonUp("Jump")) //if the jump button is released
        {
            jumpReleased = true; //set the bool jumpReleased to true
        }
        if (Input.GetButtonDown("Fire1")) //if the Fire1 button is pressed
        {
            attackPressed = true; //set the bool attackPressed to true
        }
    }

    public override void OnKill()
    {
        GameObject.Find("WorldManager").SendMessage("Respawn"); //tell the worldmanager to respawn the player
    }

    private void SlopeCheckUp()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(raycastFront.position, Vector2.down, 0.5f, groundLayer); //shoot a ray down from the position of the raycastFront transform. it only collides with the ground layer

        oldIsOnSlope = isOnSlope; //set oldisonslope to isonslope after everything is done

        if (isFacing == "Right") //facing right
        {
            Debug.DrawRay(raycastFront.position, raycastHit.normal, Color.yellow); //will draw a yellow line angled away from the collision
        }
        if (isFacing == "Left")//facing left
        {
            Debug.DrawRay(raycastFront.position, raycastHit.normal, Color.magenta); //will draw a magenta line angled away from the collision
        }

        if (raycastHit) //if the ray hits
        {
            Debug.DrawRay(raycastFront.position, Vector2.down, Color.cyan); //draw a cyan line down from the transform position
            hitAngle = raycastHit.normal.y;
            isOnSlope = true; //mark isonslope as true
        }
        else //if the slope doesn't hit 
        { 
            hitAngle = raycastHit.normal.y;
            isOnSlope = false; //mark isonslope as false
        }
        //Debug.Log(isOnSlope);
        if (isOnSlope == false && oldIsOnSlope == true && hitAngle != 1.0f)
        {
            slopeStatus = "Exiting";
        }
        else if (isOnSlope == true && oldIsOnSlope == false && hitAngle != 1.0f)
        {
            slopeStatus = "Entering";
        }
        else if (isOnSlope == false && oldIsOnSlope == false)
        {
            slopeStatus = "None";
        }

        Debug.Log(hitAngle);
    }

    private void Movement()
    {
        Vector2 velocity = new Vector2(0, rb.velocity.y); //create a new vector2 variable called velocty that has the values of 0 and the current y velocity

        if (!isOnSlope) //if not on a slope
        {
            velocity.x = moveSpeed * xAxis; //move the movespeed either left or right depending on input
        }
        else if (isOnSlope && isFacing == "Right") //if on a slope and facing right
        {
            velocity = new Vector2(moveSpeed * xAxis, moveSpeed * xAxis); //moves the player diagonally right
        }
        else if (isOnSlope && isFacing == "Left") //if on a slope and facing left
        {
            velocity = new Vector2(moveSpeed * xAxis, moveSpeed * -xAxis); //moves the player diagonally left
        }

        if (slopeStatus == "Exiting")
        {
            velocity = Vector2.zero;
        }

        if (xAxis == 1)
        {
            isFacing = "Right";
            transform.localScale = new Vector2(1, 1); //facing right
        }
        if (xAxis == -1)
        {
            isFacing = "Left";
            transform.localScale = new Vector2(-1, 1); //facing left
        }

        rb.velocity = velocity; //the velocity of the rigidbody equals the velocity variable

        if (isGrounded && !isAttacking) //if the player is grounded and isn't attacking
        {
            if (xAxis != 0) //if the player isn't still
            {
                ChangeAnimationState(PlayerWalk); //set the animation to walking
            }
            else
            {
                ChangeAnimationState(PlayerIdle); //set the animation to idle
            }
        }

    }
    private void Attacking()
    {
        if (attackPressed) //if the attack is pressed
        {
            attackPressed = false; //set attackPressed to false
            if (!isAttacking) //if isAttacking isn't true
            {
                isAttacking = true; //set isAttacking to true
                if (isGrounded) //if the player is grounded
                {
                    ChangeAnimationState(PlayerAttack); //set the animation to PlayerAttack
                }
                attackDelay = animator.GetCurrentAnimatorStateInfo(0).length; //attackDelay variable equals the length of the animation
                Invoke("AttackComplete", attackDelay); //invoke attackcomplete after the animation is done
            }
        }
    }

    private void Jumping()
    {
        if (jumpPressed && isGrounded) //if the jump is pressed and the player is touching the ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower); //change the y velocty to jumpingPower
            jumpPressed = false; //set jumpPressed to false
            ChangeAnimationState(PlayerJump); //set the animation to PlayerJump
        }
        if (jumpReleased && rb.velocity.y > 0) //if jump is released and y velocity is greater than 0
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod); //change the y velocity to the current y velocty divided by jumpReleaseMod
            jumpReleased = false; //set jumpReleased to false
        }
    }

    private void FixedUpdate() //for physics
    {
        float extraHeight = 0.2f; //new float extraHeight equals 0.1
        Color rayColor; //new color variable rayColor 

        RaycastHit2D hit = Physics2D.Raycast(bc2d.bounds.center, Vector2.down, bc2d.bounds.extents.y + extraHeight, groundLayer); //new raycast2d called hit that starts from the center of the player rigidbody, goes down, and goes the extent of the rigidbody downwards + extraHeight. it only collides with the groundlayer variable

        if (hit.collider != null) //if the raycast is hitting something;
        {
            isGrounded = true; //isgrounded is true
            rayColor = Color.green; //the raycolor is green
        }
        else
        {
            isGrounded = false; //isgrounded is false
            rayColor = Color.red; //the raycolor is red
        }
        Debug.DrawRay(bc2d.bounds.center, Vector2.down * (bc2d.bounds.extents.y + extraHeight), rayColor); //draw the ray 

        Attacking();
        SlopeCheckUp();
        Movement();
        Jumping();
    }
    void AttackComplete()
    {
        isAttacking = false; //set isAttacking to false
    }
}
