using System.Collections;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    public Vector2 velocity;
    public GameObject target;
    public int life;
    public bool active;
    public int lifeMax;
    public float IFrames = 1f;
    public HealthBar healthBar;
    public Rigidbody2D rb;
    public float[] ai = new float[4];
    [SerializeField] private bool immune;
    [SerializeField] private Animator playerAnimator;
    void Start()
    {
        active = true;
        for(int i = 0; i < ai.Length; i++)
            ai[i] = 0.0f;
        SetDefaults();
    }
    void Update() => UpdateNPC();
    public void UpdateVelocity() => transform.position += (Vector3)velocity;
    public void UpdateNPC()
    {
        if (!active)
            return;
        UpdateVelocity();
        AI();

        Die();
    }
    public void MoveTowards(float speedX, float speedY)
    {
        if(transform.position.x < target.transform.position.x)
            velocity.x = speedX;
        else
            velocity.x = -speedX;
        if(transform.position.y < target.transform.position.y)
            velocity.y = speedY;
        else
            velocity.y = -speedY;
    }
    public void TakeDamage(int damage)
    {
        life -= damage;
        healthBar.SetHealth(life);
    }
    public void Die()
    {
        if (life <= 0)
        {
            gameObject.SetActive(false);
            active = false;
        }
    }
    public virtual void SetDefaults()
    {
    }
    public virtual void AI()
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Item" && immune == false)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Swing") == true)
            {
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
