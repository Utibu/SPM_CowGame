using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/JumpBaleState")]
public class JumpBaleState : PlayerBaseState
{
    public float jumpHeight = 3f;


    public override void Enter()
    {
        base.Update();
        
        Vector3 bounce = Vector3.up * jumpHeight * Time.deltaTime;
        owner.velocity += bounce;
    }

    public override void Update()
    {
        base.Update();
        if (IsGrounded())
            owner.Transition<WalkState>();
    }

    
}
