using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded = false;

    [SerializeField] private Animator animator;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public int iFrames = 1;
    private bool immune;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        Physics2D.IgnoreLayerCollision(10, 6);
        horizontal = Input.GetAxis("Horizontal"); //sets horizontal to -1 or 1 based on the player's input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Attacking", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("Attacking", false);
        }
        
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);//sets the speed of the player along the x coordinate to 1 * speed or -1 * speed, allowing the player to move horizontally based on input
        //rb.velocity.y
        if (horizontal > 0 || horizontal < 0) //left or right
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        if (horizontal > 0) //right
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (horizontal < 0) //left
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        int damage = 0;
        if(collision.gameObject.name == AureusAI.Name)
            damage = AureusAI.Damage;
        if(collision.gameObject.name == GreenSlimeAI.Name)
            damage = GreenSlimeAI.Damage;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.layer == 3 && immune == false)
        {
            TakeDamage(damage); //damage the player for 5 damage
            StartCoroutine(Immunity()); //when the player is damaged, start courotine
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
    private bool IsGrounded()
    {
        return isGrounded;
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator Immunity()
    {
        immune = true; //once courotine is started, set immune to true
        yield return new WaitForSeconds(iFrames); //wait iFrames seconds
        immune = false; //set immune to false, allowing for the player to be damaged
    }
}
