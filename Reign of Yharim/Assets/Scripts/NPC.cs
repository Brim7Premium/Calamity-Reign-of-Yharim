using System;
using System.Collections;
using UnityEngine;

public abstract class NPC : Entity
{
    public GameObject target;
    public int life;
    public bool worm;
    public int lifeMax;
    public float IFrames = 1f;
    public HealthBar healthBar;
    public Rigidbody2D rb;
    public AudioClip HitSound;
    public float[] ai = new float[4];
    [SerializeField] private bool immune;
    public Animator playerAnimator;
    public AudioSource audioSource;
    void Start()
    {
        active = true;
        //reset ai variables
        for (int i = 0; i < ai.Length; i++)
            ai[i] = 0.0f;
        SetDefaults();
        //assign components
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        audioSource.clip = HitSound;
    }
    void Update() => UpdateNPC();
    public void UpdateVelocity() => transform.position += (Vector3)velocity;
    public void UpdateNPC()
    {
        if (!active)
            return;
        Physics2D.IgnoreLayerCollision(3, 3);
     target = GameObject.Find("Player");
        UpdateVelocity();
        AI();
        if (life <= 0)
            Die();
    }
    public void MoveTowards(float speedX, float speedY)//moves the npc towards the player at a set speed.
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
    public void TakeDamage(int damage)
    {
        life -= damage;
        healthBar.SetHealth(life);
    }
    public void Die()//kills the npc(sets gameobject.active to false) and calls onkill
    {
        if (worm)
        {
            gameObject.transform.parent.gameObject.SetActive(false);//kill the parent gameobject of the worm segment
            OnKill();
            active = false;
        }
        else
        {
            OnKill();
            gameObject.SetActive(false);//kill the gameobject
            active = false;
        }
    }
    public virtual void SetDefaults()//called on start
    {
    }
    public virtual void AI()//called every frame
    {
    }
    public virtual void OnHit()//called when the npc is hit
    {
    }
    public virtual void OnKill()//called when the npc dies
    {
    }
    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));//converts an angle into a Vector2
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Item" && immune == false)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Swing") == true)
            {
                OnHit();
                if (HitSound != null)
                    audioSource.Play();
                TakeDamage(5);
                StartCoroutine(EnemyImmunity());
            }
        }
    }
    IEnumerator EnemyImmunity()
    {
        immune = true;
        yield return new WaitForSeconds(IFrames);
        immune = false;
    }
}
