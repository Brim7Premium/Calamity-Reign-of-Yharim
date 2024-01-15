using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindProj : Projectile
{
    public override void SetDefaults()
    {
        timeLeft = 12;
        base.SetDefaults();
        rb.gravityScale = 0;
        damage = 5;
        velocity = 4;
        rb.velocity = (MousePos - (Vector2)transform.position).normalized * velocity;
    }
    public override void AI()
    {
        Collider2D target = Physics2D.OverlapBox(transform.position, Vector2.one*16.875f*4, 0, 1 << LayerMask.NameToLayer("NPCs"));
        
        if (target)
        {
            Vector2 direction = target.transform.position - transform.position;
            rb.velocity = direction.normalized * velocity;
        }
    }
}
