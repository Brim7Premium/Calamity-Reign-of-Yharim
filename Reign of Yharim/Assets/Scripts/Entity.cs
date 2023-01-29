using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public bool active;
    public Vector2 velocity;
    public float AngleTo(Vector2 Destination) => (float)Math.Atan2(Destination.y - transform.position.y, Destination.x - transform.position.x);
    public Vector2 DirectionTo(Vector2 Destination) => Vector3.Normalize((Vector3)Destination - transform.position);//self explanitory
}