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
}