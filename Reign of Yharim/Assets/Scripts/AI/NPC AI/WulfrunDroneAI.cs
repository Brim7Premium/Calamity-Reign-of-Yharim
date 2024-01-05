using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WulfrunDroneAI : NPC
{
    private bool spotted;
    private Color color;

    private float Speed = 5;
    private Vector2 Offsetter = new Vector2(10, 5);

    const float SightDistance = 13;

    const string Normal = "WulfrumDrone_basic";
    const string ChargedState = "WulfrumDrone_charged";
    string CurrentAIState = "Positioning";

    //int SearchLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("WalkTroughNPCPlayer"));

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPCName = "WulfrumDrone";
        Damage = 16;
        LifeMax = 21;
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
            Speed = 6;
        }
        else
        {
            ChangeAnimationState(Normal);
            Speed = 5;
        }

        if (target != null)
        {
                switch (CurrentAIState)
                {
                    case "Positioning":

                        rb.velocity = Vector3.Lerp(rb.velocity, GetDirection(target.transform.position + new Vector3(Offsetter.x, Offsetter.y, 0), transform.position) * Speed, 0.1f);
                        if (Vector2.Distance(target.transform.position + new Vector3(Offsetter.x, Offsetter.y, 0), transform.position) <= 0.5f)
                        {
                            Offsetter = target.transform.position - new Vector3(0.4f * (target.transform.position.x < transform.position.x ? 1 : -1), 0, 0); // reusing Offsetter Vector as a dash point
                            ChangeAIState("Assault");
                        }

                        break;

                    case "Assault":

                        rb.velocity = Vector3.Lerp(rb.velocity, GetDirection(Offsetter, transform.position) * Speed, 0.1f);
                        if (Vector2.Distance(Offsetter, transform.position) <= 0.4f)
                        {
                            Offsetter = new Vector2(10 * (target.transform.position.x < transform.position.x ? 1 : -1), 5);
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
                                    Offsetter = new Vector2(10 * (target.transform.position.x < transform.position.x ? 1 : -1), 5);
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
