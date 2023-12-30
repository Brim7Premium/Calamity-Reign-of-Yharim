using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;
    [SerializeField] private float velPower;
    private float xAxis;
    public float isFacing;

    [Header("Jumping")]
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float jumpReleaseMod;
    private bool isJumping;
    private bool isFalling;

    [Header("Ground Detection")]
    [SerializeField] private float rayHeight;
    [SerializeField] private float rideHeight;
    [SerializeField] private float rideSpringStrength;
    [SerializeField] private float rideSpringDamper;
    private Vector2 bottomPoint;
    private bool isGrounded;

    [Header("Misc")]
    [SerializeField] private GameObject targetTestObject;

    [SerializeField] [Range(1, 180)]private int framerate; //create int with range of 1 to 180, used for setting framerate. Why this is in the player's AI  script will remain unknown for eternity

    [Header("Item usage")]
    [SerializeField] private GameObject DefaultAttackPrefab;
    public bool IsAttacking;
    public GUIController gUIController;
    //constants can't be changed
    const string PlayerIdle = "Player_idle";
    const string PlayerWalk = "Player_walk";
    const string PlayerJump = "Player_jump";
    const string PlayerRun = "Player_run";
    const string PlayerAttack = "Player_attack";


    public override void SetDefaults() //awake
    {
        base.SetDefaults(); //do whatever base npcs use as defaults plus stuff below

        NPCName = "Player";
        Damage = 0; //Note to future developers/self, this can be used for times when the player does deal contact damage to enemies. armor sets are an example. right now, it's useless.
        LifeMax = 100;
        Life = LifeMax;        

        rb.velocity = new Vector2(rb.velocity.x, Vector2.zero.y);
    }
    public override void AI() //every frame (Update)
    {
        xAxis = Input.GetAxis("Horizontal"); //sets horizontal to -1 or 1 based on the player's input

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

        bottomPoint = new Vector2(c2d.bounds.center.x, c2d.bounds.min.y); //the bottompoint variable equals the bottommost y point and center x point of the capsule collider

        //Debug.DrawRay(transform.position, ToRotationVector2(AngleTo(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition))), Color.cyan); //gets the angle to the mouse position by using AngleTo from the player's position to the mouse position converted to a world point. The output is then converted to a Vector2 so a ray can be drawn at said angle

        Debug.DrawRay(transform.position, DirectionTo(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition))); //better version
        //Debug.Log("IsJumping: " + isJumping + " IsFalling: " + isFalling);

        //Debug.Log(rb.velocity.y);

        /* if (isGrounded)
            isFalling = false;

        if (rb.velocity.y < -3f)
        {
            isFalling = true;
        }
        */
        ItemData item = gUIController.GetSelectedItem(false);
        if(Input.GetKeyDown(KeyCode.Mouse0) && item && !IsAttacking)
        {
            IsAttacking = true;
            GameObject attack = Instantiate(DefaultAttackPrefab, transform);
            attack.AddComponent(Type.GetType(item.Script)).GetComponent<Item>().item = gUIController.GetSelectedItem(item.consumable);
        }
    }
    public override void Kill()
    {
        gameObject.SetActive(false); //deactivate the object
        GameObject.Find("WorldManager").SendMessage("Respawn"); //tell the worldmanager to respawn the player
    }
    private void Movement()
    {
        float targetSpeed = xAxis * moveSpeed;

        float speedDif = targetSpeed - rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deacceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);

        animator.speed = Mathf.Abs(targetSpeed / 9);

        if (isGrounded) //if the player is grounded and isn't attacking
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
        if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, 1); //facing right
            isFacing = 1;
        }
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, 1); //facing left
            isFacing = -1;
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
        
        Application.targetFrameRate = framerate; //target framerate equals the set number
        #region GroundDetection
        Color rayCol;
        RaycastHit2D hit = Physics2D.Raycast(bottomPoint, Vector2.down, rayHeight, groundLayer);

        if (hit)
        {
            if (!isJumping)
            {
                float rayDirVel = Vector2.Dot(Vector2.down, rb.velocity); //math stuff
                float otherDirVel = Vector2.Dot(Vector2.down, Vector2.zero);
                float relVel = rayDirVel - otherDirVel;
                float x = hit.distance - rideHeight;
                float force = (x * rideSpringStrength) - (relVel * rideSpringDamper);

                rb.AddForce(Vector2.down * force);
            }

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
    private IEnumerator JumpWithDelay() //this entire thing just triggers jump and waits until the player is falling to change the variable
    {
        Jump(); //trigger the jump method

        while (rb.velocity.y > 0) //while the player is still going up
        {
            yield return null; //return null (creates a loop until rb.velocity is less than or equal to zero
        }

        isJumping = false; //set isjumping to false (doesn't trigger until the above loop is done)
    }
}
