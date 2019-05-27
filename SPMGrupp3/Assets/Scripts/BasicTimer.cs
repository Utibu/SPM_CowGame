using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTimer
{
    private float elapsedTime;
    private float length;
    private bool isCompleted;

    public BasicTimer(float lengthInSeconds)
    {
        elapsedTime = 0f;
        length = lengthInSeconds;
        isCompleted = false;
    }

    public void SetStartTime(float startTime)
    {
        elapsedTime = startTime;
    }

    public void Update(float deltaTime)
    {
        if(isCompleted == false)
        {
            elapsedTime += deltaTime;
            if(elapsedTime >= length)
            {
                isCompleted = true;
            }
        }
    }

    public bool IsCompleted(float deltaTime, bool resetOnCompletion, bool update = true)
    {
        if(isCompleted)
        {
            if (resetOnCompletion)
                Reset();
            return true;
        } else
        {
            if(update)
            {
                Update(deltaTime);
            }
            return false;
        }
        
    }

    public void Reset() {
        isCompleted = false;
        elapsedTime = 0f;
    }

    public float GetPercentage()
    {
        return elapsedTime / length;
    }

    public float GetDuration()
    {
        return length;
    }
   
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}