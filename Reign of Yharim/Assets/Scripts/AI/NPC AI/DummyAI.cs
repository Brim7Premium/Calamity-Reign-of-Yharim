using UnityEngine;
public class DummyAI : NPC
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Damage = 10;
        NPCName = "Dummy";
        LifeMax = 100;
        Life = LifeMax;
        
        target = GameObject.Find("Player");
    }
    public override void AI()
    {
        UpdateVelocity();
        if (target != null)
        {
            velocity = new Vector2(DirectionTo(transform.position, target.transform.position).x * 0.05f, velocity.y);

            if (1 == TargetDirection) //for looking at player
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            else
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
