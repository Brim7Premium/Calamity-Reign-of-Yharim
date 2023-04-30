using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Entity
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
        Physics2D.IgnoreLayerCollision(3, 6);

        //sets horizontal to -1 or 1 based on the player's input
        horizontal = Input.GetAxis("Horizontal"); 
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
        //sets the speed of the player along the x coordinate to 1 * speed or -1 * speed, allowing the player to move horizontally based on input
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if (horizontal > 0 || horizontal < 0) { //left or right
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
    public void OnCollisionStay2D(Collision2D collision)
    {
        int damage = 0; //creates damage int variable and resets it to 0

        // If colliding with Aureus
        if(collision.gameObject.name == AureusAI.Name) 
            // Set damage to Aureus damage value
            damage = AureusAI.Damage; 

        // If colliding with Green Slime
        if(collision.gameObject.name == GreenSlimeAI.Name) 
            // Set damage to Green Slime damage value
            damage = GreenSlimeAI.Damage; 

        // If colliding with the ground layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
            //set the isgrounded variable to true
            isGrounded = true; 

        // If colliding with the layerID 3 (NPCs) and the player is not immune
        if (collision.gameObject.layer == 3 && immune == false) 
        {
            TakeDamage(damage); // Take damage equal to damage variable
            StartCoroutine(Immunity()); // When the player is damaged, start courotine
        }
    }
    
    /*
    NOTE FROM JELLO:
    I recommend that we approach the damage function from different means. It would be resource intensive
    if we were to make the player determine what it's colliding with as we add more enemies.

    It would make more sense if we have each enemy call TakeDamage() when a collision with the player
    is detected.
    */

    private void Die()//does nothing rn
    {
        if(currentHealth <= 0)
        {
            rb.velocity = Vector2.zero;
            
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
    /*
    NOTE FROM JELLO:
        We could have the players Immunity() be called by enemies after they call TakeDamage() 
        and modify Immunity to take an input, or use polumorphism to create another Immunity() that does
        take an input so that we can have damage use the default iFrames amount or specify them.

        Example:
            
            IEnumerator Immunity() { //Default call
                immune = true;
                yield return new WaitForSeconds(1);
                immune = false;
            }

            IEnumerator Immunity(int frames) { //Specified call
                immune = true;
                yield return new WaitForSeconds(frames);
                immune = false;
            }

            // Immunity() -> calls default 
            // Immunity(0.05) ->  calls specified

        This could also be placed in the Entity class, but exact implementation details for detecting 
        what is colliding with what in an intuitive manner cannot be provided by me at the moment.  
    */
}
