using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static Vector2 getNormal(Vector2 force, Vector2 normal)
    {
        float dot = Vector2.Dot(force, normal);
        if(dot > 0f)
        {
            dot = 0f;
        }
        Vector2 projection = dot * normal;
        return -projection;
    }

    public static Vector3 getNormal(Vector3 force, Vector3 normal)
    {
        float dot = Vector3.Dot(force, normal);
        if (dot > 0f)
        {
            dot = 0f;
        }
        Vector3 projection = dot * normal;
        return -projection;
    }

    public static bool IsWithinDistance(Vector3 currentPosition, Vector3 otherPosition, float distance)
    {
        return GetDistanceOffset(currentPosition, otherPosition) < distance * distance;
    }

    private static float GetDistanceOffset(Vector3 currentPosition, Vector3 otherPosition)
    {
        return (otherPosition - currentPosition).sqrMagnitude;
    }


    //https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    public static Vector3 RotateAroundPivot(Vector3 position, Vector3 pivotPoint, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (position - pivotPoint) + pivotPoint;
    }
}