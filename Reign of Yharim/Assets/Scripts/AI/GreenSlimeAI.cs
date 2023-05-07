using UnityEngine;

public class GreenSlimeAI : NPC
{
    public static string Name => "GreenSlime(Clone)";
    public static int Damage => 5;
    public override void SetDefaults()
    {
        lifeMax = 20;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Item" && immune == false) //if not immune, and colliding with the item gameobject
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(1).IsName("Swing") == true) //if the player is swinging the sword (checks though animator)
            {
                OnHit(); //trigger method
                TakeDamage(PlayerAI.Damage); //takes damage
                StartCoroutine(EnemyImmunity()); //start EnemyImmunity coroutine
            }
        }
        if (collision.gameObject.name == "Player")
        {
            //write later
        }
    }
    public override void AI()
    {
        ai[0]++;//increment ai[0] by 1 every frame.(the framerate is capped at 60)
        velocity *= 0.95f;//this is for smoothing the movement.
        if (ai[0] == 90.0f) //if it has been 90 frames, jump.
        {
            velocity.x = GetTargetDirectionX() * 0.2f;
            velocity.y = 0.2f;
        }
        if (ai[0] == 120.0f)
        {
            velocity = Vector2.zero;//set velocity to 0.
            ai[0] = 0.0f;//reset ai[0] so we can jump again.
        }
    }
}
