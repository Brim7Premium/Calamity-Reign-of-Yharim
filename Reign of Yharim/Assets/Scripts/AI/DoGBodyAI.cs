using UnityEngine;

public class DoGBodyAI : NPC
{
    public static string Name => "TheDevourerofGodsBody";
    public static int Damage => 442;
    public int SegmentIndex;
    public GameObject Head;
    public GameObject AheadSegment;
    public float SegmentSize = 2f;
    public float VelocitySmoothing;
    public override void SetDefaults()
    {
        lifeMax = 1706400;
        life = lifeMax;
    }
    public override void AI()
    {
        if (Vector2.Distance(transform.position, AheadSegment.transform.position) >= SegmentSize)
            velocity = DirectionTo(AheadSegment.transform.position) * VelocitySmoothing;
        else
            velocity *= VelocitySmoothing;
        if (velocity != Vector2.zero)
        {
            Vector2 movementDirection = new(velocity.x, velocity.y);
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
        }
    }
}
