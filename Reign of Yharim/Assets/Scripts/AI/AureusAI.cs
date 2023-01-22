using UnityEngine;

public class AureusAI : NPC
{
    public override void SetDefaults()
    {
        lifeMax = 20000;
        life = lifeMax;
        active = true;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void AI()
    {
        target = GameObject.Find("Player");
        if (ai[0] == 0.0f)
        {
            MoveTowards(0.1f, 0.1f);
            ai[1]++;
            if (ai[1] >= 180.0f)
            {
                ai[0] = 1.0f;
                ai[1] = 0.0f;
            }
        }
        if (ai[0] == 1.0f)
        {
            transform.position = new Vector2(target.transform.position.x, target.transform.position.y + 10f);
            ai[1]++;
            if (ai[1] >= 180.0f)
            {
                ai[0] = 0.0f;
                ai[1] = 0.0f;
            }
        }
    }
}
