using System.IO;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DoGHeadAI : NPC
{
    public float MoveSpeed, RotationSpeed;

    public Vector2 oldTargetPos;

    private EventInstance theme;
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        NPCName = "DevourerofGodsHead";
        Damage = 600;
        LifeMax = 1706400;
        Life = LifeMax;
        worm = true;
        
        if (!GameObject.Find("WorldManager").GetComponent<BiomeManager>().bossAlive)
        {
            theme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.DoG1);
            theme.start();
            GameObject.Find("WorldManager").GetComponent<BiomeManager>().bossAlive = true;
        }
    }
    public override void AI()
    {
        UpdateVelocity();

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

                    Projectile telegraph = Projectile.NewProjectile(projectiles[1], transform.position, Quaternion.identity, 0, 0, 0, 4);

                    oldTargetPos = target.transform.position;

                    Vector3 direction = target.transform.position - telegraph.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    telegraph.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    telegraph.transform.localScale = new Vector3(400f, 7f, transform.localScale.z);
                }
                if (ai[1] == 60f) //after one second
                {
                    Vector2 _vel = DirectionTo(transform.position, oldTargetPos) * 25;
                    int _damage = Damage;
                    float _knockback = 0;
                    float _timeLeft = 1;

                    Projectile deathray = Projectile.NewProjectile(projectiles[0], transform.position, Quaternion.identity, _vel, _damage, _knockback, _timeLeft);

                    Vector3 direction = (Vector3)oldTargetPos - deathray.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    deathray.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    deathray.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
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
            if (DistanceBetween(transform.position, target.transform.position) > 240f)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnDestroy()
    {
        theme.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        GameObject.Find("WorldManager").GetComponent<BiomeManager>().bossAlive = false;
    }
}
