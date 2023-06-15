using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    private float xAxis;

    private float moveSpeed = 8f;

    private float jumpingPower = 16f;

    private bool isGrounded = false;

    private bool jumpPressed;

    private bool jumpReleased;

    private bool attackPressed;

    private bool isAttacking;

    [SerializeField] private float attackDelay = 0.3f;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float jumpReleaseMod = 2;

    private string currentAnimationState;

    public Animator playerAnimator;

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
        playerAnimator = GetComponent<Animator>(); //playerAnimator variable equals the animator component of the player

    }
    public override void AI() //every frame (Update)
    {
        Physics2D.IgnoreLayerCollision(10, 3); //Layer 10 (WalkThroughNPCSPlayer) will ignore collisions with layer 3 (NPCS) the child gameobjects don't use layer 10, so they can still detect collisions

        xAxis = Input.GetAxisRaw("Horizontal"); //sets horizontal to -1 or 1 based on the player's input

        if (Input.GetButtonDown("Jump")) //if the jump button is pressed
        {
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

        if (jumpPressed && isGrounded) //if the jump is pressed and the player is touching the ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower); //change the y velocty to jumpingPower
            jumpPressed = false; //set jumpPressed to false
            ChangeAnimationState(PlayerJump); //set the animation to PlayerJump
        }
        if (jumpReleased && rb.velocity.y > 0) //if jump is released and y velocity is greater than 0
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/jumpReleaseMod); //change the y velocity to the current y velocty divided by jumpReleaseMod
            jumpReleased = false; //set jumpReleased to false
        }
    }

    private void FixedUpdate() //for physics
    {
        float extraHeight = 0.1f; //new float extraHeight equals 0.1
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>(); //new BoxCollider2D variable equals the boxcollider component
        Color rayColor; //new color variable rayColor 

        RaycastHit2D hit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + extraHeight, groundLayer); //new raycast2d called hit that starts from the center of the player rigidbody, goes down, and goes the extent of the rigidbody downwards + extraHeight. it only collides with the groundlayer variable

        //Debug.Log(hit.collider);
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
        Debug.DrawRay(boxCollider2D.bounds.center, Vector2.down * (boxCollider2D.bounds.extents.y + extraHeight), rayColor); //draw the ray

        Vector2 velocity = new Vector2(0, rb.velocity.y); //create a new vector2 variable called velocty that has the values of 0 and the current y velocity

        if (xAxis < 0) //if the xAxis variable is less than 0
        {
            velocity.x = -moveSpeed; //the velocity equals negative speed
            transform.localScale = new Vector2(-1, 1); //reverse the direction of the player
        }
        else if (xAxis > 0) //if the xAxis variable is greater than 0
        {
            velocity.x = moveSpeed; //the velocity equals speed
            transform.localScale = new Vector2(1, 1); //reverse the diretion of the player
        }
        else
        {
            velocity.x = 0; //the velocity equals 0
        }

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

        rb.velocity = velocity; //the velocity of the rigidbody equals the velocity variable

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
                attackDelay = playerAnimator.GetCurrentAnimatorStateInfo(0).length; //attackDelay variable equals the length of the animation
                Invoke("AttackComplete", attackDelay); //invoke attackcomplete after the animation is done
            }

        }
    }
    void AttackComplete()
    {
        isAttacking = false; //set isAttacking to false
    }

    void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; //if currentAnimationState equals newAnimationState, stop the method (prevents animations from interupting themselves)

        playerAnimator.Play(newAnimationState); //play the newState animation

        currentAnimationState = newAnimationState; //set currentAnimationState to newAnimationState
    }

}
