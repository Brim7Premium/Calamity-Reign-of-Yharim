using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphAI : Projectile
{
    private Renderer objectRenderer;

    public override void SetDefaults()
    {
        projName = "TelegraphProjectile";
        damage = 0;

        objectRenderer = GetComponent<Renderer>();
    }
    public override void AI()
    {
        if (IsVisibleFromCamera())
        {
            // Enable rendering if the object is visible
            objectRenderer.enabled = true;
        }
        else
        {
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
