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
        worm = true;
    }
    public override void AI()
    {
        if (Vector2.Distance(transform.position, AheadSegment.transform.position) >= SegmentSize)//if this segments positon >= the aheadsegment's position + SegmentsSize, move towards the ahead segment.
            velocity = DirectionTo(AheadSegment.transform.position) * VelocitySmoothing;
        else
            velocity *= VelocitySmoothing;//quickly lower the segments velocity to stop it from moving into weird positions.
        if (velocity != Vector2.zero)//this code is copyed from this yt video https://www.youtube.com/watch?v=gs7y2b0xthU&t=366s and modified slightly.
        {
            Vector2 movementDirection = new(velocity.x, velocity.y);
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
        }
    }
}
