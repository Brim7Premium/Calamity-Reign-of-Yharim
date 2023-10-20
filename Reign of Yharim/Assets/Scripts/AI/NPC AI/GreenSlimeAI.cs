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
        damage = 5;
        lifeMax = 20;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
        target = GameObject.Find("Player");

    }

    public override void AI()
    {
        
        if (target != null)
        {
            animator.speed = 0.8f;

            ai[0]++;
            if (ai[0] == 90.0f)
            {
                curTargetPos = GetTargetDirectionX();

                ChangeAnimationState(SlimeIdle);
                rb.velocity = new Vector2(curTargetPos * 3.13f, 6.26f);
            }
            else if (ai[0] > 100.0f && isGrounded) 
            {
                ChangeAnimationState(SlimeBounce);
                rb.velocity = Vector2.zero; 
                ai[0] = 0.0f;
            }
            if (GetDistanceToPlayer() > 60f)
            {
                Destroy(gameObject);
            }
        }
        
    }
    void OnDestroy()
    {
        print(targetPos);
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
