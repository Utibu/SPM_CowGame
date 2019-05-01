using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/WalkState")]
public class WalkState : PlayerBaseState
{
    private bool allowDash = true;
    public float dashThreshold = 10f;
    private float time = 0f;
    public override void Enter()
    {
        base.Enter();
        Debug.Log("enter walkstate");
        Debug.Log("velocity: " + owner.velocity);
        time = 0f;
        if (owner.lastState != null && owner.lastState.GetType() == typeof(DashState))
        {
            allowDash = false;
        }
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Update()
    {
        if(!allowDash)
        {
            time += Time.deltaTime;
            if(time % 60 > dashThreshold)
            {
                allowDash = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //owner.Transition<JumpState>();
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded() && (allowDash || ((PlayerStateMachine)owner).hasFreeDash))
        {
            owner.Transition<ChargeState>();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && IsGrounded() && ((PlayerStateMachine)owner).countdown <= 0)
        {
            //sideDash();
            owner.Transition<SideDashState>();
            ((PlayerStateMachine)owner).ResetCooldown();
        }

        base.Update();

    }
    //TESTSKIT
    /*
    private void sideDash()
    {
        HandleInput();
        inputDirection = new Vector3(direction.x, 0.0f, direction.z);
        Debug.Log(inputDirection);
        owner.velocity = inputDirection * dashDistance * Time.deltaTime;
    }
    */

    public override void ActOnCollision(Collider hitCollider)
    {
        base.ActOnCollision(hitCollider);
        if (hitCollider.tag.Equals("JumpBale"))
        {
            Collider col = GetGroundCollider();
            if (col != null)
            {
                if(col.tag.Equals("JumpBale"))
                {
                    owner.Transition<JumpBaleState>();

                } else
                {
                    if (hitCollider.GetComponent<BarrellStateMachine>() != null)
                    {
                        hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
                    }
                }
            }
        }
            
    }

    public override void ActOnTrigger(Collider hitCollider)
    {
        base.ActOnTrigger(hitCollider);
        Interactable interactable = hitCollider.GetComponent<Interactable>();
        if (interactable != null)
        {
            //Show E-button on screen
            if(Input.GetKeyDown(KeyCode.E))
            {
                owner.Transition<InteractState>();
                interactable.InvokeInteraction();
            }
            
        }
    }

}
