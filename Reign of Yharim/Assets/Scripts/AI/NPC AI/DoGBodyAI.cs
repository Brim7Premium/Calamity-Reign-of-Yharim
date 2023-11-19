using UnityEngine;

public class DoGBodyAI : NPC
{
    public int SegmentIndex;
    public GameObject Head;
    public GameObject AheadSegment;
    public float SegmentSize = 2f;
    public float VelocitySmoothing;
    private float rng;

    public Vector2 oldTargetPos;
    public override void SetDefaults()
    {
        NPCName = "DevourerofGodsBody";
        damage = 442;
        lifeMax = 1706400;
        life = lifeMax;
        worm = true;

        ResetRNG();
    }
    public override void AI()
    {
        if (GameObject.Find("DevourerofGodsHead") != null)
        {
            if (Vector2.Distance(transform.position, AheadSegment.transform.position) >= SegmentSize)//if this segments positon >= the aheadsegment's position + SegmentsSize, move towards the ahead segment.
                velocity = DirectionTo(transform.position, AheadSegment.transform.position) * VelocitySmoothing;
            else
                velocity *= VelocitySmoothing;//quickly lower the segments velocity to stop it from moving into weird positions.
            if (velocity != Vector2.zero)//this code is copyed from this yt video https://www.youtube.com/watch?v=gs7y2b0xthU&t=366s and modified slightly.
            {
                Vector2 movementDirection = new(velocity.x, velocity.y);
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
            }
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }

        if (target != null)
        {
            ai[1]++;

            if (ai[0] == 0.0f) //if ai[0] equals 0 (First Attack)
            {

                if (ai[1] == 300) //after 300 frames (5 seconds)
                {
                    ai[0] = 1.0f; //set ai[0] phase to phase one
                    ai[1] = 0.0f; //reset ai[1] timer to 0
                }
            }
            if (ai[0] == 1.0f) //second phase
            {
                if (ai[1] == 0f)
                {
                    if (rng == 3)
                    {
                        Projectile telegraph = Projectile.NewProjectile(projectiles[1], transform.position, Quaternion.identity, 0, 60);

                        oldTargetPos = target.transform.position;

                        Vector3 direction = target.transform.position - telegraph.transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        telegraph.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                        telegraph.transform.localScale = new Vector3(400f, 7f, transform.localScale.z);
                    }
                }
                if (ai[1] == 60f) //after one second
                {
                    if (rng == 3)
                    {
                        Projectile deathray = Projectile.NewProjectile(projectiles[0], transform.position, Quaternion.identity, damage, 240); //create a new projectile called proj (remember class variables must equal an instance of that class. in this example, the variable equals the new projectile)

                        Vector3 direction = (Vector3)oldTargetPos - deathray.transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        deathray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                        deathray.velocity = DirectionTo(transform.position, oldTargetPos) * 0.9f; //the new new projectile will travel towards the player

                        deathray.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

                        deathray.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    }
                }
                if (ai[1] == 240f) //after four seconds
                {
                    ai[0] = 0.0f; //set ai[0] phase to phase one
                    ai[1] = 0.0f; //reset ai[1] timer to 0
                    ResetRNG();
                }
            }
        }
    }
    void ResetRNG()
    {
        rng = Random.Range(1, 6);
    }
}



