//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhysicsStateMachine : StateMachine
{
    // Start is called before the first frame update
    public Vector3 velocity = Vector3.zero;
    //[HideInInspector] public CapsuleCollider objectCollider;
    public LayerMask collisionMask;
    public LayerMask triggerMask;
    [HideInInspector] public BoxCollider objectCollider;

    public float skinWidth = 0.01f;
    public float groundCheckDistance = 0.2f;


    override protected void Awake()
    {
        base.Awake();
        objectCollider = GetComponent<BoxCollider>();
    }

    public override void Update()
    {
        base.Update();
    }

}

