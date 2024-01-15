using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WulfrumGyratorAI : NPC
{
    private bool spotted;
    private Color color;
    private float AFKtimer = 0, JumpCooldown = 0;

    const string GyratorSpin = "WulfrumGyrator_spin", ChargeGyratorSpin = "WulfrumGyrator_chargespin";
    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "WulfrumGyrator";
        Damage = 15;
        LifeMax = 18;
        Life = LifeMax;

        spotted = false;
        IsWulfrumGuy = true;

    }
    public override void AI()
    {
        animator.speed = (Mathf.Abs(rb.velocity.x) / 5f); // animate faster based on NPCs speed

        if (target != null)
        {
            if (ai[1] > 0)
            {
                ChangeAnimationState(ChargeGyratorSpin);
                ai[1] -= Time.deltaTime;
                if (JumpCooldown > 0) JumpCooldown -= Time.deltaTime;
                else if (IsGrounded && (int)(target.transform.position.y - 1.5f) > (int)(transform.position.y)) // Able to jump if charged
                {
                    rb.velocity += new Vector2(0, 18);
                    JumpCooldown = 3;
                }
            }
            else ChangeAnimationState(GyratorSpin);

            rb.velocity += (new Vector2(TargetDirection * 10f + (Mathf.Abs((transform.position.x - target.transform.position.x) * 0.4f)) * TargetDirection, 0) * Time.deltaTime); // Movement

            if (!IsVisibleFromCamera())
            {
                spotted = false;
                ai[0] = 0.0f;
            }

            if ((int)transform.position.x == (int)oldPosition.x && (int)transform.position.y == (int)oldPosition.y) AFKtimer += Time.deltaTime; //Helping Gyrator overcome slopes
            else AFKtimer = 0;

            if (AFKtimer >= 3)
            {
                rb.velocity += new Vector2(TargetDirection * 6f, 0);
                AFKtimer = 0;
            }

            DrawDistance(transform.position, target.transform.position, color);

            if (DistanceBetween(transform.position, target.transform.position) > 60f && spotted == false)
            {
                Destroy(gameObject); //despawn the object
            }
        }
    }
}
