using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WulfrumHovercraftAI : NPC
{
    private bool spotted;
    private Color color;

    private float Speed = 6;
    private string CurrentAIState = "Positioning";
    private float DiveStrength = 0;
    private Vector2 Offsetter = new Vector2(12, 7);
    private Vector2 Diver = new Vector2(0, 0);
    float OffsetterSpecialY = 7;

    const string Normal = "WulfrumHovercraft_basic", ChargedState = "WulfrumHovercraft_charged";

    //int SearchLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("WalkTroughNPCPlayer"));

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "WulfrumHovercraft";
        Damage = 15;
        LifeMax = 20;
        Life = LifeMax;
        target = GameObject.Find("Player");

        spotted = false;
        IsWulfrumGuy = true;

    }

    public void ChangeAIState(string NewAIState)
    {
        if (CurrentAIState == NewAIState) return;
        CurrentAIState = NewAIState;
    }

    public override void AI()
    {
        if (ai[1] > 0)
        {
            ChangeAnimationState(ChargedState);
            ai[1] -= Time.deltaTime;
            Speed = 7;
            OffsetterSpecialY = 10;
        }
        else
        {
            ChangeAnimationState(Normal);
            Speed = 5;
            OffsetterSpecialY = 7;
        }

        if (target != null)
        {
            switch (CurrentAIState)
            {
                case "Positioning":

                    rb.velocity = Vector3.Lerp(rb.velocity, GetDirection(target.transform.position + new Vector3(Offsetter.x, Offsetter.y, 0), transform.position) * Speed, 0.1f);
                    if (Vector2.Distance(target.transform.position + new Vector3(Offsetter.x, Offsetter.y, 0), transform.position) <= 0.3f)
                    {
                        Offsetter = Offsetter * new Vector2(-1, 1);
                        ChangeAIState("Prepare");
                    }

                    break;

                case "Prepare":

                    rb.velocity = Vector3.Lerp(rb.velocity, GetDirection(target.transform.position + new Vector3(Offsetter.x, Offsetter.y, 0), transform.position) * Speed, 0.09f);
                    if (Vector2.Distance(target.transform.position + new Vector3(Offsetter.x, Offsetter.y, 0), transform.position) <= 0.3f)
                    {
                        Offsetter = new Vector2(Offsetter.x * -2, Offsetter.y) + new Vector2(target.transform.position.x, target.transform.position.y);
                        Diver = new Vector2(Offsetter.x, Offsetter.y - 60);
                        DiveStrength = 0;
                        ChangeAIState("Dive");
                    }

                    break;

                case "Dive":

                    rb.velocity = Vector3.Lerp(rb.velocity, GetDirection(Diver, transform.position) * Speed, (0.3f + (DiveStrength * 0.1f)));
                    rb.velocity += new Vector2((rb.velocity.x < 0 ? -DiveStrength : DiveStrength) * 2.5f, 0);
                    Diver += new Vector2(0, 1 + (Time.deltaTime * DiveStrength * 10));
                    DiveStrength += Time.deltaTime;
                    if (Offsetter.y - 1 < transform.position.y && DiveStrength >= 1)
                    {
                        Offsetter = new Vector2(12 * (target.transform.position.x < transform.position.x ? 1 : -1), OffsetterSpecialY);
                        ChangeAIState("Positioning");
                    }

                    break;

                    /*case "Appear":

                        rb.velocity += new Vector2(0, Time.deltaTime * 2);
                        RaycastHit2D hit = Physics2D.Raycast(c2d.bounds.center, GetDirection(target.transform.position, transform.position), SightDistance, SearchLayer);
                        Debug.DrawRay(c2d.bounds.center, GetDirection(target.transform.position, transform.position) * SightDistance, Color.red);
                        //Debug.Log(hit.collider.gameObject.name);
                        if (hit.collider.gameObject != null)
                        {
                            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("WalkTroughNPCPlayer"))
                            {
                                Offsetter = new Vector2(12 * (target.transform.position.x < transform.position.x ? 1 : -1), 10);
                                ChangeAIState("Positioning");
                            }
                        }

                        break;*/ //Couldn't fix the ray, basically, after spawn the drone just flies upwards, if the ray points right at the player with no obstacles, he starts positioning and doing the rest of his things
            }

            rb.velocity = new Vector2(rb.velocity.x * 0.97f, rb.velocity.y);

            sprite.flipX = (rb.velocity.x < 0);

            transform.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -1.25f);

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
}
