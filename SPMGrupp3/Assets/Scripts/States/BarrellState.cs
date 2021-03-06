﻿//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BarrellStateMachine/BarrellState")]
public class BarrellState : PhysicsBaseState
{

    public float moveMultiplier;

    private Vector3 originalTransform;

    public override void Enter()
    {
        base.Enter();
        originalTransform = ((BarrellStateMachine)owner).transform.position;
        ((BarrellStateMachine)owner).moveMultiplier = moveMultiplier;
    }

    public override void Update()
    {
        base.Update();

    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        if (hitCollider.tag.Equals("Enemy"))
        {
            Peasant bonde = hitCollider.GetComponent<Peasant>();
            Collider collider = owner.GetComponent<Collider>();
            bonde.PlayerDash(owner.velocity);
        }
        
        base.ActOnCollision(hitCollider, out skipCollision);
    }

    public override void ActOnTrigger(Collider hitCollider)
    {
        if (hitCollider.tag.Equals("Killzone") || hitCollider.tag.Equals("ObjectRespawner"))
        {
            Respawn();
        }
        /*
        if (hitCollider.tag.Equals("ObjectRespawner"))
        {
            Debug.Log("object collision");
            if (!hitCollider.bounds.Contains(((BarrellStateMachine)owner).transform.position))
            {
                Respawn();
            }
        }
        */
        base.ActOnTrigger(hitCollider);

    }

    private void Respawn()
    {
        ((BarrellStateMachine)owner).velocity = Vector3.zero;
        ((BarrellStateMachine)owner).transform.position = originalTransform;
    }
}
