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
        
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (Bonde)stateMachine;
    }

    public override  void Enter()
    {

    }
}
