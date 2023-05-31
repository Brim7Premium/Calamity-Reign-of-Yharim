using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : NPC //basically, this script is a copy of the npc script and all of it's values. the main differences are that each value can be overriden from the base script for the new one, and this one can be attached to gameobjects.
{
    public static string Name => "Player";
    public static int Damage => 5;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    [SerializeField] public static bool isGrounded = false;

    private string currentAnimationState;
    public Animator playerAnimator;

    //constants can't be changed
    const string PlayerIdle = "Player_idle";
    const string PlayerWalk = "Player_walk";

    public override void SetDefaults()
    {
        lifeMax = 100;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);

        playerAnimator = GetComponent<Animator>();
    }
    public override void AI()
    {
        Physics2D.IgnoreLayerCollision(10, 3);

        horizontal = Input.GetAxis("Horizontal"); //sets horizontal to -1 or 1 based on the player's input

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); //sets the speed of the player along the x coordinate to 1 * speed or -1 * speed, allowing the player to move horizontally based on input

        if (horizontal > 0) //right
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        else if (horizontal < 0) //left
            gameObject.transform.localScale = new Vector3(-1, 1, 1);

        if (isGrounded)
        {
            if (horizontal != 0)
                ChangeAnimationState(PlayerWalk);
            else
                ChangeAnimationState(PlayerIdle);
        }
    }

    void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; //if currentAnimationState equals newAnimationState, stop the method (prevents animations from interupting themselves)

        playerAnimator.Play(newAnimationState); //play the newState animation

        currentAnimationState = newAnimationState; //set currentAnimationState to newAnimationState
    }
    private bool IsGrounded()
    {
        return isGrounded;
    }
}
