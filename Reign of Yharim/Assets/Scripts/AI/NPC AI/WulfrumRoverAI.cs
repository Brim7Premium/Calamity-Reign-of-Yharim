using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WulfrumRoverAI : NPC
{
    private bool spotted;
    private Color color;
    [SerializeField] private Animator Shield;

    private bool LeftRay = false, RightRay = false;
    private float MaxSpeed = 12;

    const string Normal = "WulfrumRover_slide", ChargedState = "WulfrumRover_charged";
    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "WulfrumRover";
        Damage = 10;
        LifeMax = 32;
        Life = LifeMax;
        target = GameObject.Find("Player");

        spotted = false;
        IsWulfrumGuy = true;

    }
    public override void AI()
    {
        if (ai[1] > 0)
        {
            ChangeAnimationState(ChargedState);
            ai[1] -= Time.deltaTime;
            MaxSpeed = 14;
        }
        else
        {
            ChangeAnimationState(Normal);
            MaxSpeed = 12;
        }
        Shield.SetBool("Exist", (ai[1] > 0));

        if (target != null)
        {
            if (!LeftRay && !RightRay && rb.velocity.y > -3) rb.velocity = new Vector2(rb.velocity.x, -3); // Additional gravity to have him landed always
            if (Mathf.Abs(rb.velocity.x) < MaxSpeed)
            {
                rb.velocity += new Vector2(TargetDirection * 16 * Time.deltaTime, 0);
            } // Movement
            rb.velocity = new Vector2(rb.velocity.x * 0.97f, rb.velocity.y);

            sprite.flipX = (TargetDirection < 0);


            if (!IsVisibleFromCamera())
            {
                spotted = false;
                ai[0] = 0.0f;
            }

            DrawDistance(transform.position, target.transform.position, color);

            if (DistanceBetween(transform.position, target.transform.position) > 60f && spotted == false)
            {
                Destroy(gameObject); //despawn the object
            }
        }
    }

    void FixedUpdate()
    {
        float Height = 0.85f;
        Vector2 LookAt = new Vector2(1, -1);
        RaycastHit2D hitL = Physics2D.Raycast(c2d.bounds.center, LookAt, Height, groundLayer);
        RaycastHit2D hitR = Physics2D.Raycast(c2d.bounds.center, LookAt * new Vector2(-1, 1), Height, groundLayer);

        LeftRay = (hitL.collider != null);
        RightRay = (hitR.collider != null);

        Debug.DrawRay(c2d.bounds.center, LookAt * Height, Color.white);
        Debug.DrawRay(c2d.bounds.center, (LookAt * new Vector2(-1, 1)) * Height, Color.white);
    }
}
