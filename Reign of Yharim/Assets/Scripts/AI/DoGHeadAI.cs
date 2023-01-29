using UnityEngine;

public class DoGHeadAI : NPC
{
    public static string Name => "TheDevourerofGodsHead";
    public static int Damage => 600;
    public float MoveSpeed;
    public float RotationSpeed;
    public override void SetDefaults()
    {
        lifeMax = 1706400;
        life = lifeMax;
        healthBar.SetMaxHealth(lifeMax);
    }
    public override void AI()
    {
        velocity = velocity.RotateTowards(AngleTo(target.transform.position), RotationSpeed, true) * MoveSpeed;
        if (velocity != Vector2.zero)
        {
            Vector2 movementDirection = new(velocity.x, velocity.y);
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);
        }
    }
}
