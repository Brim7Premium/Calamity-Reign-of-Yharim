using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Renderer objectRenderer;
    public virtual void SetDefaults() => objectRenderer = GetComponent<Renderer>();
    void Awake() => SetDefaults();
    public float AngleTo(Vector2 Destination) => (float)Math.Atan2(Destination.y - transform.position.y, Destination.x - transform.position.x); 
    public Vector2 DirectionTo(Vector2 Destination) => Vector3.Normalize((Vector3)Destination - transform.position);
    public Vector2 DirectionTo(Vector2 Destination, Entity entity) => Vector3.Normalize((Vector3)Destination - entity.transform.position);

    public bool IsVisibleFromCamera()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, objectRenderer.bounds) == true;//null check
    }

}