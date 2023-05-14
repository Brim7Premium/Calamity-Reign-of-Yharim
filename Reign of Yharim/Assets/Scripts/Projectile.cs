using System;
using UnityEngine;

public abstract class Projectile : Entity //Must be inherited, cannot be instanced 
{
    public GameObject target;

    public int timeLeft;//how long the projectile will be alive for (timeLeft = 60 would be one second, timeLeft = 120 would be 2, 180 would be 3 and so on)

    public AudioClip HitSound;

    public float[] ai = new float[4];

    public Animator playerAnimator;

    public AudioSource audioSource;

    public int damage;

    void Start()
    {
        active = true;
        //reset ai variables
        for (int i = 0; i < ai.Length; i++)
            ai[i] = 0.0f;
        SetDefaults();
        //assign components
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        //audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        //audioSource.clip = HitSound;
    }
    void Update() => UpdateProj();

    public void UpdateVelocity() => transform.position += (Vector3)velocity;

    public void UpdateProj()
    {
        if (!active)
            return;

        target = GameObject.Find("Player");

        Physics2D.IgnoreLayerCollision(3, 3);

        UpdateVelocity();
        AI();

        timeLeft--;

        if (timeLeft <= 0)
            Destroy(gameObject);
    }
    public static Projectile GetProjectile(GameObject gameObject)
    {
        return gameObject.GetComponent<Projectile>();
    }

    /*projectile info:
     * projectile.newprojectile creates a projectile gameoject using the supplied parameters
     * 
     * GameObject projectile is a prefab with a projectile ai script attached.
     * 
     * if the prefab's projectile ai script modifies position, rotation, damage, timeleft, or ai[] in the SetDefaults() method it will override the values set in projectile.newprojectile
     */

    public static Projectile NewProjectile(GameObject projectile, Transform transform, Quaternion quaternion, int damage, int timeleft = 0, float ai0 = 0.0f, float ai1 = 0.0f, float ai2 = 0.0f, float ai3 = 0.0f)
    {
        GameObject projGameObject = Instantiate(projectile, transform.position, quaternion);
        Projectile proj = GetProjectile(projGameObject);

        if (timeleft != 0)
        {
            proj.timeLeft = timeleft;
        }

        proj.damage = damage;
        proj.ai[0] = ai0;
        proj.ai[1] = ai1;
        proj.ai[2] = ai2;
        proj.ai[3] = ai3;

        return proj;
    }

    public void MoveTowards(float speedX, float speedY)//moves the projectile towards the player at a set speed.
    {
        if (transform.position.x < target.transform.position.x)
            velocity.x = speedX;
        else
            velocity.x = -speedX;
        if (transform.position.y < target.transform.position.y)
            velocity.y = speedY;
        else
            velocity.y = -speedY;
    }

    public int GetTargetDirectionX() => transform.position.x < target.transform.position.x ? 1 : -1;

    public virtual void SetDefaults()//called on start
    {
    }

    public virtual void AI()//called every frame
    {
    }

    public virtual void OnHitPlayer()//called when the projectile hits a player
    {
    }

    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));//converts an angle into a Vector2
}
