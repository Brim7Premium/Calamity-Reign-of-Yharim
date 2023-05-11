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
    public override void SetDefaults()
    {
        lifeMax = 20;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void AI()
    {
        Physics2D.IgnoreLayerCollision(3, 6);

        //sets horizontal to -1 or 1 based on the player's input
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("Attacking", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("Attacking", false);
        }
        //sets the speed of the player along the x coordinate to 1 * speed or -1 * speed, allowing the player to move horizontally based on input
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (horizontal > 0 || horizontal < 0)
        { //left or right
            playerAnimator.SetBool("Moving", true);
        }
        else
        {
            playerAnimator.SetBool("Moving", false);
        }
        if (horizontal > 0) //right
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontal < 0) //left
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private bool IsGrounded()
    {
        return isGrounded;
    }
}
