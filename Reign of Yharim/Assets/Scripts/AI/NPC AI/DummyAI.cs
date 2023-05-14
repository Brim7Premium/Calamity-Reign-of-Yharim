using UnityEngine;
public class DummyAI : NPC
{
    public static string Name => "Dummy";
    public static int Damage => 5;
    public override void SetDefaults()
    {
        lifeMax = 100;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void AI()
    {
        velocity = DirectionTo(target.transform.position) * 0.05f;
        if (1 == GetTargetDirectionX())
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        else
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }
}
