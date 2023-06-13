using System;
using System.Collections;
using UnityEngine;

public abstract class NPC : Entity //Must be inherited, cannot be instanced 
{
    public string NPCName;

    public int lifeMax;

    public int life;

    public int damage;

    public HealthBar healthBar;

    public Rigidbody2D rb;

    public float[] ai = new float[4];

    public float IFrames = 1f;

    public GameObject[] projectiles;

    public GameObject target;

    public bool immune;

    public bool worm;

    public bool respawnable;


    void Start()
    {
        active = true; //if active

        //reset AI variables
        for (int i = 0; i < ai.Length; i++) //will loop until it reaches ai.length (4)
            ai[i] = 0.0f; //set every ai index to 0 until ai.length (4)

        SetDefaults(); //call setdefaults
    }

    void Update() => UpdateNPC(); //changes update to updatenpc (gives UpdateNPC the function of Update (to be called every frame))

    public void UpdateVelocity() => transform.position += (Vector3)velocity; //calling UpdateVelocity updates the position of the attached gameobject based on vector2 velocity. Basically, the vector2 velocity stores the movements, and UpdateVelocity turns it into actual transform movement

    public void UpdateNPC() //triggers every frame
    {
        if (active) //if active
        {
            target = GameObject.Find("Player"); //target gameobject variable is equal to the Player gameobject

            Physics2D.IgnoreLayerCollision(3, 3); //NPCs (layer 3) don't collide with other NPCs (also layer 3)

            UpdateVelocity(); //Call updatevelocity

            AI(); //Call ai (AI method is overridden by subclasses)

            if (life <= 0) //If life in is less than or equal to 0
                Die(); //trigger Die method
        }
        else
        {
            UpdateVelocity(); //Call updatevelocity
            return; //prevents the subsequent code from running every frame until active again
        }
    }

    public void MoveTowards(float speedX, float speedY)//moves the npc towards the player at a set speed.
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

    public void RemoveHealth(int damage) //remove health with no Iframes
    {
        life -= damage; //Subtracts damage from life and sets life to result
        healthBar.SetHealth(life); //Set the health of the healthbar to new life value
    }

    public void TakeDamage(int damage)
    {
        if (immune == false)
        {
            OnHit();
            RemoveHealth(damage);
            StartCoroutine(Immunity());
        }
    }

    public void Die()//kills the npc(sets gameobject.active to false) and calls onkill
    {
        if (worm)
        {
            gameObject.transform.parent.gameObject.SetActive(false);//kill (Set inactive) the parent gameobject of the worm segment
            OnKill(); //Trigger OnKill
            active = false; //Kill segment
        }
        else if (respawnable)
        {
            StartCoroutine(RespawnTimer());
        }
        else
        {
            gameObject.SetActive(false);//kill the gameobject
            OnKill(); //Trigger Onkill
            active = false; //Kill
        }
    }

    public virtual void SetDefaults()//called on start, overridden by subclasses for customization
    {
    }
    public virtual void AI()//called every frame, overridden by subclasses for customization
    {
    }
    public virtual void OnHit()//called when the npc is hit, overridden by subclasses for customization
    {
    }
    public virtual void OnKill()//called when the npc dies, overridden by subclasses for customization
    {
    }
    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));//converts an angle into a Vector2

    //Code for old damage detection system
    /* public virtual void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "Item" && immune == false) //if not immune, and colliding with the item gameobject
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Swing") == true) //if the player is swinging the sword (checks though animator)
            {
                OnHit(); //trigger method
                TakeDamage(PlayerAI.Damage); //takes damage
                StartCoroutine(EnemyImmunity()); //start EnemyImmunity coroutine
            }
        }
        if (collision.gameObject.name == "Player")
        {
        }
    }
    */
    public IEnumerator Immunity()
    {
        immune = true; //set immune to true
        yield return new WaitForSeconds(IFrames); //wait for IFrames seconds
        immune = false; //set Immune to false
    }
    public IEnumerator RespawnTimer()
    {
        active = false;
        yield return new WaitForSeconds(5); //wait for 5 seconds
        active = true;
        SetDefaults();
    }
}