using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WulfrumGyratorAI : NPC
{
    public static string Name => "WulfrumGyrator";
    public static int Damage => 15;
    public override void SetDefaults()
    {
        lifeMax = 18;
        life = lifeMax;
        //healthBar.SetMaxHealth(lifeMax);//
    }
    public override void AI()
    {
        ai[0]++;//increment ai[0] by 1 every frame.(the framerate is capped at 60)
        velocity *= 0.95f;//this is for smoothing the movement.
        if (ai[0] == 70.0f) //if it has been 90 frames, jump.
        {
            velocity.y = 0.5f;
        }
        if (ai[0] == 120.0f)
        {
            ai[0] = 0.0f;//reset ai[0] so we can jump again.
        }
    }
}
