﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseState : EnemyGeneralState
{

    protected BossStateMachine owner;
    public float speed;

    // for collision
    private Vector3 movement;


    public override void Update()
    {

    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (BossStateMachine)stateMachine;
    }

    public override void Enter()
    {

    }
}
