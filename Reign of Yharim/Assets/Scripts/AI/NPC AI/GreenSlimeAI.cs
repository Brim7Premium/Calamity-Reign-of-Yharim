using UnityEngine;

public class GreenSlimeAI : NPC
{
    private bool isGrounded = false;
    private int curTargetPos;

    const string SlimeBounce = "Slime_bounce";
    const string SlimeIdle = "Slime_idle";

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "GreenSlime";
        Damage = 5;
        LifeMax = 20;
        Life = LifeMax;

        target = GameObject.Find("Player");

    }

    public override void AI()
    {
        
        if (target != null)
        {
            animator.speed = 0.8f;

            ai[1]++;
            if (ai[1] == 90.0f && isGrounded)
            {
                curTargetPos = TargetDirection;

                ChangeAnimationState(SlimeIdle);
                rb.velocity = new Vector2(curTargetPos * 2.735f, 5.47f); //Match original as much as possible
            }
            else if (ai[1] > 150.0f && isGrounded) 
            {
                ChangeAnimationState(SlimeBounce);
                rb.velocity = Vector2.zero; 
                ai[1] = 0.0f;
            }
            if (GetDistanceToPlayer() > 60f)
            {
                Destroy(gameObject);
            }
        }
        
    }
    void OnDestroy()
    {
        
    }
    private void FixedUpdate()
    {
        float extraHeight = 0.4f; 
        Color rayColor; 

        RaycastHit2D hit = Physics2D.Raycast(c2d.bounds.center, Vector2.down, c2d.bounds.extents.y + extraHeight, groundLayer); 

        //Debug.Log(hit.collider);
        if (hit.collider != null) 
        {
            isGrounded = true; 
            rayColor = Color.green; 
        }
        else
        {
            isGrounded = false; 
            rayColor = Color.red; 
        }
        Debug.DrawRay(c2d.bounds.center, Vector2.down * (c2d.bounds.extents.y + extraHeight), rayColor); 
    }
}
