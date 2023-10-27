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
        
        target = GameObject.Find("Player");
    }
    public override void AI()
    {
        UpdateVelocity();
        if (target != null)
        {
            velocity = new Vector2(DirectionTo(target.transform.position).x * 0.05f, velocity.y);

            if (1 == TargetDirection) //for looking at player
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            else
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
