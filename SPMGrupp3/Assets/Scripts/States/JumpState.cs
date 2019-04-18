using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/JumpState")]
public class JumpState : PlayerBaseState
{

    //public float jumpForce = 5f;

    public override void Enter()
    {
        base.Enter();
        //Vector3 jumpMovement = Vector2.up * jumpForce * Time.deltaTime;
        //owner.velocity += jumpMovement;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
        {
            owner.Transition<ChargeState>();
        }

        if (IsGrounded())
        {
            owner.Transition<WalkState>();
        }

    }
}
