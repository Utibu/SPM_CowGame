using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/ChargeState")]
public class ChargeState : PlayerBaseState
{

    //public float velocityToDash;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Update()
    {
        base.Update();
        owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.magenta;

        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded() && owner.velocity.magnitude > ((PlayerStateMachine)owner).velocityToDash)
        {
            owner.Transition<DashState>();
        }

        if (!Input.GetKey(KeyCode.LeftShift) || !IsGrounded())
        {
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.white;
            owner.Transition<WalkState>();
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            owner.Transition<JumpState>();
        }



    }
}
