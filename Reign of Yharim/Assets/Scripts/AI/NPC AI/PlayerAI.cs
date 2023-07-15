using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    private float xAxis;
    private float jumpingPower = 16f;
    private float extraHeight;

    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float jumpReleaseMod = 2;
    [SerializeField] private float moveSpeed = 8f;

    private bool upIsOnSlope;
    private bool downIsOnSlope;
    private bool jumpPressed;
    private bool jumpReleased;
    private bool attackPressed;
    private bool isAttacking;
    private bool isJumping;
    private bool isGrounded;
    private bool rightGrounded;
    private bool leftGrounded;

    private string isFacing;

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
        RaycastHit2D slopeRight = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.right, 0.5f, groundLayer); //shoot a ray right from the position of the raycastFront transform. it only collides with the ground layer
        RaycastHit2D slopeLeft = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.left, 0.5f, groundLayer); //shoot a ray left from the position of the raycastFront transform. it only collides with the ground layer

        if (slopeRight && isFacing == "Right") //if the right ray is hitting something, and the player is facing right
        {
            if (slopeRight.normal.x != -1.0f) //if the object that is hit isn't a wall
            {
                upIsOnSlope = true;
                downIsOnSlope = false;
                Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), slopeRight.normal, Color.yellow); //will draw a yellow line angled away from the collision
            }

            Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.right, Color.cyan); //draw a cyan line right from the position of the raycastFront transform
        }
        else if (!slopeRight && !slopeLeft || slopeRight.normal.x == 1.0f) //if the right ray isn't hitting anything and the left ray isn't hitting anything, or the right ray is hitting a wall
        {
            upIsOnSlope = false;
        }
        if(slopeLeft && isFacing == "Left")
        {
            if (slopeLeft.normal.x != 1.0f) //if the object that is hit isn't a wall
            {
                upIsOnSlope = true;
                downIsOnSlope = false;
                Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), slopeLeft.normal, Color.magenta); //will draw a magenta line angled away from the collision
            }

            Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.left, Color.cyan); //draw a cyan line left from the position of the raycastFront transform
        }
        else if (!slopeLeft && !slopeRight || slopeLeft.normal.y == 1.0f) //if the left ray isn't hitting anything and the right ray isn't hitting anything, or the left ray is hitting a wall
        {
            upIsOnSlope = false;
        }

        //Debug.Log("Left: " + slopeLeft.normal + " Right: " + slopeRight.normal);
    }
    private void SlopeCheckDown()
    {
        RaycastHit2D slopeDown = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.down, 0.5f, groundLayer);

        if (leftGrounded == true && rightGrounded == false && isFacing == "Right") //if going down a right facing slope
        {
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.down, Color.cyan);
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), slopeDown.normal, Color.yellow);
            if (slopeDown.normal.y != 1.0f && slopeDown.normal.y != 0.0f) //if the normal doesn't equal a wall or nothing (if hitting a slope)
            {
                downIsOnSlope = true;
                upIsOnSlope = false;
            }
            else if (slopeDown.normal.y == 1.0f || slopeDown.normal.y == 0.0f)
            {
                downIsOnSlope = false;
            }
        }
        if (leftGrounded == false && rightGrounded == true && isFacing == "Left") //if going down a left facing slope
        {
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), Vector2.down, Color.cyan);
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x, bc2d.bounds.center.y - bc2d.bounds.extents.y), slopeDown.normal, Color.yellow);
            if (slopeDown.normal.y != 1.0f && slopeDown.normal.y != 0.0f) //if the normal doesn't equal a wall or nothing (if hitting a slope)
            {
                downIsOnSlope = true;
                upIsOnSlope = false;
            }
            else if (slopeDown.normal.y == 1.0f || slopeDown.normal.y == 0.0f)
            {
                downIsOnSlope = false;
            }
        }
        if (leftGrounded == true && rightGrounded == true)
        {
            downIsOnSlope = false;
        }
        Debug.Log(downIsOnSlope);
    }
    private void Movement()
    {
        Vector2 velocity = new Vector2(0, rb.velocity.y); //create a new vector2 variable called velocty that has the values of 0 and the current y velocity

        if (!upIsOnSlope) //if not on a slope
        {
            velocity.x = moveSpeed * xAxis; //move the movespeed either left or right depending on input
        }
        if (upIsOnSlope && isFacing == "Right") //if on an up slope and facing right
        {
            velocity = new Vector2(moveSpeed * xAxis, moveSpeed * xAxis); //moves the player diagonally right
        }
        if (upIsOnSlope && isFacing == "Left") //if on an up slope and facing left
        {
            velocity = new Vector2(moveSpeed * xAxis, moveSpeed * -xAxis); //moves the player diagonally left
        }
        if (downIsOnSlope && isFacing == "Right")
        {
            velocity = new Vector2(moveSpeed * xAxis, moveSpeed * -xAxis);
        }
        if (downIsOnSlope && isFacing == "Left")
        {
            velocity = new Vector2(moveSpeed * xAxis, moveSpeed * xAxis);
        }

        if (isGrounded && !upIsOnSlope && !isJumping)
        {
            velocity.y = Vector2.zero.y;
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
        /*if (Input.GetButtonDown("Jump") && isGrounded) //if the jump button is pressed and the player is touching the ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower); //change the y velocty to jumpingPower
            isJumping = true;
        }
        if (Input.GetButtonUp("Jump") && !isGrounded) //if the jump button is released and the player isn't touching the ground
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod); //change the y velocity to the current y velocty divided by jumpReleaseMod
            isJumping = false;
        }

         if (jumpPressed && isGrounded) //if the jump is pressed and the player is touching the ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower); //change the y velocty to jumpingPower
            jumpPressed = false; //jumppressed is instantly reset when the player presses the jump
            isJumping = true;
        }
        if (jumpReleased && isGrounded) //if jump is released and the player hasn't landed yet
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseMod); //change the y velocity to the current y velocty divided by jumpReleaseMod
            jumpReleased = false; //jumpreleased is instantly reset when the player releases the jump
            isJumping = false;
        }*/
    }
    private void FixedUpdate() //for physics
    {
        if (upIsOnSlope)
        {
            extraHeight = 0.6f; //new float extraHeight equals 0.1
        }
        else
        {
            extraHeight = 0.1f;
        }
        Color colRight; 
        Color colLeft;

        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.down, bc2d.bounds.extents.y + extraHeight, groundLayer);
        RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.down, bc2d.bounds.extents.y + extraHeight, groundLayer);

        if (rightHit) //if the right raycast is hitting something;
        {
            isGrounded = true; //isgrounded is true
            colRight = Color.blue; //the raycolor is green
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.down * (bc2d.bounds.extents.y + extraHeight), colRight);
            rightGrounded = true;
        }
        else //if the right raycast isn't hitting anything
        {
            colRight = Color.magenta; //the raycolor is red
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x + bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.down * (bc2d.bounds.extents.y + extraHeight), colRight);
            rightGrounded = false;
        }
        if (leftHit)
        {
            isGrounded = true; //isgrounded is true
            colLeft = Color.green; //the raycolor is green
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.down * (bc2d.bounds.extents.y + extraHeight), colLeft);
            leftGrounded = true;
        }
        else
        {
            colLeft = Color.red; //the raycolor is red
            Debug.DrawRay(new Vector2(bc2d.bounds.center.x - bc2d.bounds.extents.x, bc2d.bounds.center.y), Vector2.down * (bc2d.bounds.extents.y + extraHeight), colLeft);
            leftGrounded = false; //frontgrounded is false 
        }
        if (!rightHit && !leftHit)
        {
            isGrounded = false;
        }

        //Debug.Log("Left: " + leftGrounded + " Right: " + rightGrounded);


        Attacking();
        SlopeCheckUp();
        SlopeCheckDown();
        Movement();
        Jumping();
    }
    void AttackComplete()
    {
        isAttacking = false; //set isAttacking to false
    }

}
