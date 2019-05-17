using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondeBaseState : EnemyGeneralState
{

    protected Bonde owner;
    public float speed;


    // for collision
    private Vector3 movement;


    public  override void Update()
    {
        if (owner.isDying)
        {
            owner.Transition<BondeStunState>();
            owner.isDying = false;
        }
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (Bonde)stateMachine;
    }

    public override void Enter()
    {
        //owner.agnes.speed = speed;
    }
}
