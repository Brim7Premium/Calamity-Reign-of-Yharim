using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Renderer objectRenderer;
    public virtual void SetDefaults() => objectRenderer = GetComponent<SpriteRenderer>();
    void Awake() => SetDefaults(); //SetDefaults BECOMES awake
    public float AngleTo(Vector2 Start, Vector2 Destination) => (float)Math.Atan2(Destination.y - Start.y, Destination.x - Start.x); //gets the angle from the starting position to the destination position as a float, can be turned into a vector2 using ToRotationVector2 from Utils.cs
    public Vector2 DirectionTo(Vector2 Start, Vector2 Destination) => Vector3.Normalize(Destination - Start); //gets the vector direction fron the starting position to the destination.b
    public Vector2 DirectionTo(Vector2 Destination, Entity entity) => Vector3.Normalize((Vector3)Destination - entity.transform.position); //why
    public abstract void Kill(); //just a base method that inherited classes must define

    public bool IsVisibleFromCamera()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, objectRenderer.bounds) == true;//null check
    }

}