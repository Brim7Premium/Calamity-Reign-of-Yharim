using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public bool active; //bool variable  active
    public Vector2 velocity; //vector2 variable velocity 
    public Renderer objectRenderer;
    public float AngleTo(Vector2 Destination) => (float)Math.Atan2(Destination.y - transform.position.y, Destination.x - transform.position.x); 
    public Vector2 DirectionTo(Vector2 Destination) => Vector3.Normalize((Vector3)Destination - transform.position);//self explanitory
    public Vector2 DirectionTo(Vector2 Destination, Entity entity) => Vector3.Normalize((Vector3)Destination - entity.transform.position);

    public bool IsVisibleFromCamera()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Check if the object's collider bounds intersect with any of the frustum planes
        return GeometryUtility.TestPlanesAABB(frustumPlanes, objectRenderer.bounds);
    }
}