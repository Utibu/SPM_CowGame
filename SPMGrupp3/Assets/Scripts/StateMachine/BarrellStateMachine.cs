using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrellStateMachine : PhysicsStateMachine
{

    [HideInInspector] public float moveMultiplier;

    public void Move(Vector3 playerVelocity)
    {
        if(moveMultiplier <= 0)
        {
            moveMultiplier = 1;
        }

        playerVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);

        velocity += playerVelocity.normalized * playerVelocity.magnitude * moveMultiplier;
    }

}
