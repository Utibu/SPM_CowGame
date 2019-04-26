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

    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (BondeRangedStateMachine)stateMachine;
    }

    public override void Enter()
    {
        
    }
}
