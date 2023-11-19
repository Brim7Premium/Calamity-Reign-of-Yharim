using System;
using System.Collections;
using UnityEngine;

public abstract class NPC : Entity //Must be inherited, cannot be instanced 
{
    public string NPCName;

    private int _lifeMax;
    public int lifeMax
    {
        set
        {
            if(value < 0){ Debug.LogError(NPCName + " can't have negative health"); return;}
            _lifeMax = value;

            try{healthBar.SetMaxHealth(_lifeMax);}
            catch(NullReferenceException){}
        }
        get => _lifeMax;
    }

    private int _life;
    public int TargetDirection
    {
        get => transform.position.x < target.transform.position.x ? 1 : -1;
    }
    public int life
    {
        set
        {
            _life = value;
            if(value > lifeMax)
            {
                _life = lifeMax;
                Debug.LogWarning(NPCName + " can't have more hp then max hp");
            }
            if (_life <=0)
            {
                Kill();
            }

            try{ healthBar.SetHealth(_life);}
            catch(NullReferenceException){}
        }

        get {return _life;}
    }

    private int _damage;
    public int damage
    {
        set { 
            if(value < 0) Debug.LogError(NPCName + " can't deal negative damage"); //I think it's very unlikely that we gonna use negative values here. 
            else _damage = value;
        }

        get { return _damage;}
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
            life -= damage;            
        }
    }

    public virtual void AI(){}
    public virtual void OnHit(){}
    public override void Kill() => Destroy(gameObject);

    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));
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