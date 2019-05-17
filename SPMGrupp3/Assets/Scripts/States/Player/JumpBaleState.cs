using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/JumpBaleState")]
public class JumpBaleState : PlayerBaseState
{
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float momentum;

    public override void Enter()
    {
        base.Update();
        
        Vector3 bounce = Vector3.up * jumpHeight;
        owner.velocity += bounce + momentum * owner.velocity.normalized;
    }

    public override void Update()
    {
        base.Update();
        if (IsGrounded())
            owner.Transition<WalkState>();
    }

    
}
