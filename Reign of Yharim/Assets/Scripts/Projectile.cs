using System;
using UnityEngine;

public abstract class Projectile : Entity //Must be inherited, cannot be instanced 
{
    public GameObject target;

    public int timeLeft;//how long the projectile will be alive for (timeLeft = 60 would be one second, timeLeft = 120 would be 2, 180 would be 3 and so on)

    public float[] ai = new float[4];

    public int damage;

    void Start()
    {
        active = true; //if active

        //reset AI variables
        for (int i = 0; i < ai.Length; i++) //will loop until it reaches ai.length (4)
            ai[i] = 0.0f; //set every ai index to 0 until ai.length (4)

        SetDefaults(); //call setdefaults
    }
    void Update() => UpdateProj(); //changes update to updateproj (gives UpdateProj the function of Update (to be called every frame))

    public void UpdateVelocity() => transform.position += (Vector3)velocity; //calling UpdateVelocity updates the position of the attached gameobject based on vector2 velocity. Basically, the vector2 velocity stores the movements, and UpdateVelocity turns it into actual transform movement

    public void UpdateProj() //triggers every frame
    {
        if (!active) //if not active
            return; //prevents the subsequent code from running every frame until active again

        target = GameObject.Find("Player"); //target gameobject variable is equal to the Player gameobject

        Physics2D.IgnoreLayerCollision(3, 3); //NPCs (layer 3) don't collide with other NPCs (also layer 3)

        UpdateVelocity(); //Call updatevelocity
        AI(); //Call ai (AI method is overridden by subclasses)

        timeLeft--; //Subtract timeLeft variable by 1

        if (timeLeft <= 0) //if timeleft is less than or equal to 0
            Destroy(gameObject); //destroy this object
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
        if (transform.position.x < target.transform.position.x) //if the attached transform's x position is less than the target's x position
            velocity.x = speedX; //x of velocity equals float speedX parameter
        else
            velocity.x = -speedX; //x of velocity equals negative speedX
        if (transform.position.y < target.transform.position.y) //if the attached transform's y position is less than the target's y position
            velocity.y = speedY; //y of velocity equals float speedY parameter
        else
            velocity.y = -speedY; //y of velocity equals negative speedY
    }

    public int GetTargetDirectionX() => transform.position.x < target.transform.position.x ? 1 : -1; //if transform.position.x is less than, then GetTargetDirectionX returns 1, if else -1

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
