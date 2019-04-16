using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrellStateMachine : PhysicsStateMachine
{
    public void Move(Vector3 playerVelocity)
    {
        //Debug.Log("MOVE" + playerVelocity);
        velocity += playerVelocity.normalized * playerVelocity.magnitude;
        //Debug.Log(velocity);
    }

}
