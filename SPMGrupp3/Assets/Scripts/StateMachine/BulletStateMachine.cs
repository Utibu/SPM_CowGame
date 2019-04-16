using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateMachine : PhysicsStateMachine
{
    public float bulletDamage = 10f;
    public void SendBullet(Vector3 vel)
    {
        velocity += vel;
       // Debug.Log(velocity);
    }

}