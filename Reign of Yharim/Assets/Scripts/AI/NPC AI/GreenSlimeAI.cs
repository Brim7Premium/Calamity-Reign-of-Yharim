using UnityEngine;

public class GreenSlimeAI : NPC
{
    const string SlimeBounce = "Slime_bounce";
    const string SlimeIdle = "Slime_idle";

    public override void SetDefaults()
    {
        NPCName = "GreenSlime";
        damage = 5;
        lifeMax = 20;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
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
                ChangeAnimationState(SlimeIdle);
                velocity.x = GetTargetDirectionX() * 0.2f;
                velocity.y = 0.2f;
            }
            if (ai[0] == 120.0f)
            {
                ChangeAnimationState(SlimeBounce);
                velocity = Vector2.zero;//set velocity to 0.
                ai[0] = 0.0f;//reset ai[0] so we can jump again.
            }
            if (GetDistanceToPlayer() > 60f)
            {
                Destroy(gameObject); //despawn the object
            }
        }
    }
}
