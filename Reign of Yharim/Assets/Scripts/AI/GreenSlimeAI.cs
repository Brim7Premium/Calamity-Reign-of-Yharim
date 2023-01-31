using UnityEngine;

public class GreenSlimeAI : NPC
{
    public static string Name => "GreenSlime(Clone)";
    public static int Damage => 15;
    public override void SetDefaults()
    {
        lifeMax = 20;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void AI()
    {
        ai[0]++;//increment ai[0] by 1 every frame.(the framerate is capped at 60)
        velocity *= 0.95f;//this is for smoothing the movement.
        if (ai[0] == 90.0f) //if it has been 90 frames, jump.
        {
            velocity.x = GetTargetDirectionX() * 0.1f;
            velocity.y = 0.1f;
        }
        if (ai[0] == 120.0f)
        {
            velocity = Vector2.zero;//set velocity to 0.
            ai[0] = 0.0f;//reset ai[0] so we can jump again.
        }
    }
}
