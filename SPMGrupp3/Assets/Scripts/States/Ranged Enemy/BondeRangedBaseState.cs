//Main Author: Niklas Almqvist
//Secondary Author: Sofia Kauko
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondeRangedBaseState : EnemyGeneralState
{

    protected BondeRangedStateMachine owner;
    public float speed;

    // for collision
    private Vector3 movement;


    public override void Update()
    {
        if (owner.isDying)
        {
            owner.Transition<BondeRangedStunState>();
            owner.isDying = false;
            return;
        }

        if (owner.ShouldGoAlive)
        {
            owner.Transition<BondeRangedPatrolState>();
            owner.ShouldGoAlive = false;
            return;
        }
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (BondeRangedStateMachine)stateMachine;
    }

    public override void Enter()
    {
        //owner.agnes.speed = speed;
    }
}
