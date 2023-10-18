using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleProjectileAI : Projectile
{
    public override void SetDefaults()
    {
        gameObject.name = "ExampleProjectile";
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
}
