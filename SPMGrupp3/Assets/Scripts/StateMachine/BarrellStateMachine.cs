using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrellStateMachine : PhysicsStateMachine
{

    [HideInInspector] public float moveMultiplier;

    public void Move(Vector3 playerVelocity)
    {
        //Debug.Log("MOVE" + playerVelocity);

        if(moveMultiplier <= 0)
        {
            moveMultiplier = 1;
        }

        velocity += playerVelocity.normalized * playerVelocity.magnitude * moveMultiplier;
        //Debug.Log(velocity);
    }

}
