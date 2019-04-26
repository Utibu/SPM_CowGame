using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateMachine : PhysicsStateMachine
{
    [HideInInspector] public float bulletDamage;
    public void SendBullet(Vector3 vel, float damage)
    {
        velocity += vel;
        bulletDamage = damage;
       // Debug.Log(velocity);
    }

}