//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateMachine : PhysicsStateMachine
{
    [HideInInspector] public float bulletDamage;
    [HideInInspector] public bool hasKnockback = false;

    public void SendBullet(Vector3 vel, float damage)
    {
        velocity += vel;
        bulletDamage = damage;
       // Debug.Log(velocity);
    }

}