using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class GreenSlimeAI : NPC
{
    const string SlimeBounce = "Slime_bounce";
    const string SlimeIdle = "Slime_idle";

    private bool LeftRay = false;
    private bool RightRay = false;

    private float Srotation = 0;
    private float NeededRotation = 0;

    public override void SetDefaults()
    {
        base.SetDefaults();

		NPCName = "GreenSlime";
		Damage = 5;
		LifeMax = 20;
		Life = LifeMax;
	}


    public override void AI()
    {
        if (target != null)
        {
            animator.speed =  0.4f + (2f * ai[1]); // speed of Slime's "preparing to jump" animation

            ai[1] += Time.deltaTime; //counting

            if (ai[1] >= 1f && ai[1] <= 1.12f && IsGrounded || inWater)
            {
                ChangeAnimationState(SlimeIdle);
                //Jump
                if (!inWater)
                    rb.velocity = new Vector2(TargetDirection * 6, 10);
                else
                    rb.velocity = new Vector2(TargetDirection * 5, 8);
            }

            if (IsGrounded)
            {
                rb.velocity *= new Vector2(0.89f, 1); // stops it from sliding too much

                if (Mathf.Abs(rb.velocity.y) <= 0.01f && ai[1] > 1.5f) // Completely vertically stopped, can refresh the cycle
                {
                    ChangeAnimationState(SlimeBounce);
                    ai[1] = 0;
                }
            }
            if (DistanceBetween(transform.position, target.transform.position) > 60f)
            {
                Destroy(gameObject);
            }
        }

        // Slime's alligning with ground
        if (LeftRay && !RightRay) Srotation = 45;
        else if (!LeftRay && RightRay) Srotation = -45;
        else Srotation = 0;

        if (Srotation != 0) NeededRotation += (Srotation - transform.rotation.z) * 0.2f;
        else NeededRotation += (Srotation - transform.rotation.z) * 6;
        if (NeededRotation > 45) NeededRotation = 45;
        if (NeededRotation < -45) NeededRotation = -45;
        transform.rotation = Quaternion.Euler(0, 0, NeededRotation);

    }
    void OnDestroy()
    {
        
    }
    void FixedUpdate()
    {
        float Height = 0.85f; //new float extraHeight equals 0.1
        Vector2 LookAt = new Vector2(1, -1);
        RaycastHit2D hitL = Physics2D.Raycast(c2d.bounds.center, LookAt, Height, groundLayer);
        RaycastHit2D hitR = Physics2D.Raycast(c2d.bounds.center, LookAt * new Vector2(-1, 1), Height, groundLayer);

        LeftRay = (hitL.collider != null);
        RightRay = (hitR.collider != null);

        Debug.DrawRay(c2d.bounds.center, LookAt * Height, Color.white);
        Debug.DrawRay(c2d.bounds.center, (LookAt * new Vector2(-1, 1)) * Height, Color.white);
    }
}

