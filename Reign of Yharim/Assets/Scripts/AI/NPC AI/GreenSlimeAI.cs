using UnityEngine;

public class GreenSlimeAI : NPC
{
    private bool isGrounded = false;
    private int curTargetPos;

    const string SlimeBounce = "Slime_bounce";
    const string SlimeIdle = "Slime_idle";

    public override void SetDefaults()
    {
        NPCName = "GreenSlime";
        damage = 5;
        lifeMax = 20;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);

        bc2d = GetComponent<BoxCollider2D>();
    }

    public override void AI()
    {
        if (target != null)
        {
            animator.speed = 0.8f;

            ai[0]++;//increment ai[0] by 1 every frame.(the framerate is capped at 60)
            velocity *= 0.95f;//this is for smoothing the movement.
            if (ai[0] == 90.0f) //if it has been 90 frames, jump.
            {
                curTargetPos = GetTargetDirectionX(); //curent target position variable equals get target direction (-1 or 1 based on which direction the player is to the enemy)

                ChangeAnimationState(SlimeIdle); //set the animation to idle
                velocity.x = curTargetPos * 0.2f; //jump towards the player
                velocity.y = 0.2f; //at the same time jump up
            }
            if (ai[0] > 90.0f && !isGrounded) //while the slime isn't grounded
            {
                velocity.x = curTargetPos * 0.1f; //continue going horizontally
            }
            if (ai[0] > 100.0f && isGrounded) //if ai[0] is greater than 100.0f (if this wasnt here, the slime would constantly reset when touching ground) and if the slime is grounded
            {
                ChangeAnimationState(SlimeBounce); //set the animation state to bounce
                velocity = Vector2.zero; //stop the slime from moving
                ai[0] = 0.0f;//reset ai[0] so we can jump again.
            }
            if (GetDistanceToPlayer() > 60f) //if the player is too far away
            {
                Destroy(gameObject); //despawn the slime
            }
        }
        
    }
    private void FixedUpdate()
    {
        float extraHeight = 0.4f; //new float extraHeight equals 0.1
        Color rayColor; //new color variable rayColor 

        RaycastHit2D hit = Physics2D.Raycast(bc2d.bounds.center, Vector2.down, bc2d.bounds.extents.y + extraHeight, groundLayer); //new raycast2d called hit that starts from the center of the player rigidbody, goes down, and goes the extent of the rigidbody downwards + extraHeight. it only collides with the groundlayer variable

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
        Debug.DrawRay(bc2d.bounds.center, Vector2.down * (bc2d.bounds.extents.y + extraHeight), rayColor); //draw the ray 
    }
}
