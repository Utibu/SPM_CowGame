using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrellStateMachine : PhysicsStateMachine
{

    [HideInInspector] public float moveMultiplier;
    public bool movable;
    [SerializeField] private AudioClip[] bounceSounds;

    public void Move(Vector3 playerVelocity)
    {
        Debug.Log(playerVelocity);
        if(!movable)
        {
            return;
        }

        if(moveMultiplier <= 0)
        {
            moveMultiplier = 1;
        }

        playerVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);

        velocity += playerVelocity.normalized * playerVelocity.magnitude * moveMultiplier;
    }

    public AudioClip GetClip()
    {
        if (bounceSounds.Length == 0)
        {
            return null;
        }
        else
        {
            return bounceSounds[Random.Range(0, bounceSounds.Length)];
        }
    }

}
