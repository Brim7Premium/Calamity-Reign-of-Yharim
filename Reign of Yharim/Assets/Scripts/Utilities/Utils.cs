using System;
using UnityEngine;

public static class Utils
{
    public static float ToRotation(this Vector2 v) => (float)Math.Atan2(v.y, v.x);

    public static Vector2 ToRotationVector2(this float f) => new((float)Math.Cos(f), (float)Math.Sin(f));
    public static Vector2 Normalize(Vector2 value)
    {
        float num = 1f / (float)Math.Sqrt(value.x * value.x + value.y * value.y);
        value.x *= num;
        value.y *= num;
        return value;
    }
    public static float Distance(this Vector2 Origin, Vector2 Target)
    {
        return Vector2.Distance(Origin, Target);
    }
    public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default)
    {
        float num = (float)Math.Cos(radians);
        float num2 = (float)Math.Sin(radians);
        Vector2 vector = spinningpoint - center;
        Vector2 result = center;
        result.x += vector.x * num - vector.y * num2;
        result.y += vector.x * num2 + vector.y * num;
        return result;
    }
    public static Vector2 RotateTowards(this Vector2 originalVector, float idealAngle, float angleIncrement, bool returnUnitVector = false)
    {
        Vector2 newDirection = originalVector.ToRotation().AngleTowards(idealAngle, angleIncrement).ToRotationVector2();
        if (!returnUnitVector)
            return newDirection * originalVector.Length();
        return newDirection;
    }
    public static float AngleTowards(this float curAngle, float targetAngle, float maxChange)
    {
        curAngle = WrapAngle(curAngle);
        targetAngle = WrapAngle(targetAngle);
        if (curAngle < targetAngle)
        {
            if (targetAngle - curAngle > MathF.PI)
            {
                curAngle += MathF.PI * 2f;
            }
        }
        else if (curAngle - targetAngle > MathF.PI)
        {
            curAngle -= MathF.PI * 2f;
        }

        curAngle += Clamp(targetAngle - curAngle, 0f - maxChange, maxChange);
        return WrapAngle(curAngle);
    }
    public static float Clamp(float value, float min, float max)
    {
        value = ((value > max) ? max : value);
        value = ((value < min) ? min : value);
        return value;
    }
    public static float WrapAngle(float angle)
    {
        if (angle > -MathF.PI && angle <= MathF.PI)
        {
            return angle;
        }

        angle %= MathF.PI * 2f;
        if (angle <= -MathF.PI)
        {
            return angle + MathF.PI * 2f;
        }

        if (angle > MathF.PI)
        {
            return angle - MathF.PI * 2f;
        }

        return angle;
    }
    public static float Length(this Vector2 vector)
    {
        return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y);
    }
}
