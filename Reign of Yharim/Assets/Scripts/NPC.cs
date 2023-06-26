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

    public BoxCollider2D bc2d;

    public LayerMask groundLayer;

    public float[] ai = new float[4];

    public float IFrames = 1f;

    public GameObject[] projectiles;

    public GameObject target;

    public Vector2 targetPos;

    public string currentAnimationState;

    public bool immune;

    public Animator animator;

    public bool worm;


    void Start()
    {
        active = true; //if active

        //reset AI variables
        for (int i = 0; i < ai.Length; i++) //will loop until it reaches ai.length (4)
            ai[i] = 0.0f; //set every ai index to 0 until ai.length (4)

        objectRenderer = GetComponent<Renderer>();

        animator = GetComponent<Animator>();

        SetDefaults(); //call setdefaults

    }

    void Update() => UpdateNPC(); //changes update to updatenpc (gives UpdateNPC the function of Update (to be called every frame))

    public void UpdateVelocity() => transform.position += (Vector3)velocity; //calling UpdateVelocity updates the position of the attached gameobject based on vector2 velocity. Basically, the vector2 velocity stores the movements, and UpdateVelocity turns it into actual transform movement

    public void UpdateNPC() //triggers every frame
    {
        if (!active)
            return;

        target = GameObject.Find("Player"); //target gameobject variable is equal to the Player gameobject
        if(target != null)
            targetPos = target.transform.position;

        Physics2D.IgnoreLayerCollision(3, 3); //NPCs (layer 3) don't collide with other NPCs (also layer 3)

        UpdateVelocity(); //Call updatevelocity

        AI(); //Call ai (AI method is overridden by subclasses)

        if (life <= 0) //If life in is less than or equal to 0
            Die(); //trigger Die method

        if (IsVisibleFromCamera())
        {
            // Enable rendering if the object is visible
            objectRenderer.enabled = true;
        }
        else
        {
            // Disable rendering if the object is outside the camera's view
            objectRenderer.enabled = false;
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

    public float GetDistanceToPlayer() //returns the distance between the object and the target
    {
        return Vector2.Distance(gameObject.transform.position, targetPos);
    }

    public void DrawDistanceToPlayer(Color color) => Debug.DrawLine(gameObject.transform.position, targetPos, color); //drawdistancetoplayer will draw a line from the object to the player that is a set color

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

    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; //if currentAnimationState equals newAnimationState, stop the method (prevents animations from interupting themselves)

        animator.Play(newAnimationState); //play the newState animation

        currentAnimationState = newAnimationState; //set currentAnimationState to newAnimationState
    }

    public IEnumerator Immunity()
    {
        immune = true; //set immune to true
        yield return new WaitForSeconds(IFrames); //wait for IFrames seconds
        immune = false; //set Immune to false
    }
}