using System;
using System.Collections;
using UnityEngine;

public abstract class NPC : Entity //Must be inherited, cannot be instanced 
{
    public string NPCName;

    private int _lifeMax; //lifemax property value field
    public int LifeMax //property, which can be called as set or get in the other code. Whatever the other code sets this as will return to the set as value, and get will just directly return _lifeMax
    {
        set //brim is learning what getters and setters are as we speak :)
        {
            if(value < 0) //if something tries to set the value below 0
            { 
                Debug.LogError(NPCName + " can't have negative health"); return; //just a simple note, but return will stop this part of code and prevent subsequent code from running (for brim because he forgets stuff all the time)
            }
            _lifeMax = value; //if LifeMax set is called, it will set the property value field to whatever the LifeMax value is called as

            try
            {
                healthBar.SetMaxHealth(_lifeMax); //try to set the healthbar's max health to whatever the new _lifeMax value is.
            }
            catch(NullReferenceException)
            {
                Debug.LogError(NPCName + " couldn't set health bar's max health. Maybe it doesn't exist or isn't set?"); //as long as it actually has a healthbar. If it doesn't, mark a custom error
            }
        }
        get => _lifeMax; //directly returns _lifeMax
    }

    private int _life; //let there be life (property value field)
    public int Life //life property 
    {
        set
        {
            _life = value; //set life to whatever value is input
            if(value > LifeMax) //if said value is greater than the get of LifeMax (which is just _lifeMax)
            {
                _life = LifeMax; //set the _life field back to max life
                Debug.LogWarning(NPCName + " can't have more hp than max hp"); //warn the system for some reason
            }
            if (_life <=0) //if life is less than or equal to 0, call kill method
            {
                Kill();
            }

            try
            { 
                healthBar.SetHealth(_life); //try to set the healthbar's current health to whatever the new _life value is
            }
            catch(NullReferenceException)
            {
                Debug.LogError(NPCName + " couldn't change health of the healthbar. Maybe it doesn't exist or isn't set?"); //as long as it actually has a healthbar. If it doesn't, mark a custom error
            }
        }

        get => _life; //directly returns life
    }

    private int _damage; //damage property value field
    public int Damage //damage property
    {
        set 
        { 
            if(value < 0) //as long as damage isn't negative, which we probably won't use, but could always just change if we needed to
                Debug.LogError(NPCName + " can't deal negative damage"); //I think it's very unlikely that we gonna use negative values here. 
            else _damage = value; //damage will just be set to damage property value field
        }

        get => _damage; //directly returns damage
    }
    public int TargetDirection //read-only
    {
        get => transform.position.x < target.transform.position.x ? 1 : -1; //if NPC's x position is less than the target's x position, return get as 1, else return get as -1
    }

    public HealthBar healthBar;

    public Rigidbody2D rb;

    public Collider2D c2d;

    public LayerMask groundLayer;

    public float[] ai = new float[4];
    private const string indulgencesHolders = "WulfrumGyrator, DevourerofGodsBody, DevourerofGodsHead, Dummy, ExampleNPC";//This ones need to be remade at some point

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
        base.SetDefaults();

        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        rb = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if(healthBar == null){healthBar = GetComponent<HealthBar>();}
        if(healthBar == null){healthBar = GetComponentInChildren<HealthBar>();}
        if(healthBar == null){Debug.LogWarning(NPCName + " doesn't have healthBar");}
    }

    public void UpdateVelocity(){
        if(indulgencesHolders.Contains(NPCName))
            transform.position += (Vector3)velocity;
        else Debug.LogWarning(NPCName + ": Use Rigidbody2D instead of UpdateVelocity");

    }
    public void Update()
    {
        AI();
        objectRenderer.enabled = IsVisibleFromCamera();
    }

    public float GetDistanceToPlayer()
    {
        return Vector2.Distance(gameObject.transform.position, target.transform.position);
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
    public override void Kill() => Destroy(gameObject);

    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f)); //converts the float rotation that is output by methods like AngleTo into a vector2 rotation
    public void DrawDistanceToPlayer(Color color) => Debug.DrawLine(gameObject.transform.position, target.transform.position, color);

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
}