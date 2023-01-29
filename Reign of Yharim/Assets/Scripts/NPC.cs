using System;
using System.Collections;
using UnityEngine;

public abstract class NPC : Entity
{
    public GameObject target;
    public int life;
    public bool collide;
    public int lifeMax;
    public float IFrames = 1f;
    public HealthBar healthBar;
    public Rigidbody2D rb;
    public AudioClip HitSound;
    public float[] ai = new float[4];
    [SerializeField] private bool immune;
    private Animator playerAnimator;
    void Start()
    {
        active = true;
        for (int i = 0; i < ai.Length; i++)
            ai[i] = 0.0f;
        SetDefaults();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
    }
    void Update() => UpdateNPC();
    public void UpdateVelocity() => transform.position += (Vector3)velocity;
    public void UpdateNPC()
    {
        if (!active)
            return;
        target = GameObject.Find("Player");
        UpdateVelocity();
        AI();
        if (life <= 0)
            Die();
    }
    public void MoveTowards(float speedX, float speedY)
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
    public void Die()
    {
        OnKill();
        gameObject.SetActive(false);
        active = false;
    }
    public virtual void SetDefaults()
    {
    }
    public virtual void AI()
    {
    }
    public virtual void OnHit()
    {
    }
    public virtual void OnKill()
    {
    }
    public Vector2 ToRotationVector2(float f) => new((float)Math.Cos(f), (float)Math.Sin(f));
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Item" && immune == false)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Swing") == true)
            {
                OnHit();
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
