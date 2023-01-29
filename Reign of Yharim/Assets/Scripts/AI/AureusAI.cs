using UnityEngine;

public class AureusAI : NPC
{
    public static string Name => "AstrumAureus";
    public static int Damage => 100;
    public override void SetDefaults()
    {
        lifeMax = 20000;
    }
    public override void AI()
    {
        if (ai[0] == 0.0f)
        {
            velocity = DirectionTo(target.transform.position) * 0.2f;
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
