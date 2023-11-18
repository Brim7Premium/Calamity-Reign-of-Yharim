using UnityEngine;
using FMOD.Studio;

public class DoGHeadAI : NPC
{
    public float MoveSpeed;
    public float RotationSpeed;

    public Vector2 oldTargetPos;

    private EventInstance DoGMusic;
    public override void SetDefaults()
    {
        NPCName = "DevourerofGodsHead";
        damage = 600;
        lifeMax = 1706400;
        life = lifeMax;
        worm = true;
        healthBar.SetMaxHealth(lifeMax);

        DoGMusic = AudioManager.instance.CreateEventInstance(FMODEvents.instance.DoGMusic);
    }
    public override void AI()
    {
        PLAYBACK_STATE playbackState;
        DoGMusic.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            DoGMusic.start();
        }

        if (target != null)
        {
            ai[1]++;

            if (ai[0] == 0.0f) //if ai[0] equals 0 (First Attack)
            {
                Debug.Log("Devourer of gods is phase 1!");
                velocity = velocity.RotateTowards(AngleTo(transform.position, target.transform.position), RotationSpeed, true) * MoveSpeed;//this makes the worms velocity rotate towards the player.
                if (velocity != Vector2.zero)//this code is copyed from this yt video https://www.youtube.com/watch?v=gs7y2b0xthU&t=366s and modified slightly.
                {
                    Vector2 movementDirection = new(velocity.x, velocity.y);
                    Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
                }
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
                    Debug.Log("Devourer of gods is phase 2!");

                    velocity = Vector2.zero;

                    Projectile telegraph = Projectile.NewProjectile(projectiles[1], transform.position, Quaternion.identity, 0, 60);

                    oldTargetPos = target.transform.position;

                    Vector3 direction = target.transform.position - telegraph.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    telegraph.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    telegraph.transform.localScale = new Vector3(400f, 7f, transform.localScale.z);
                }
                if (ai[1] == 60f) //after one second
                {
                    Projectile deathray = Projectile.NewProjectile(projectiles[0], transform.position, Quaternion.identity, damage, 240); //create a new projectile called proj (remember class variables must equal an instance of that class. in this example, the variable equals the new projectile)

                    Vector3 direction = (Vector3)oldTargetPos - deathray.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    deathray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    deathray.velocity = DirectionTo(oldTargetPos) * 0.9f; //the new new projectile will travel towards the player

                    deathray.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

                    deathray.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    ;
                }
                if (ai[1] == 240f) //after four seconds
                {
                    ai[0] = 0.0f; //set ai[0] phase to phase one
                    ai[1] = 0.0f; //reset ai[1] timer to 0
                }
            }
        }
        else if (target == null)
        {
            Vector2 movementDirection = new(velocity.x, velocity.y);
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
            velocity = new Vector2(-1, 1) * 0.5f;

            if (GetDistanceToPlayer() > 240f)
            {
                DoGMusic.stop(STOP_MODE.ALLOWFADEOUT);
                Destroy(gameObject);
            }
        }
    }
}
