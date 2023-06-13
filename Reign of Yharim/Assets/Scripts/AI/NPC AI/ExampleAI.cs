using UnityEngine;

public class ExampleAI : NPC
{
    public override void SetDefaults()//this is were we set the npcs max life, healthbar, and other variables.
    {
        NPCName = "ExampleNPC";
        damage = 7;
        lifeMax = 100;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
    }
    //for this examplenpc, I will make a simple "state machine" for switching between different attacks.
    //ai[] usage:
    //ai[0]: the current attack state the npc is in.
    //ai[1]: this is used as a timer.
    //ai[2/3]: can be used for random things.
    public override void AI()//this is were the npc's ai code goes.
    {
        if (target != null)
        {
            ai[1]++; //increment ai[1] by 1

            if (ai[0] == 0.0f) //if ai[0] equals 0 (First Attack)
            {
                if (ai[1] % 60.0f == 0.0f) //if ai[1] divided by 60's remainder equals 0 (If ai[1] equals a number that evenly divides by 60) It will essentially trigger three times (when ai[1] is 60, 120, and 180)
                    velocity = DirectionTo(target.transform.position) * 0.8f;//dash at the player every second. velocity = DirectionTo basically allows the NPC to move anywhere, and multiplying it changes the speed

                velocity *= 0.9f;//lower the velocity every frame to make the dashes smoother. It sets velocity to velocity times 0.9.

                if (ai[1] > 180.0f)//if it has been more than 3 seconds, switch phases and and reset ai[1](the timer).
                {
                    ai[0] = 1.0f; //set ai[0] phase to phase one
                    ai[1] = 0.0f; //reset ai[1] timer to 0
                }
            }
            if (ai[0] == 1.0f)//this is the second attack type.
            {
                velocity = DirectionTo(new Vector2(target.transform.position.x, target.transform.position.y + 4)) * 0.4f;//attempt to fly above the player.

                if (ai[1] > 120.0f)//if it has been more than 2 seconds, fire a projectile at the player, switch phases, and reset ai[1](the timer).
                {
                    Projectile proj = Projectile.NewProjectile(projectiles[0], transform, Quaternion.identity, 20, 240); //create a new projectile called proj (remember class variables must equal an instance of that class. in this example, the variable equals the new projectile)

                    proj.velocity = DirectionTo(target.transform.position) * 0.3f; //the new new projectile will travel towards the player

                    ai[0] = 0.0f; //set ai[0] phase to phase one
                    ai[1] = 0.0f; //reset ai[1] timer to 0
                }
            }
        }
    }
}
