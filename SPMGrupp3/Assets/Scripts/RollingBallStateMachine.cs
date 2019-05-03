using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBallStateMachine : PhysicsStateMachine
{
    public Transform[] points;
    public int currentPoint;
    public float speed;

}