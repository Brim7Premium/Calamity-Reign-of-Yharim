using UnityEngine;

public class DoGHeadAI : NPC
{
    public float MoveSpeed;
    public float RotationSpeed;
    public override void SetDefaults()
    {
        NPCName = "DevourerofGodsHead";
        damage = 600;
        lifeMax = 1706400;
        life = lifeMax;
        worm = true;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void AI()
    {
        if (target != null)
        {
            velocity = velocity.RotateTowards(AngleTo(target.transform.position), RotationSpeed, true) * MoveSpeed;//this makes the worms velocity rotate towards the player.
            if (velocity != Vector2.zero)//this code is copyed from this yt video https://www.youtube.com/watch?v=gs7y2b0xthU&t=366s and modified slightly.
            {
                Vector2 movementDirection = new(velocity.x, velocity.y);
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
            }
        }
    }
}
