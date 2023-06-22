using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WulfrumGyratorAI : NPC
{
    private bool spotted;

    const string GyratorSpin = "WulfrumGyrator_spin";
    const string ChargeGyratorSpin = "WulfrumGyrator_chargespin";
    public override void SetDefaults()
    {
        NPCName = "WulfrumGyrator";
        damage = 15;
        lifeMax = 18;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
        spotted = false;
    }
    public override void AI()
    {
        if (target != null)
        {
            animator.speed = 0.8f;
            ChangeAnimationState(GyratorSpin);
            Color color;

            ai[0]++;//increment ai[0] by 1 every frame.(the framerate is capped at 60)
            velocity *= 0.97f;//this is for smoothing the movement.

            if (GetDistanceToPlayer() > 20f && spotted == false)
            {
                color = Color.green;

                if (ai[0] == 70.0f) //if it has been 70 frames, jump.
                    velocity.y = 0.5f;

                if (ai[0] == 120.0f)
                    ai[0] = 0.0f;//reset ai[0] so we can jump again.
            }
            else
            {
                spotted = true;
                color = Color.red;
                velocity.x = DirectionTo(target.transform.position).x * 0.12f; //speed of 
            }

            if (!IsVisibleFromCamera())
            {
                spotted = false;
                ai[0] = 0.0f;
            }

            DrawDistanceToPlayer(color);

            if (GetDistanceToPlayer() > 60f && spotted == false) 
            {
                Destroy(gameObject); //despawn the object
            }
        }
    }
}
