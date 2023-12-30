using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProj : Projectile
{
    public override void SetDefaults()
    {
        timeLeft = 5;
        base.SetDefaults();
        rb.gravityScale = 0;
        damage = 5;
        rb.velocity = (MousePos - transform.position).normalized * 5;
    }
    public override void AI()
    {
        Collider2D target = Physics2D.OverlapBox(transform.position, Vector2.one*4, 0, 1 << LayerMask.NameToLayer("NPCs"));
        if(target)
        {
            Vector2 direction = target.transform.position - transform.position;
            float force = Mathf.Sqrt(6-direction.magnitude);
            rb.AddForce(direction.normalized * force * 200);
        }
        
    }
}
