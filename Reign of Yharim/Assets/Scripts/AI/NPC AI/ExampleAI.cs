using UnityEngine;

public class ExampleAI : NPC
{
    bool isGrounded;
    Vector2 bottomPoint;
    public override void SetDefaults()
    {
        base.SetDefaults();
        NPCName = "ExampleNPC";
        damage = 7;
        lifeMax = 100;
        life = lifeMax;

        target = GameObject.Find("Player");
    }
    //for this examplenpc, I will make a simple "state machine" for switching between different attacks.
    //ai[] usage:
    //ai[0]: the current attack state the npc is in.
    //ai[1]: this is used as a timer.
    //ai[2/3]: can be used for random things.
    public override void AI()
    {
        if (target != null)
        {
            ai[1]++;

            if (ai[0] == 0.0f)
            {
                if (ai[1] % 120 == 0 && isGrounded){
                    rb.velocity = new Vector2(TargetDirection * 2.735f, 5.47f)*1.5f;
                }

                if (ai[1] > 360.0f)
                {
                    ai[0] = 1.0f;
                    ai[1] = 0.0f;
                }
            }

            
            else if (ai[0] == 1.0f)
            {
                rb.velocity = DirectionTo(new Vector2(target.transform.position.x, target.transform.position.y + 4)) * 24;//attempt to fly above the player.

                if (ai[1] > 120.0f)
                {
                    //I don't remember how I got this but basically it shoots in player, while he moves in a straight line, at such speed that they will intersect. Kind of prediction of player movement.
                    Vector2 bulletPos = transform.position - target.transform.position;

                    Vector2 targetVel = target.GetComponent<Rigidbody2D>().velocity;

                    int speed = 15;

                    float a = speed*speed - targetVel.sqrMagnitude;

                    float b = targetVel.x*bulletPos.x+targetVel.y*bulletPos.y;

                    float c = -bulletPos.sqrMagnitude;

                    float D = b*b - a*c;

                    if(D<0){
                        D = 0;
                    }

                    float z = (-b-Mathf.Sqrt(D))/a;

                    if(z<0){
                    
                        z = (-b+Mathf.Sqrt(D))/a;

                        if(z<0){
                            z = 1;
                        }
                    }

                    float velX = (float)(-bulletPos.x/z+targetVel.x);

                    float velY = (float)(-bulletPos.y/z+targetVel.y);


                    Vector2 _velocity = new Vector2(velX, velY);
                    int _damage = 20;
                    float _knockback = 0;
                    int _timeLeft = 4;

                    Projectile proj = Projectile.NewProjectile(projectiles[0], transform.position, Quaternion.identity, _velocity, _damage, _knockback, _timeLeft);

                    ai[0] = 0.0f;
                    ai[1] = 0.0f;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        bottomPoint = new Vector2(c2d.bounds.center.x, c2d.bounds.min.y);
        isGrounded = Physics2D.Raycast(bottomPoint, Vector2.down, 0.1f, groundLayer);
    }
}
