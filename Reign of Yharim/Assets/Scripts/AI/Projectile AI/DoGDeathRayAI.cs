using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoGDeathRayAI : Projectile
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        projName = "DoGDeathRay";
        rb.gravityScale = 0;
    }
}
