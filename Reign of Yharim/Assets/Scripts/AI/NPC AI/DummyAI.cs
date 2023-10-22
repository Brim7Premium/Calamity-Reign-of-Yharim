using UnityEngine;
public class DummyAI : NPC
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        damage = 10;
        NPCName = "Dummy";
        lifeMax = 100;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
        
        Physics2D.IgnoreLayerCollision(3, 3);
        target = GameObject.Find("Player");
    }
    public override void AI()
    {
        if (target != null)
        {
            velocity = new Vector2(DirectionTo(target.transform.position).x * 0.05f, velocity.y);

            if (1 == GetTargetDirectionX()) //for looking at player
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            else
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
