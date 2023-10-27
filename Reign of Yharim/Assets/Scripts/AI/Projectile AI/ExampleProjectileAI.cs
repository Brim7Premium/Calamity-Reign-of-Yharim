using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleProjectileAI : Projectile
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        gameObject.name = "ExampleProjectile";
        target = GameObject.Find("Player");
        rb.gravityScale = 0;
    }
}
