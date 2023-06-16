using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleProjectileAI : Projectile
{
    private Renderer objectRenderer;

    public override void SetDefaults()
    {
        projName = "ExampleProjectile";
        damage = 1;

        objectRenderer = GetComponent<Renderer>();
    }
    public override void AI()
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
    }


    private bool IsVisibleFromCamera()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Check if the object's collider bounds intersect with any of the frustum planes
        return GeometryUtility.TestPlanesAABB(frustumPlanes, objectRenderer.bounds);
    }
}
