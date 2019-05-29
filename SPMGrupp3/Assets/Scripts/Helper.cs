using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    public static float GetDistance(Vector3 currentPosition, Vector3 otherPosition)
    {
        return Mathf.Sqrt(GetDistanceOffset(currentPosition, otherPosition));
    }

    private static float GetDistanceOffset(Vector3 currentPosition, Vector3 otherPosition)
    {
        return (otherPosition - currentPosition).sqrMagnitude;
    }

    //https://forum.unity.com/threads/solved-how-to-get-rotation-value-that-is-in-the-inspector.460310/
    public static float GetCorrectAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        return angle;
    }

    public static Vector3 GetCorrectEulerVector(Vector3 originalAngle)
    {
        return new Vector3(GetCorrectAngle(originalAngle.x), GetCorrectAngle(originalAngle.y), GetCorrectAngle(originalAngle.z));
    }

    public static SaveModel GetSaveFile()
    {
        if (Directory.Exists("Saves") == false || File.Exists("Saves/save.binary") == false)
        {
            Debug.LogWarning("SAVEFILE == null");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);

        SaveModel model = (SaveModel)formatter.Deserialize(saveFile);

        saveFile.Close();
        return model;
    }


    //https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    public static Vector3 RotateAroundPivot(Vector3 position, Vector3 pivotPoint, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (position - pivotPoint) + pivotPoint;
    }
}