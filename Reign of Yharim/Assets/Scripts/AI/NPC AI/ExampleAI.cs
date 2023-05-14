using UnityEngine;

public class ExampleAI : NPC
{
    public static string Name => "ExampleNPC";
    public static int Damage => 10;
    public override void SetDefaults()//this is were we set the npcs max life, healthbar, and other variables.
    {
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
        ai[1]++;//increment ai[0] by 1 every frame so we can use it as a timer.
        
        if (ai[0] == 0.0f)//the first attack.
        {
            if(ai[1] % 60.0f == 0.0f)
                velocity = DirectionTo(target.transform.position) * 0.8f;//dash at the player every second.
            
            velocity *= 0.9f;//lower the velocity every frame to make the dashes smoother.
            
            if (ai[1] > 180.0f)//if it has been more than 3 seconds, switch phases and and reset ai[1](the timer).
            {
                ai[0] = 1.0f;
                ai[1] = 0.0f;
            }
        }
        if (ai[0] == 1.0f)//this is the second attack type.
        {
            velocity = DirectionTo(new Vector2(target.transform.position.x, target.transform.position.y + 4)) * 0.4f;//attempt to fly above the player.
            
            if (ai[1] > 120.0f)//if it has been more than 2 seconds, switch phases and and reset ai[1](the timer).
            {
                ai[0] = 0.0f;
                ai[1] = 0.0f;
            }
        }
    }
}
