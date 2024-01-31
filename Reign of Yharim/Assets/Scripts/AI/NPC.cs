using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


// Custom serializable class
[Serializable]
public class Loot
{
    public ItemData item;
    public string condition;
    public int amountLower;
    public int amountUpper;
    public int chanceLower;
    public int chanceUpper;
    [Range(1, 4)] public int difficulty;
}
public abstract class NPC : Entity //Must be inherited, cannot be instanced 
{
    public Loot[] lootTable;
    public string NPCName;

    public bool inWater;

    public bool IsGrounded;
    private int _lifeMax; //lifemax property value field
    public int LifeMax //property, which can be called as set or get in the other code. Whatever the other code sets this as will return to the set as value, and get will just directly return _lifeMax
    {
        set 
        {
            //just a simple note, but return will stop this part of code and prevent subsequent code from running (for brim because he forgets stuff all the time)
            if (value < 0) { Debug.LogError(NPCName + " can't have negative health"); return; }
            _lifeMax = value;
            //try to set the healthbar's max health to whatever the new _lifeMax value is.
            try {healthBar.SetMaxHealth(_lifeMax);}
            //as long as it actually has a healthbar. If it doesn't, mark a custom error
            catch (NullReferenceException) { Debug.LogWarning(NPCName + " couldn't set health bar's max health. Maybe it doesn't exist or isn't set?"); }
        }
        get => _lifeMax; //directly returns _lifeMax
    }

    private int _life; 
    public int Life 
    {
        set
        {
            _life = value; 
            if(value > LifeMax) 
            {
                _life = LifeMax; 
                Debug.LogWarning(NPCName + " can't have more hp than max hp"); 
            }
            if (_life <=0) 
                Kill();

            try {healthBar.SetHealth(_life);}
            catch (NullReferenceException) { Debug.LogWarning(NPCName + " couldn't change health of the healthbar. Maybe it doesn't exist or isn't set?"); }
        }

        get => _life;
    }

    private int _damage;
    public int Damage
    {
        set 
        { 
            if(value < 0) 
                Debug.LogError(NPCName + " can't deal negative damage"); //I think it's very unlikely that we gonna use negative values here. 
            else _damage = value; 
        }

        get => _damage;
    }
    public int TargetDirection //read-only
    {
        //if NPC's x position is less than the target's x position, return get as 1, else return get as -1
        get => transform.position.x < target.transform.position.x ? 1 : -1; 
    }

    //public bool FighterAI = false;

    public Vector3 oldPosition;

    public HealthBar healthBar;

    public SpriteRenderer sprite;

    public Rigidbody2D rb;

    public Collider2D c2d;

    public LayerMask groundLayer;

    public float[] ai = new float[4];
    private const string indulgencesHolders = "DevourerofGodsBody, DevourerofGodsHead, Dummy, ExampleNPC";//This ones need to be remade at some point

    public float IFrames = 1f;

    public GameObject[] projectiles;

    public GameObject target;

    public string currentAnimationState;

    public bool immune;

    public Animator animator;

    public bool worm;

    public Vector2 velocity;


    public override void SetDefaults()
    {
        base.SetDefaults(); //first run the base code from entity

        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if(healthBar == null){healthBar = GetComponent<HealthBar>();}
        if(healthBar == null){healthBar = GetComponentInChildren<HealthBar>();}

        //LifeMax already does this
        //if(healthBar == null){Debug.LogWarning(NPCName + " doesn't have healthBar");} 
    }

    public void UpdateVelocity(){
        if(indulgencesHolders.Contains(NPCName))
            transform.position += (Vector3)velocity;
        else Debug.LogWarning(NPCName + ": Use Rigidbody2D instead of UpdateVelocity");

    }
    public void Update()
    {
        if (target == null)
		{
			target = GameObject.Find("Player");
		}
        OnGroundDeterminer();
        AI();
        inWater = InWaterDeterminer(c2d);
        objectRenderer.enabled = IsVisibleFromCamera();

        healthBar.gameObject.transform.parent.parent.parent.rotation = Quaternion.Euler(0, 0, 0); // Fix the HealthBar so it won't rotate if NPC does

        oldPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        if (immune == false)
        {
            OnHit();
            StartCoroutine(Immunity());
            Life -= damage;            
        }
    }

    public virtual void AI(){}
    public virtual void OnHit(){}
    public override void Kill()
    {
        DropItems();
        Destroy(gameObject); //specific NPCs can still override 
    }
    public void DropItems()
    {
        foreach(Loot i in lootTable)
        {
            if(Random.value>i.chanceLower) continue;

            WorldItem.InitItem(i.item, Random.Range(i.amountLower, i.amountUpper), transform.position).rb.AddForce(200 * Random.Range(-1f, 1f) * Vector2.one);
        }
    }

    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f)); //converts the float rotation that is output by methods like AngleTo into a vector2 rotation

    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState == newAnimationState) return; 

        animator.Play(newAnimationState);

        currentAnimationState = newAnimationState;
    }

    public IEnumerator Immunity()
    {
        immune = true;
        yield return new WaitForSeconds(IFrames);
        immune = false;
    }
    public GameObject ClosestNPC() //Function to find Closest NPC to this NPC
    {
        GameObject NearestObj = null;
        float nearestdist = float.MaxValue;
        foreach (var Obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (Obj != null && LayerMask.LayerToName(Obj.layer) == "NPCs" && gameObject != Obj)
            {
                if (Vector2.Distance(transform.position, Obj.transform.position) < nearestdist)
                {
                    nearestdist = Vector3.Distance(transform.position, Obj.transform.position);
                    NearestObj = Obj;
                }
            }
        }
        return NearestObj;
    }

    public Vector2 GetDirection(Vector2 TargetPos, Vector2 MyPos) => (TargetPos - MyPos).normalized;

    public float GroundDeterminerRayLength = 0;

    void OnGroundDeterminer()
    {
        float extraHeight = GroundDeterminerRayLength;
        Color rayColor; //new color variable rayColor 

        RaycastHit2D hit = Physics2D.Raycast(c2d.bounds.center, Vector2.down, c2d.bounds.extents.y + extraHeight, groundLayer); //new raycast2d called hit that starts from the center of the player rigidbody, goes down, and goes the extent of the rigidbody downwards + extraHeight. it only collides with the groundlayer variable

        //Debug.Log(hit.collider);
        if (hit.collider != null) //if the raycast is hitting something;
        {
            IsGrounded = true; //isgrounded is true
            rayColor = Color.green; //the raycolor is green
        }
        else
        {
            IsGrounded = false; //isgrounded is false
            rayColor = Color.red; //the raycolor is red
        }
        Debug.DrawRay(c2d.bounds.center, Vector2.down * (c2d.bounds.extents.y + extraHeight), rayColor); //draw the ray 
    }
    public static bool InWaterDeterminer(Collider2D c2d)
    {
        List<Collider2D> colliders = new();
        ContactFilter2D contactFilter = new();
        contactFilter.layerMask = 1 << LayerMask.NameToLayer("Liquid");
        contactFilter.useTriggers = true;
        c2d.GetContacts(contactFilter, colliders);

        foreach(Collider2D i in colliders)
        {
            if(i.name == "Water") return true;
        }
        return false;
    }

    public bool IsWulfrumGuy = false;
}