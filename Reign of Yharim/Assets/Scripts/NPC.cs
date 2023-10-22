using System;
using System.Collections;
using UnityEngine;

public abstract class NPC : Entity //Must be inherited, cannot be instanced 
{
    public string NPCName;

    public int lifeMax;

    private int _life;
    public int life
    {
        set
        {
            _life = value;
            healthBar.SetHealth(_life);
            if (_life <=0){gameObject.SetActive(false);}
        }
        get {return _life;}
    }

    private int _damage;
    public int damage
    {
        get { return _damage;}
        set { _damage = value < 0 ? _damage : value;} //I think it's very unlikely that we gonna use negative values here. 
    }

    public HealthBar healthBar;

    public Rigidbody2D rb;

    public Collider2D c2d;

    public LayerMask groundLayer;

    public float[] ai = new float[4];
    private const string indulgencesHolders = "WulfrumGyrator, DevourerofGodsBody, DevourerofGodsHead, Dummy";

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

        rb = GetComponent<Rigidbody2D>();
        c2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void UpdateVelocity(){
        if(indulgencesHolders.Contains(NPCName))
            transform.position += (Vector3)velocity;
        else Debug.LogError(NPCName + ":Use Rigidbody2D instead of UpdateVelocity");

    }
    public void Update()
    {
        AI();
        objectRenderer.enabled = IsVisibleFromCamera();
    }

    public int GetTargetDirectionX() => transform.position.x < target.transform.position.x ? 1 : -1;

    public float GetDistanceToPlayer()
    {
        return Vector2.Distance(gameObject.transform.position, target.transform.position);
    }

    public void TakeDamage(int damage)
    {
        if (immune == false)
        {
            OnHit();
            life -= damage;
            StartCoroutine(Immunity());
            
        }
    }

    public virtual void AI(){}
    public virtual void OnHit(){}

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