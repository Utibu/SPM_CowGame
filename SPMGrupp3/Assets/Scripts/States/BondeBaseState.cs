//Main Author: Sofia Kauko
//Secondary Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondeBaseState : EnemyGeneralState
{

    protected Peasant owner;
    public float speed;


    // for collision
    private Vector3 movement;


    public  override void Update()
    {
        if (owner.isDying)
        {
            owner.Transition<BondeStunState>();
            owner.isDying = false;
            return;
        }

        if(owner.ShouldGoAlive)
        {
            owner.Transition<BondePatrolState>();
            owner.ShouldGoAlive = false;
            return;
        }
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (Peasant)stateMachine;
    }

    public override void Enter()
    {
        
    }
}
