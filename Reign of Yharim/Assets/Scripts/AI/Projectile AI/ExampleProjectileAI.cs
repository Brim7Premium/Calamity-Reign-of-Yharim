using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleProjectileAI : Projectile
{
    public override void SetDefaults()
    {
        projName = "ExampleProjectile";
        //objectRenderer = GetComponent<Renderer>();
    }
    /*public override void AI()
    {
        // Check if the object is inside the camera's view frustum
        if (IsVisibleFromCamera())
        {
            // Enable rendering if the object is visible
            objectRenderer.enabled = true;
        }
        else
        {
            // Disable rendering if the object is outside the view frustum
            objectRenderer.enabled = false;
        }
    }*/
}
